import axiosInstance from "../../../utils/api.service";
import { APIs } from "../../../utils/common/api-paths";
import LocalStorageService from "../../../utils/localstorage.service";

const localStorageService = LocalStorageService.getService();

class AuthService {
  async login(email: string, password: string) {
    const response = await axiosInstance.post(APIs.signinApi, {
      email,
      password,
    });
    if (response.data) {
      localStorageService.setToken(response.data.data);
    }
    return response.data;
  }

  logout() {
    localStorageService.clearToken();
  }
}

export default new AuthService();
