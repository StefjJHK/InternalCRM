import { ChangeDetectionStrategy, Component, ContentChild, Input } from '@angular/core';
import { PageHeaderContentComponent } from './page-header-content/page-header-content.component';
import { Button } from '../../common-components/button/button.model';

@Component({
  selector: 'bip-page-header',
  templateUrl: './page-header.component.html',
  styleUrls: ['./page-header.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PageHeaderComponent {
  @Input() title?: string;

  @Input() buttons?: Button[];

  @Input() showBreadcrumb = false;

  @ContentChild(PageHeaderContentComponent) content?: PageHeaderContentComponent;
}
