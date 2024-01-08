import type { IUserInfo } from "@/types";

class TokenService {
    getLocalRefreshToken(): string | null {
        const userString = localStorage.getItem('user');
        const user = userString ? JSON.parse(userString) : null;
        return user?.refreshToken ?? null;
    }

    getLocalAccessToken() {
        const userString = localStorage.getItem('user');
        const user = userString ? JSON.parse(userString) : null;
        return user?.token;
    }

    updateLocalAccessToken(token : any) {
        const userString = localStorage.getItem('user');
        const user = userString ? JSON.parse(userString) : null;
        user.token = token;
        localStorage.setItem('user', JSON.stringify(user));
    }

    getUser() {
        const userString = localStorage.getItem('user');
        return userString ? JSON.parse(userString) : null;
    }

    setUser(user: IUserInfo) {
        console.log(JSON.stringify(user));
        localStorage.setItem('user', JSON.stringify(user));
    }

    removeUser() {
        localStorage.removeItem('user');
    }
}

export default new TokenService();
