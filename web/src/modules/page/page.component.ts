import { ChangeDetectionStrategy, Component, ContentChild, Input } from '@angular/core';
import { PageHeaderComponent } from './page-header/page-header.component';
import { PageHeaderContentComponent } from './page-header/page-header-content/page-header-content.component';
import { PageState } from './page-state';

@Component({
  selector: 'bip-page[pageState]',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PageComponent {
  @ContentChild(PageHeaderComponent) pageHeader?: PageHeaderContentComponent;

  @Input() pageState!: PageState;

  @Input() paddings = true;

  PageState = PageState;
}
