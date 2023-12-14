import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HttpEvent,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { AuthenticationService } from '../services/authentication.service';
import { APIs } from '../../shared/constants/api-endpoints';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthenticationService) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler,
  ): Observable<HttpEvent<any>> {
    if (
      this.authService.isAuthenticated() &&
      !this.isRequestUrlAllowAnonymous(request.url)
    ) {
      const authToken = this.authService.getToken();
      request = this.addTokenHeader(request, authToken);
    }

    return next.handle(request).pipe(
      catchError((error) => {
        if (error.status === 401 && this.authService.isAuthenticated()) {
          return this.handleTokenExpired(request, next);
        } else {
          return throwError(error);
        }
      }),
    );
  }

  private addTokenHeader(
    request: HttpRequest<any>,
    token: string | null,
  ): HttpRequest<any> {
    return token
      ? request.clone({
          setHeaders: {
            Authorization: `Bearer ${token}`,
          },
        })
      : request;
  }

  private handleTokenExpired(
    request: HttpRequest<any>,
    next: HttpHandler,
  ): Observable<HttpEvent<any>> {
    return this.authService.refreshTokens().pipe(
      switchMap((response) => {
        this.authService.setToken(response.token);
        const newRequest = this.addTokenHeader(request, response.token);
        return next.handle(newRequest);
      }),
      catchError((refreshError) => {
        this.authService.signOut();
        return throwError(refreshError);
      }),
    );
  }

  private isRequestUrlAllowAnonymous(url: string): boolean {
    const anonymousEndpoints = [
      APIs.signinApi,
      APIs.signupApi,
      APIs.refreshTokenApi,
    ];
    return anonymousEndpoints.some((endpoint) => url.endsWith(endpoint));
  }
}
