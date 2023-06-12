import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChartWidgetComponent } from './chart-widget.component';
import { WidgetModule } from '../widget/widget.module';
import { ChartWidgetHeaderComponent } from './chart-widget-header/chart-widget-header.component';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { ChartWidgetFilterComponent } from './chart-widget-filter/chart-widget-filter.component';
import { NzGridModule } from 'ng-zorro-antd/grid';

@NgModule({
  declarations: [ChartWidgetComponent, ChartWidgetHeaderComponent, ChartWidgetFilterComponent],
  imports: [CommonModule, WidgetModule, NzEmptyModule, NzGridModule],
  exports: [ChartWidgetComponent, ChartWidgetHeaderComponent, ChartWidgetFilterComponent],
})
export class ChartWidgetModule {}
