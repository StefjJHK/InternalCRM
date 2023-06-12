import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TableWidgetComponent } from './table-widget.component';
import { TableWidgetHeaderComponent } from './table-widget-header/table-widget-header.component';
import { WidgetModule } from '../widget/widget.module';

@NgModule({
  declarations: [TableWidgetComponent, TableWidgetHeaderComponent],
  imports: [CommonModule, WidgetModule],
  exports: [TableWidgetComponent, TableWidgetHeaderComponent],
})
export class TableWidgetModule {}
