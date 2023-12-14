import { Injectable, inject } from '@angular/core';
import { ApiHandlerService } from '../../../core/services/api-handler.service';
import { APIs } from '../../../shared/constants/api-endpoints';
import { IRegistrationRequest } from '../models/registration-request.interface';
import { IAuthenticationRequest } from '../models/authentication-request.interface';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  apiHandlerService = inject(ApiHandlerService);

  signup(obj: IRegistrationRequest) {
    return this.apiHandlerService.post(APIs.signupApi, obj);
  }

  signin(obj: IAuthenticationRequest) {
    return this.apiHandlerService.post(APIs.signinApi, obj);
  }
}
