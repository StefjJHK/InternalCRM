import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzTabsModule } from 'ng-zorro-antd/tabs';
import { TabsetComponent } from './tabset.component';
import { TabsetItemDirective } from './tabset-item.directive';

@NgModule({
  declarations: [TabsetComponent, TabsetItemDirective],
  imports: [CommonModule, NzTabsModule],
  exports: [TabsetComponent, TabsetItemDirective],
})
export class TabsetModule {}
