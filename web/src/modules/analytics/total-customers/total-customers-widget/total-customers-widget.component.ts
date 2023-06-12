import { ChangeDetectionStrategy, ChangeDetectorRef, Component } from '@angular/core';
import { WidgetState } from '../../../widget/widget-state';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { TotalCustomers } from '../total-customers.model';
import { TotalCustomersService } from '../total-customers.service';
import { TotalCustomersRequest } from '../total-customers-request.model';
import { errorHandler, handleErrors } from '../../../operators/handleErrors';

@UntilDestroy()
@Component({
  selector: 'bip-total-customers-widget',
  templateUrl: './total-customers-widget.component.html',
  styleUrls: ['./total-customers-widget.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TotalCustomersWidgetComponent {
  totalCustomers?: TotalCustomers;

  widgetState: WidgetState = WidgetState.loaded;

  constructor(private readonly totalCustomersService: TotalCustomersService, private readonly changeDetectorRef: ChangeDetectorRef) {}

  ngOnInit() {
    this.fetchTotalCustomers();
  }

  fetchTotalCustomers() {
    const totalCustomerRequest: TotalCustomersRequest = {
      startDate: TotalCustomersWidgetComponent.calculateFromDate().toISOString(),
      endDate: TotalCustomersWidgetComponent.calculateToDate().toISOString(),
    };

    this.widgetState = WidgetState.loading;

    this.totalCustomersService
      .getTotalCustomers(totalCustomerRequest)
      .pipe(
        untilDestroyed(this),
        handleErrors([
          errorHandler(() => {
            this.widgetState = WidgetState.error;

            this.changeDetectorRef.markForCheck();
          }),
        ]),
      )
      .subscribe((totalCustomers) => {
        this.totalCustomers = totalCustomers;
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
