import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardPageComponent } from './dashboard-page.component';
import { PageModule } from '../../../modules/page/page.module';
import { TotalSalesChartWidgetModule } from '../../../modules/analytics/total-sales/total-sales-chart-widget/total-sales-chart-widget.module';
import { TotalCustomersWidgetModule } from '../../../modules/analytics/total-customers/total-customers-widget/total-customers-widget.module';
import { TotalRevenueWidgetModule } from '../../../modules/analytics/total-revenue/total-revenue-widget/total-revenue-widget.module';
import { NzGridModule } from 'ng-zorro-antd/grid';

@NgModule({
  declarations: [DashboardPageComponent],
  imports: [CommonModule, TotalSalesChartWidgetModule, PageModule, TotalCustomersWidgetModule, TotalRevenueWidgetModule, NzGridModule],
  exports: [DashboardPageComponent],
})
export class DashboardPageModule {}
