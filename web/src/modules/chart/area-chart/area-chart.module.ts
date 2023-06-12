import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AreaChartComponent } from './area-chart.component';
import { ChartModule } from '../chart.module';

@NgModule({
  declarations: [AreaChartComponent],
  imports: [CommonModule, ChartModule],
  exports: [AreaChartComponent],
})
export class AreaChartModule {}
