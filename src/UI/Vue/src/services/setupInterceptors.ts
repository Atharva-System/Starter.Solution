import tokenService from './token.service';
import axiosInstance from './api';

const setup = () => {
    axiosInstance.interceptors.request.use(
        (config: any) => {
            const token = tokenService.getToken();
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

            if (originalConfig.url !== '/signin' && err.response) {
                // Access Token was expired
                if (err.response.status === 401 && !originalConfig._retry) {
                    originalConfig._retry = true;

                    try {
                        const rs = await axiosInstance.post('/auth/refreshtoken', {
                            refreshToken: tokenService.getRefreshToken(),
                        });

                        const { token, refreshToken } = rs.data;

                        tokenService.setToken(token);
                        tokenService.setRefreshToken(refreshToken);

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
