import { Injectable, inject } from '@angular/core';
import {
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HttpEvent,
  HttpStatusCode,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { AuthenticationService } from '../services/authentication.service';
import { APIs } from '../../shared/constants/api-endpoints';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  authenticationService = inject(AuthenticationService);

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler,
  ): Observable<HttpEvent<any>> {
    request = request.clone({
      setHeaders: {
        'Content-Type': 'application/json',
        'request-source' : 'angular'
      },
    });

    if (
      this.authenticationService.isAuthenticated() &&
      !this.isRequestUrlAllowAnonymous(request.url)
    ) {
      const authToken = this.authenticationService.getToken();
      request = this.addTokenHeader(request, authToken);
    }

    return next.handle(request).pipe(
      catchError((error) => {
        if (
          error.status === HttpStatusCode.Unauthorized &&
          !this.authenticationService.isAuthenticated()
        ) {
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
    return this.authenticationService.refreshToken().pipe(
      switchMap((response) => {
        this.authenticationService.setToken(response.token);
        this.authenticationService.setRefreshToken(response.refreshToken);
        const newRequest = this.addTokenHeader(request, response.token);
        return next.handle(newRequest);
      }),
      catchError((refreshError) => {
        this.authenticationService.signOut();
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
