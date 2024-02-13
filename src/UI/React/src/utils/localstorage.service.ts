import * as jwtDecode from "jwt-decode";
import { StorageKey } from "./common/constants";
import { TokenData } from "./types/token.data";
import { TokenClaims } from "./types/token.claims";

class LocalStorageService {
  private static instance: LocalStorageService;

  private constructor() {}

  public static getService(): LocalStorageService {
    if (!LocalStorageService.instance) {
      LocalStorageService.instance = new LocalStorageService();
    }
    return LocalStorageService.instance;
  }

  public setToken(tokenData: TokenData): void {
    localStorage.setItem(StorageKey.token, JSON.stringify(tokenData));
    this.setUser();
  }

  public getAccessToken(): string | null {
    const tokenDataString = localStorage.getItem(StorageKey.token);
    if (tokenDataString) {
      const tokenData: TokenData = JSON.parse(tokenDataString);
      return tokenData.token;
    }
    return null;
  }

  public getRefreshToken(): string | null {
    const tokenDataString = localStorage.getItem(StorageKey.token);
    if (tokenDataString) {
      const tokenData: TokenData = JSON.parse(tokenDataString);
      return tokenData.refreshToken;
    }
    return null;
  }

  public clearToken(): void {
    localStorage.removeItem(StorageKey.token);
    localStorage.removeItem(StorageKey.userInfo);
  }

  isAuthenticated(): boolean {
    const token = this.getAccessToken();
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

  setUser() {
    localStorage.setItem(
      StorageKey.userInfo,
      JSON.stringify(this.decodeToken(this.getAccessToken() ?? ""))
    );
  }

  getUser(): TokenClaims | null {
    const userInfo = localStorage.getItem(StorageKey.userInfo);
    if (!userInfo) return null;
    return JSON.parse(userInfo);
  }

  updateStorageUserInfo(name: string, email: string) {
    const token = this.getAccessToken();
    if (token) {
      const user = this.decodeToken(token);
      if (user) {
        user.name = name;
        user.email = email;
        localStorage.setItem(StorageKey.userInfo, JSON.stringify(user));
      }
    }
  }
}

export default LocalStorageService;
