import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductCustomersWidgetComponent } from './product-customers-widget.component';
import { NzTableModule } from 'ng-zorro-antd/table';
import { TableWidgetModule } from '../../table-widget/table-widget.module';

@NgModule({
  declarations: [ProductCustomersWidgetComponent],
  imports: [CommonModule, NzTableModule, TableWidgetModule],
  exports: [ProductCustomersWidgetComponent],
})
export class ProductCustomersWidgetModule {}
