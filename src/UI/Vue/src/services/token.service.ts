import { refreshTokenStorageKey, tokenStorageKey, userInfoStorageKey } from '@/common/constants';
import * as jwtDecode from 'jwt-decode';

export interface TokenClaims {
    name: string;
    aud: string;
    email: string;
    exp: number;
    iss: string;
    jti: string;
    roles: string;
    sub: string;
    uid: string;
}

class TokenService {
    setToken(token: string): void {
        localStorage.setItem(tokenStorageKey, token);
        this.setUser();
    }

    setUser() {
        localStorage.setItem(userInfoStorageKey, JSON.stringify(this.decodeToken(this.getToken() ?? '')));
    }

    getToken(): string | null {
        return localStorage.getItem(tokenStorageKey);
    }

    setRefreshToken(refreshToken: string): void {
        localStorage.setItem(refreshTokenStorageKey, refreshToken);
    }

    getRefreshToken(): string | null {
        return localStorage.getItem(refreshTokenStorageKey);
    }

    clearToken(): void {
        localStorage.removeItem(tokenStorageKey);
        localStorage.removeItem(refreshTokenStorageKey);
        localStorage.removeItem(userInfoStorageKey);
    }

    isAuthenticated(): boolean {
        const token = this.getToken();
        return !!token && !this.isTokenExpired(token);
    }

    isTokenExpired(token: string): boolean {
        const decodedToken: any = this.decodeToken(token);
        const expirationTime = decodedToken.exp * 1000;
        return Date.now() >= expirationTime;
    }

    decodeToken(token: string): TokenClaims | null {
        if (!token) return null;
        const decodedToken: TokenClaims = jwtDecode.jwtDecode(token);
        return decodedToken;
    }

    getUser(): TokenClaims | null {
        const userInfo = localStorage.getItem(userInfoStorageKey);
        if (!userInfo) return null;
        return JSON.parse(userInfo);
    }

    signOut() {
        this.clearToken();
    }

    updateStorageUserInfo(name: string, email: string) {
        const token = this.getToken();
        if (token) {
            const user = this.decodeToken(token);
            if (user) {
                user.name = name;
                user.email = email;
                localStorage.setItem(userInfoStorageKey, JSON.stringify(user));
            }
        }
    }
}

export default new TokenService();
