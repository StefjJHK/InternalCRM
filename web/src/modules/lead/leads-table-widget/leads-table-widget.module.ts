import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LeadsTableWidgetComponent } from './leads-table-widget.component';
import { TableWidgetModule } from '../../table-widget/table-widget.module';
import { NzTabsModule } from 'ng-zorro-antd/tabs';
import { NzTableModule } from 'ng-zorro-antd/table';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [LeadsTableWidgetComponent],
  imports: [CommonModule, TableWidgetModule, NzTabsModule, NzTableModule, RouterModule],
  exports: [LeadsTableWidgetComponent],
})
export class LeadsTableWidgetModule {}
