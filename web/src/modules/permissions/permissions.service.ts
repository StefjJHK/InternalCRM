import { Injectable } from '@angular/core';
import { LocalStorageService } from '../local-storage/local-storage.service';
import { Permissions } from './permissions.model';
import { accessAvailable, PartialPermissions } from '../../utils/permissions-utils';

@Injectable({
  providedIn: 'root',
})
export class PermissionsService {
  constructor(private readonly localStorageService: LocalStorageService) {}

  setPermissions(permissions: Permissions) {
    const localStorage = this.localStorageService.getLocalStorage();

    this.localStorageService.setLocalStorage({
      ...localStorage,
      permissions,
    });
  }

  getPermissions(): Permissions | null {
    return this.localStorageService.getLocalStorage().permissions ?? null;
  }

  containsPermissions(expectedPermissions: PartialPermissions) {
    const actualPermissions = this.getPermissions();

    return actualPermissions && accessAvailable(expectedPermissions, actualPermissions);
  }
}
