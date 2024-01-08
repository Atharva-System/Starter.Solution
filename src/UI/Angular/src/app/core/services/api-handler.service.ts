import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, map, throwError } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ApiHandlerService {
  public apiUrl: string = environment.apiUrl;
  constructor(public httpClient: HttpClient) {}

  // GET
  public get(
    path: string,
    params: HttpParams = new HttpParams(),
  ): Observable<any> {
    return this.httpClient
      .get(`${this.apiUrl}${path}`, { params })
      .pipe(catchError(this.formatErrors));
  }

  //PUT
  public put(path: string, body: object = {}): Observable<any> {
    return this.httpClient
      .put(`${this.apiUrl}${path}`, JSON.stringify(body))
      .pipe(catchError(this.formatErrors));
  }

  //POST
  public post(path: string, body: object = {}): Observable<any> {
    return this.httpClient
      .post(`${this.apiUrl}${path}`, body)
      .pipe(map((response: any) => response));
  }

  //DELETE
  public delete(path: string): Observable<any> {
    return this.httpClient
      .delete(`${this.apiUrl}${path}`)
      .pipe(catchError(this.formatErrors));
  }

  //Format Error
  public formatErrors(error: any): Observable<any> {
    return throwError(() => new Error(error.error));
  }
}
