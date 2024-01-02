import { Injectable, inject } from '@angular/core';
import { ApiHandlerService } from '../../../core/services/api-handler.service';
import { APIs } from '../../../shared/constants/api-endpoints';
import { IUpdateUser } from '../models/update-user.interface';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  apiHandlerService = inject(ApiHandlerService);

  getUsers(params: any) {
    return this.apiHandlerService.post(APIs.searchUserApi, params);
  }

  getUser(id: string) {
    return this.apiHandlerService.get(APIs.getUserApi + id);
  }

  deleteUser(id: string) {
    return this.apiHandlerService.delete(APIs.deleteUserApi + id);
  }

  updateUsers(params: IUpdateUser) {
    return this.apiHandlerService.put(APIs.updateUserApi + params.id, params);
  }
}
