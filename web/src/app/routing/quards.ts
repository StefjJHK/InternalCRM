import { inject } from '@angular/core';
import { UserAccessService } from '../../modules/user/user-access.service';
import { ActivatedRouteSnapshot, Router, UrlTree } from '@angular/router';
import { AppRoutes } from './app-routes';
import { PermissionsService } from '../../modules/permissions/permissions.service';
import { accessAvailable } from '../../utils/permissions-utils';
import { Permissions } from '../../modules/permissions/permissions.model';

export class BipGuards {
  static authGuard(): boolean | UrlTree {
    const router = inject(Router);
    const userService = inject(UserAccessService);

    if (!userService.user) {
      return router.createUrlTree([AppRoutes.Unauthorized]);
    }

    return true;
  }

  static permissionsGuard(route: ActivatedRouteSnapshot): boolean | UrlTree {
    const router = inject(Router);
    const permissionService = inject(PermissionsService);

    const actualPermissions = permissionService.getPermissions();
    const expectedPermissions = route.data.permissions as Permissions | undefined;

    if (!expectedPermissions) {
      throw Error('permissionsGuard expects permissions prop in route.data');
    }

    if (!actualPermissions) {
      throw Error('permissionsGuard must use after authGuard');
    }

    if (accessAvailable(expectedPermissions, actualPermissions)) {
      return true;
    }

    return router.createUrlTree([AppRoutes.PermissionsDenied]);
  }
}
