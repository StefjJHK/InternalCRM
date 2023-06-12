import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AppRoutes } from '../../routing/app-routes';

@Component({
  selector: 'bip-permissions-denied-page',
  templateUrl: './permissions-denied-page.component.html',
  styleUrls: ['./permissions-denied-page.component.less'],
})
export class PermissionsDeniedPageComponent {
  constructor(private router: Router) {}

  profile() {
    this.router.navigate([AppRoutes.Dashboard]);
  }
}
