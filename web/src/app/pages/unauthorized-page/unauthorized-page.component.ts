import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AppRoutes } from '../../routing/app-routes';

@Component({
  selector: 'bip-unauthorized-page',
  templateUrl: './unauthorized-page.component.html',
  styleUrls: ['./unauthorized-page.component.less'],
})
export class UnauthorizedPageComponent {
  constructor(private router: Router) {}

  login() {
    this.router.navigate([AppRoutes.Login]);
  }
}
