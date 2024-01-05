import { Injectable, inject } from '@angular/core';
import { ApiHandlerService } from '../../../core/services/api-handler.service';
import { APIs } from '../../../shared/constants/api-endpoints';
import { ICreateProject } from '../models/create-project.interface';
import { IUpdateProject } from '../models/update-project.interface';

@Injectable({
  providedIn: 'root',
})
export class ProjectService {
  apiHandlerService = inject(ApiHandlerService);

  getProjects(params: any) {
    return this.apiHandlerService.post(APIs.searchProjectsApi, params);
  }

  deleteProject(id: string) {
    return this.apiHandlerService.delete(APIs.deleteProjectApi + id);
  }

  updateProject(params: IUpdateProject) {
    return this.apiHandlerService.put(
      APIs.updateProjectApi + params.id,
      params,
    );
  }

  getProject(id: string) {
    return this.apiHandlerService.get(APIs.getProjectApi + id);
  }

  createProject(obj: ICreateProject) {
    return this.apiHandlerService.post(APIs.createProjectApi, obj);
  }
}
