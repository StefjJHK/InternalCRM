import { InjectionToken } from '@angular/core';

export const ENVIRONMENT = new InjectionToken<Environment>('environment');

export interface Environment {
  env: 'development' | 'production';
  apiUrl: string;
  identityUrl: string;
  prodMode: boolean;
}
