import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UsersTableWidgetComponent } from './users-table-widget.component';
import { TableWidgetModule } from '../../table-widget/table-widget.module';
import { NzTableModule } from 'ng-zorro-antd/table';
import { ButtonModule } from '../../common-components/button/button.module';

@NgModule({
  declarations: [UsersTableWidgetComponent],
  imports: [CommonModule, TableWidgetModule, NzTableModule, ButtonModule],
  exports: [UsersTableWidgetComponent],
})
export class UsersTableWidgetModule {}
