import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class EnvironmentService {
  public apiUrl: string = environment.apiUrl;
  public domainUrl: string = environment.apiUrl.replace('api', '');
}
