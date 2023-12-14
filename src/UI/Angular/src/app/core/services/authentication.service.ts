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

  signOut() {
    this.clearToken();
    this.router.navigate(['/' + authPaths.signin]);
  }

  refreshTokens() {
    const requestBody = {
      token: this.getToken() ?? '',
      refreshToken: this.getRefreshToken() ?? '',
    };
    return this.apiHandlerService.post(APIs.refreshTokenApi, requestBody);
  }
}
