import { Injectable, inject, signal } from '@angular/core';
import { ApiHandlerService } from '../../../core/services/api-handler.service';
import { APIs } from '../../../shared/constants/api-endpoints';
import { IUpdateUser } from '../models/update-user.interface';
import {
  IUserProfile,
  IUserProfileSignal,
} from '../models/user-profile.interface';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  profileSignal = signal<IUserProfileSignal | undefined>(undefined);

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

  getProfileDetail() {
    return this.apiHandlerService.get(APIs.getProfileDetails);
  }

  updateProfile(params: IUserProfile) {
    return this.apiHandlerService.put(APIs.updateProfile + params.id, params);
  }

  setProfileSignal(param: IUserProfileSignal) {
    this.profileSignal.set(param);
  }

  sendMessage(userId: string, message: string) {
    return this.apiHandlerService.post(APIs.sendMessage, { userId, message });
  }
}
