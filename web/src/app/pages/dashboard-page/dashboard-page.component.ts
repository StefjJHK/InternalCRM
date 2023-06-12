import { Component } from '@angular/core';
import { PageState } from '../../../modules/page/page-state';

@Component({
  selector: 'bip-dashboard-page',
  templateUrl: './dashboard-page.component.html',
  styleUrls: ['./dashboard-page.component.less'],
})
export class DashboardPageComponent {
  pageState: PageState = PageState.loaded;
}
