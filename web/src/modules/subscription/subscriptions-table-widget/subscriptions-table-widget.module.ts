import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SubscriptionsTableWidgetComponent } from './subscriptions-table-widget.component';
import { TableWidgetModule } from '../../table-widget/table-widget.module';
import { NzTableModule } from 'ng-zorro-antd/table';

@NgModule({
  declarations: [SubscriptionsTableWidgetComponent],
  imports: [CommonModule, TableWidgetModule, NzTableModule],
  exports: [SubscriptionsTableWidgetComponent],
})
export class SubscriptionsTableWidgetModule {}
