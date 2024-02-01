import { Injectable, inject } from '@angular/core';
import { ApiHandlerService } from '../../../core/services/api-handler.service';
import { APIs } from '../../../shared/constants/api-endpoints';
import { IUpdateTask } from '../models/update-task.interface';
import { ICreateTask } from '../models/create-task.interface';

@Injectable({
  providedIn: 'root',
})
export class TaskService {
  apiHandlerService = inject(ApiHandlerService);

  getTasks(params: any) {
    return this.apiHandlerService.post(APIs.searchTasksApi, params);
  }

  deleteTask(id: string) {
    return this.apiHandlerService.delete(APIs.deleteTaskApi + id);
  }

  updateTask(params: IUpdateTask) {
    return this.apiHandlerService.put(APIs.updateTaskApi + params.id, params);
  }

  getTask(id: string) {
    return this.apiHandlerService.get(APIs.getTaskApi + id);
  }

  createTask(obj: ICreateTask) {
    return this.apiHandlerService.post(APIs.createTaskApi, obj);
  }

  getTaskStatusList() {
    return this.apiHandlerService.get(APIs.getTaskStatusListApi);
  }

  getTaskPriorityList() {
    return this.apiHandlerService.get(APIs.getTaskPriorityListApi);
  }

  getAssignees() {
    return this.apiHandlerService.get(APIs.getAssigneeListApi);
  }

  getProjects() {
    return this.apiHandlerService.get(APIs.getProjectListApi);
  }
}
