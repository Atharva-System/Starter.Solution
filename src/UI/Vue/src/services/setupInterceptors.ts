import TokenService from './token.service';
import axiosInstance from "./api";

const setup = () => {
    
    axiosInstance.interceptors.request.use(
        (config: any) => {
            const token = TokenService.getLocalAccessToken();
            if (token) {
                config.headers['Authorization'] = 'Bearer ' + token;
            }
            return config;
        },
        (error: any) => {
            return Promise.reject(error);
        },
    );

    axiosInstance.interceptors.response.use(
        (res: any) => {
            return res;
        },
        async (err: any) => {
            const originalConfig = err.config;

            if (originalConfig.url !== '/auth/signin' && err.response) {
                // Access Token was expired
                if (err.response.status === 401 && !originalConfig._retry) {
                    originalConfig._retry = true;

                    try {
                        const rs = await axiosInstance.post('/auth/refreshtoken', {
                            refreshToken: TokenService.getLocalRefreshToken(),
                        });

                        const { accessToken } = rs.data;

                        TokenService.updateLocalAccessToken(accessToken);

                        return axiosInstance(originalConfig);
                    } catch (_error) {
                        return Promise.reject(_error);
                    }
                }
            }

            return Promise.reject(err);
        },
    );
};

export default setup;
