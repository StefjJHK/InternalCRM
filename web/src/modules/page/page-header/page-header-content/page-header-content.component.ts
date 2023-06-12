import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'bip-page-header-content',
  templateUrl: './page-header-content.component.html',
  styleUrls: ['./page-header-content.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PageHeaderContentComponent {}
