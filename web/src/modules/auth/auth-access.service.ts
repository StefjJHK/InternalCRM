import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpStatusCode } from '@angular/common/http';
import { LoginRequest } from './login-request.model';
import { errorHandlerByHttp, handleErrors } from '../operators/handleErrors';
import { EMPTY, Observable, tap } from 'rxjs';
import { Environment, ENVIRONMENT } from '../environment';
import { LoginError } from './login-error';
import { UserAccessService } from '../user/user-access.service';
import { LoginResponse } from './login-reponse.model';
import { PermissionsService } from '../permissions/permissions.service';

@Injectable({
  providedIn: 'root',
})
export class AuthAccessService {
  constructor(
    private readonly httpClient: HttpClient,
    private readonly userService: UserAccessService,
    private readonly permissionService: PermissionsService,
    @Inject(ENVIRONMENT) private readonly environment: Environment,
  ) {}

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.httpClient.post(`${this.environment.identityUrl}/token`, request).pipe(
      handleErrors([
        errorHandlerByHttp((err) => new LoginError(err.error.title), {
          statusCode: HttpStatusCode.BadRequest,
        }),
      ]),
      tap((loginResponse: LoginResponse) => {
        this.userService.setUser(loginResponse.user);
        this.permissionService.setPermissions(loginResponse.permissions);
      }),
    );
  }

  logout(): Observable<void> {
    this.userService.clearUser();

    return EMPTY;
  }
}
