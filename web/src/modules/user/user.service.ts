import { Inject, Injectable } from '@angular/core';
import { Environment, ENVIRONMENT } from '../environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AddUserRequest } from './add-user-request.model';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(@Inject(ENVIRONMENT) private environment: Environment, private httpClient: HttpClient) {}

  getUsers(): Observable<number> {
    return this.httpClient.get<number>(`${this.environment.identityUrl}/users`);
  }

  addUser(request: AddUserRequest): Observable<void> {
    return this.httpClient.post<void>(`${this.environment.identityUrl}/users`, request);
  }

  deleteUser(username: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.environment.identityUrl}/users/${username}`);
  }
}
