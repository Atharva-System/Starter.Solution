import { Injectable, inject } from '@angular/core';
import { StorageKey } from '../../shared/constants/constants';
import { Router } from '@angular/router';
import { authPaths } from '../../shared/constants/routes';
import { jwtDecode } from 'jwt-decode';
import { ApiHandlerService } from './api-handler.service';
import { APIs } from '../../shared/constants/api-endpoints';

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

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  apiHandlerService = inject(ApiHandlerService);

  router = inject(Router);

  setToken(token: string): void {
    localStorage.setItem(StorageKey.tokenKey, token);
    this.setUser();
  }

  setUser() {
    localStorage.setItem(
      StorageKey.userInfo,
      JSON.stringify(this.decodeToken(this.getToken() ?? '')),
    );
  }

  getToken(): string | null {
    return localStorage.getItem(StorageKey.tokenKey);
  }

  setRefreshToken(refreshToken: string): void {
    localStorage.setItem(StorageKey.refreshTokenKey, refreshToken);
  }

  getRefreshToken(): string | null {
    return localStorage.getItem(StorageKey.refreshTokenKey);
  }

  clearToken(): void {
    localStorage.removeItem(StorageKey.tokenKey);
    localStorage.removeItem(StorageKey.refreshTokenKey);
    localStorage.removeItem(StorageKey.userInfo);
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
    const decodedToken: TokenClaims = jwtDecode(token);
    return decodedToken;
  }

  getUser(): TokenClaims | null {
    return JSON.parse(localStorage.getItem(StorageKey.userInfo) ?? '');
  }

  signOut() {
    this.clearToken();
    this.router.navigate(['/' + authPaths.signin]);
  }

  refreshToken() {
    const requestBody = {
      token: this.getToken() ?? '',
      refreshToken: this.getRefreshToken() ?? '',
    };
    return this.apiHandlerService.post(APIs.refreshTokenApi, requestBody);
  }

  updateStorageUserInfo(name: string, email: string) {
    let token = this.getToken();
    if (token) {
      var user = this.decodeToken(token);
      if (user) {
        user.name = name;
        user.email = email;
        localStorage.setItem(StorageKey.userInfo, JSON.stringify(user));
      }
    }
  }
}
