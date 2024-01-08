import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpStatusCode,
} from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { AlertService } from '../../shared/services/alert.service';
import { AlertNotification } from '../../shared/constants/constants';
import { AuthenticationService } from '../services/authentication.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  alertService = inject(AlertService);
  authenticationService = inject(AuthenticationService);

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler,
  ): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error) {
          if (error.status == 0) {
            this.authenticationService.signOut();
          } else if (error.error.Messages && error.error.Messages.length > 0) {
            this.alertService.showMessage(
              error.error.Messages[0],
              AlertNotification.type.error,
            );
          } else if (error.error.Message) {
            this.alertService.showMessage(
              error.error.Message,
              AlertNotification.type.error,
            );
          } else {
            this.alertService.showMessage(
              'Something went wrong, please try again!',
              AlertNotification.type.error,
            );
          }
        }
        return throwError(() => error);
      }),
    );
  }
}
