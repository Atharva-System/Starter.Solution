// auth.guard.ts
import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';
import { authPaths } from '../../shared/constants/routes';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard {
  authenticationService = inject(AuthenticationService);
  router = inject(Router);

  canActivate() {
    if (this.authenticationService.isAuthenticated()) {
      return true;
    } else {
      const refreshToken = this.authenticationService.getRefreshToken();
      if (refreshToken) {
        return this.authenticationService
          .refreshToken()
          .toPromise()
          .then((response: any) => {
            this.authenticationService.setToken(response.token);
            this.authenticationService.setRefreshToken(response.refreshToken);
            return true;
          })
          .catch(() => {
            this.router.navigate(['/' + authPaths.signin]);
            return false;
          });
      } else {
        this.router.navigate(['/' + authPaths.signin]);
        return false;
      }
    }
  }
}
