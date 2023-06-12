import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'bip-page-layout',
  templateUrl: './page-layout.component.html',
  styleUrls: ['./page-layout.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PageLayoutComponent {}
