import { ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { WidgetState } from '../../widget/widget-state';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { errorHandler, handleErrors } from '../../operators/handleErrors';
import { Subscription } from '../subscritpion.model';
import { SubscriptionService } from '../subscription.service';

@UntilDestroy()
@Component({
  selector: 'bip-subscriptions-table-widget',
  templateUrl: './subscriptions-table-widget.component.html',
  styleUrls: ['./subscriptions-table-widget.component.less'],
})
export class SubscriptionsTableWidgetComponent implements OnInit {
  widgetState: WidgetState = WidgetState.loaded;

  @Input() customerName?: string;

  @Input() invoiceName?: string;

  @Input() productName?: string;

  productSubscriptions?: Subscription[];

  constructor(private readonly subscriptionsService: SubscriptionService, private readonly changeDetectorRef: ChangeDetectorRef) {}

  ngOnInit() {
    this.fetchSubscriptions();
  }

  fetchSubscriptions() {
    this.widgetState = WidgetState.loading;

    this.subscriptionsService
      .getSubscriptions({ product: this.productName, invoice: this.invoiceName, customer: this.customerName })
      .pipe(
        untilDestroyed(this),
        handleErrors([
          errorHandler(() => {
            this.widgetState = WidgetState.error;

            this.changeDetectorRef.markForCheck();
          }),
        ]),
      )
      .subscribe((productSubscriptions) => {
        this.productSubscriptions = productSubscriptions;
        this.widgetState = WidgetState.loaded;

        this.changeDetectorRef.markForCheck();
      });
  }
}
