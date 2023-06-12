import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LineChartComponent } from './line-chart.component';
import { ChartModule } from '../chart.module';

@NgModule({
  declarations: [LineChartComponent],
  imports: [CommonModule, ChartModule],
  exports: [LineChartComponent],
})
export class LineChartModule {}
