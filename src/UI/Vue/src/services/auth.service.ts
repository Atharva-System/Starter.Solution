import type { ILogin } from '@/types';
import api from './api';
import TokenService from './token.service';
import { signInApi } from '../common/api-paths';

class AuthService {
    async login({ email, password }: ILogin) {
        const response = await api.post(signInApi, {
            email,
            password,
        });
        if (response.data) {
            TokenService.setUser(response.data.data);
        }
        return response.data;
    }

    logout() {
        TokenService.removeUser();
    }
}

export default new AuthService();
