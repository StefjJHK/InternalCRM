import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { errorHandler, handleErrors } from '../../operators/handleErrors';
import { WidgetState } from '../../widget/widget-state';
import { PurchaseOrderService } from '../purchase-order.service';
import { PurchaseOrder } from '../purchase-order.model';
import { permissionsFrom } from '../../../utils/permissions-utils';

@UntilDestroy()
@Component({
  selector: 'bip-purchase-orders-table-widget',
  templateUrl: './purchase-orders-table-widget.component.html',
  styleUrls: ['./purchase-orders-table-widget.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PurchaseOrdersTableWidgetComponent implements OnInit {
  @Input() invoiceName?: string;

  @Input() productName?: string;

  @Input() customerName?: string;

  widgetState: WidgetState = WidgetState.loaded;

  purchaseOrders: PurchaseOrder[] = [];

  permissionsFrom = permissionsFrom;

  constructor(private readonly purchaseOrderService: PurchaseOrderService, private readonly changeDetectorRef: ChangeDetectorRef) {}

  ngOnInit() {
    this.fetchPurchaseOrders();
  }

  fetchPurchaseOrders() {
    this.widgetState = WidgetState.loading;

    this.purchaseOrderService
      .getPurchaseOrders({ customer: this.customerName, invoice: this.invoiceName, product: this.productName })
      .pipe(
        untilDestroyed(this),
        handleErrors([
          errorHandler(() => {
            this.widgetState = WidgetState.error;

            this.changeDetectorRef.markForCheck();
          }),
        ]),
      )
      .subscribe((purchaseOrders: PurchaseOrder[]) => {
        this.purchaseOrders = purchaseOrders;
        this.widgetState = WidgetState.loaded;

        this.changeDetectorRef.markForCheck();
      });
  }
}
