import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { TotalSalesService } from '../total-sales.service';
import { WidgetState } from '../../../widget/widget-state';
import { TotalSales } from '../total-sales.model';
import { TotalSalesRequest } from '../total-sales-request.model';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { errorHandler, handleErrors } from '../../../operators/handleErrors';

@UntilDestroy()
@Component({
  selector: 'bip-total-sales-chart-widget',
  templateUrl: './total-sales-chart-widget.component.html',
  styleUrls: ['./total-sales-chart-widget.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TotalSalesChartWidgetComponent implements OnInit {
  totalSales?: TotalSales;

  widgetState: WidgetState = WidgetState.loading;

  constructor(private readonly totalSalesService: TotalSalesService, private readonly changeDetectorRef: ChangeDetectorRef) {}

  ngOnInit() {
    this.fetchTotalSales();
  }

  fetchTotalSales() {
    const totalSalesRequest: TotalSalesRequest = {
      startDate: TotalSalesChartWidgetComponent.calculateFromDate().toISOString(),
      endDate: TotalSalesChartWidgetComponent.calculateToDate().toISOString(),
    };

    this.widgetState = WidgetState.loading;

    this.totalSalesService
      .getTotalSales(totalSalesRequest)
      .pipe(
        untilDestroyed(this),
        handleErrors([
          errorHandler(() => {
            this.widgetState = WidgetState.error;

            this.changeDetectorRef.markForCheck();
          }),
        ]),
      )
      .subscribe((totalSales) => {
        this.totalSales = totalSales;
        this.widgetState = WidgetState.loaded;

        this.changeDetectorRef.markForCheck();
      });
  }

  private static calculateFromDate(): Date {
    const date = new Date();
    const monthBehind = 12;

    date.setMonth(date.getMonth() - monthBehind);

    return date;
  }

  private static calculateToDate(): Date {
    return new Date();
  }
}
