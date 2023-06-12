import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InvoicesTableWidgetComponent } from './invoices-table-widget.component';
import { TableWidgetModule } from '../../table-widget/table-widget.module';
import { NzTabsModule } from 'ng-zorro-antd/tabs';
import { NzTableModule } from 'ng-zorro-antd/table';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [InvoicesTableWidgetComponent],
  imports: [CommonModule, TableWidgetModule, NzTabsModule, NzTableModule, RouterModule],
  exports: [InvoicesTableWidgetComponent],
})
export class InvoicesTableWidgetModule {}
