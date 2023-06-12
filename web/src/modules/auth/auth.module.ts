import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { AuthCredentialsPrefillInterceptor } from './auth-credentials-prefill.interceptor';

@NgModule({})
export class AuthModule {
  static forRoot(): ModuleWithProviders<AuthModule> {
    return {
      ngModule: AuthModule,
      providers: [
        {
          provide: HTTP_INTERCEPTORS,
          useClass: AuthCredentialsPrefillInterceptor,
          multi: true,
        },
      ],
    };
  }
}
