import { Injectable, inject } from '@angular/core';
import { ApiHandlerService } from '../../../core/services/api-handler.service';
import { APIs } from '../../../shared/constants/api-endpoints';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  apiHandlerService = inject(ApiHandlerService);

  signup(obj: any) {
    return this.apiHandlerService.post(APIs.signupApi, obj);
  }
}
