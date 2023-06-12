import { Component, ContentChildren, HostBinding, QueryList } from '@angular/core';
import { TabsetItemDirective } from './tabset-item.directive';

@Component({
  selector: 'bip-tabset',
  templateUrl: './tabset.component.html',
  styleUrls: ['./tabset.component.less'],
})
export class TabsetComponent {
  @ContentChildren(TabsetItemDirective) tabs!: QueryList<TabsetItemDirective>;
}
