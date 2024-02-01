import type { ILogin } from '@/types';
import api from './api';
import tokenService from './token.service';
import { signInApi } from '../common/api-paths';

class AuthService {
    async login({ email, password }: ILogin) {
        const response = await api.post(signInApi, {
            email,
            password,
        });
        if (response.data) {
            tokenService.setToken(response.data.data.token);
            tokenService.setRefreshToken(response.data.data.refreshToken);
        }
        return response.data;
    }

    logout() {
        tokenService.signOut();
    }
}

export default new AuthService();
