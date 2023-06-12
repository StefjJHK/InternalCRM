import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TotalSalesChartWidgetComponent } from './total-sales-chart-widget.component';
import { ChartWidgetModule } from '../../../chart-widget/chart-widget.module';
import { NzTypographyModule } from 'ng-zorro-antd/typography';
import { CardModule } from '../../../common-components/card/card.module';
import { NzStatisticModule } from 'ng-zorro-antd/statistic';
import { AreaChartModule } from '../../../chart/area-chart/area-chart.module';

@NgModule({
  declarations: [TotalSalesChartWidgetComponent],
  imports: [CommonModule, ChartWidgetModule, NzTypographyModule, NzStatisticModule, CardModule, AreaChartModule],
  exports: [TotalSalesChartWidgetComponent],
})
export class TotalSalesChartWidgetModule {}
