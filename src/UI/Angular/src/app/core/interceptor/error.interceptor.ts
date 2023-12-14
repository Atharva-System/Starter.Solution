import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { AlertService } from '../../shared/services/alert.service';
import { AlertNotification } from '../../shared/constants/constants';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  alertService = inject(AlertService);

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler,
  ): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        this.alertService.showMessage(
          error.error.Messages[0],
          AlertNotification.type.error,
        );
        return throwError(() => error);
      }),
    );
  }
}
