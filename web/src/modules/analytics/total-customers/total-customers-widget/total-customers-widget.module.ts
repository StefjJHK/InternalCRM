import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TotalCustomersWidgetComponent } from './total-customers-widget.component';
import { ChartWidgetModule } from '../../../chart-widget/chart-widget.module';
import { NzStatisticModule } from 'ng-zorro-antd/statistic';
import { BarChartModule } from '../../../chart/bar-chart/bar-chart.module';

@NgModule({
  declarations: [TotalCustomersWidgetComponent],
  imports: [CommonModule, ChartWidgetModule, BarChartModule, NzStatisticModule],
  exports: [TotalCustomersWidgetComponent],
})
export class TotalCustomersWidgetModule {}
