import { Component } from '@angular/core';
import { AppRoutes } from '../../../routing/app-routes';
import { permissionsFrom } from '../../../../utils/permissions-utils';

@Component({
  selector: 'bip-side-menu',
  templateUrl: './side-menu.component.html',
  styleUrls: ['./side-menu.component.less'],
})
export class SideMenuComponent {
  routes = AppRoutes;

  permissionsFrom = permissionsFrom;
}
