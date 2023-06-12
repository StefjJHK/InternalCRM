import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input } from '@angular/core';
import { ProductCustomersService } from './product-customers.service';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { ProductCustomer } from './product-customer.model';
import { errorHandler, handleErrors } from '../../operators/handleErrors';
import { WidgetState } from '../../widget/widget-state';

@UntilDestroy()
@Component({
  selector: 'bip-product-customers-widget[productName]',
  templateUrl: './product-customers-widget.component.html',
  styleUrls: ['./product-customers-widget.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProductCustomersWidgetComponent {
  widgetState: WidgetState = WidgetState.loaded;

  @Input() productName!: string;

  productCustomers?: ProductCustomer[];

  constructor(private readonly productCustomersService: ProductCustomersService, private readonly changeDetectorRef: ChangeDetectorRef) {}

  ngOnInit() {
    this.fetchProductCustomers();
  }

  fetchProductCustomers() {
    this.widgetState = WidgetState.loading;

    this.productCustomersService
      .getProductCustomers(this.productName)
      .pipe(
        untilDestroyed(this),
        handleErrors([
          errorHandler(() => {
            this.widgetState = WidgetState.error;

            this.changeDetectorRef.markForCheck();
          }),
        ]),
      )
      .subscribe((productCustomers) => {
        this.productCustomers = productCustomers;
        this.widgetState = WidgetState.loaded;

        this.changeDetectorRef.markForCheck();
      });
  }
}
