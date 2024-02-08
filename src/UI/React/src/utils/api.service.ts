import axios from "axios";
import getApiUrl from "../envirenment/environment";
import LocalStorageService from "./localstorage.service";
import { useNavigate } from "react-router-dom";
import { authPaths } from "./common/route-paths";
import authService from "../pages/auth/utils/auth.service";
import messageService from "./message.service";
import { APIs } from "./common/api-paths";

const localStorageService = LocalStorageService.getService();
const apiUrl = getApiUrl();

const axiosInstance = axios.create({
  baseURL: apiUrl,
  headers: {
    "Content-Type": "application/json",
    "request-source": "react",
  },
});

// Request interceptor
axiosInstance.interceptors.request.use(
  (config) => {
    const token = localStorageService.getAccessToken();
    if (token && !isRequestUrlAllowAnonymous(config.url ?? "")) {
      config.headers["Authorization"] = "Bearer " + token;
    }
    return config;
  },
  (error) => {
    Promise.reject(error);
  }
);

// Response interceptor
axiosInstance.interceptors.response.use(
  (response) => {
    return response;
  },
  function (error) {
    const originalRequest = error.config;

    if (
      error.response.status === 401 &&
      originalRequest.url === `${apiUrl}/${APIs.refreshTokenApi}`
    ) {
      const navigate = useNavigate();
      navigate(authPaths.signin);
      return Promise.reject(error);
    }

    if (error.response.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;
      const refreshToken = localStorageService.getRefreshToken();
      return axiosInstance
        .post(`${apiUrl}/${APIs.refreshTokenApi}`, {
          refresh_token: refreshToken,
        })
        .then((res) => {
          if (res.status === 201) {
            localStorageService.setToken(res.data);
            axiosInstance.defaults.headers.common["Authorization"] =
              "Bearer " + localStorageService.getAccessToken();
            return axios(originalRequest);
          }
        });
    }
    return Promise.reject(error);
  }
);

axiosInstance.interceptors.response.use(
  (response) => response,
  (error) => {
    handleError(error);
    return Promise.reject(error);
  }
);

function handleError(error: any) {
  if (error.response) {
    handleResponseError(error.response);
  } else if (error.request) {
    console.error("Request error:", error.request);
  } else {
    console.error("Error:", error.message);
  }
}

function handleResponseError(response: any) {
  if (response.status === 0) {
    authService.logout();
  } else {
    const errorMessage =
      response.data?.Messages?.[0] ||
      response.data?.Message ||
      "Something went wrong, please try again!";
    messageService.showMessage(errorMessage, "error");
  }
}

function isRequestUrlAllowAnonymous(url: string): boolean {
  const anonymousEndpoints = [
    APIs.signinApi,
    APIs.signupApi,
    APIs.refreshTokenApi,
    APIs.forgotPasswordApi,
    APIs.resetPasswordApi,
  ];
  return anonymousEndpoints.some((endpoint) => url.endsWith(endpoint));
}

export default axiosInstance;
