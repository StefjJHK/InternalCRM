import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TotalRevenueWidgetComponent } from './total-revenue-widget.component';
import { LineChartModule } from '../../../chart/line-chart/line-chart.module';
import { ChartWidgetModule } from '../../../chart-widget/chart-widget.module';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { DateRangePickerFieldModule } from '../../../forms/fields/date-range-picker-field/date-range-picker-field.module';
import { SelectFieldModule } from '../../../forms/fields/select-field/select-field.module';
import { DatePickerFieldModule } from '../../../forms/fields/date-picker-field/date-picker-field.module';

@NgModule({
  declarations: [TotalRevenueWidgetComponent],
  imports: [
    CommonModule,
    LineChartModule,
    ChartWidgetModule,
    SelectFieldModule,
    DateRangePickerFieldModule,
    NzGridModule,
    NzSpaceModule,
    DatePickerFieldModule,
  ],
  exports: [TotalRevenueWidgetComponent],
})
export class TotalRevenueWidgetModule {}
