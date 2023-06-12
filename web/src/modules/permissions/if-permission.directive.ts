import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { PermissionsService } from './permissions.service';
import { accessAvailable, PartialPermissions } from '../../utils/permissions-utils';

@Directive({
  selector: '[bipIfPermissions]',
})
export class IfPermissionsDirective {
  @Input() set bipIfPermissions(expectedPermission: PartialPermissions) {
    const actualPermissions = this.permissionService.getPermissions();

    if (actualPermissions && accessAvailable(expectedPermission, actualPermissions)) {
      this.viewContainer.createEmbeddedView(this.templateRef);
    } else {
      this.viewContainer.clear();
    }
  }

  constructor(
    private readonly templateRef: TemplateRef<any>,
    private readonly viewContainer: ViewContainerRef,
    private readonly permissionService: PermissionsService,
  ) {}
}
