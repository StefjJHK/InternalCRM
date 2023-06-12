import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BarChartComponent } from './bar-chart.component';
import { ChartModule } from '../chart.module';

@NgModule({
  declarations: [BarChartComponent],
  imports: [CommonModule, ChartModule],
  exports: [BarChartComponent],
})
export class BarChartModule {}
