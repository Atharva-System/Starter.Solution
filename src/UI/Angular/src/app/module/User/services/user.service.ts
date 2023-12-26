import { Injectable, inject } from '@angular/core';
import { ApiHandlerService } from '../../../core/services/api-handler.service';
import { APIs } from '../../../shared/constants/api-endpoints';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  apiHandlerService = inject(ApiHandlerService);

  getUsers(params: any) {
    return this.apiHandlerService.post(APIs.searchUserApi, params);
  }
}
