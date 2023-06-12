import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { errorHandler, handleErrors } from '../../operators/handleErrors';
import { WidgetState } from '../../widget/widget-state';
import { Customer } from '../customer.model';
import { CustomerService } from '../customer.service';
import { AppRoutes } from '../../../app/routing/app-routes';

@UntilDestroy()
@Component({
  selector: 'bip-customers-table-widget',
  templateUrl: './customers-table-widget.component.html',
  styleUrls: ['./customers-table-widget.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomersTableWidgetComponent implements OnInit {
  @Input() productName?: string;

  widgetState: WidgetState = WidgetState.loaded;

  customers: Customer[] = [];

  Routes = AppRoutes;

  constructor(private readonly customerService: CustomerService, private readonly changeDetectorRef: ChangeDetectorRef) {}

  ngOnInit() {
    this.fetchCustomers();
  }

  fetchCustomers() {
    this.widgetState = WidgetState.loading;

    this.customerService
      .getCustomers({ product: this.productName })
      .pipe(
        untilDestroyed(this),
        handleErrors([
          errorHandler(() => {
            this.widgetState = WidgetState.error;

            this.changeDetectorRef.markForCheck();
          }),
        ]),
      )
      .subscribe((customers: Customer[]) => {
        this.customers = customers;
        this.widgetState = WidgetState.loaded;

        this.changeDetectorRef.markForCheck();
      });
  }
}
