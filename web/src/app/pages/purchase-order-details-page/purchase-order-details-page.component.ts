import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { switchMap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { NotificationService } from '../../../modules/notification/notification.service';
import { errorHandler, handleErrors } from '../../../modules/operators/handleErrors';
import { NotificationType } from '../../../modules/notification/notification-type';
import { PageState } from '../../../modules/page/page-state';
import { LeadService } from '../../../modules/lead/lead.service';
import { Lead } from '../../../modules/lead/lead.model';
import { PurchaseOrder } from '../../../modules/purchase-order/purchase-order.model';
import { PurchaseOrderService } from '../../../modules/purchase-order/purchase-order.service';

@UntilDestroy()
@Component({
  selector: 'bip-purchase-order-details-page',
  templateUrl: './purchase-order-details-page.component.html',
  styleUrls: ['./purchase-order-details-page.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PurchaseOrderDetailsPageComponent implements OnInit {
  pageState: PageState = PageState.loaded;

  purchaseOrder?: PurchaseOrder;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly purchaseOrderService: PurchaseOrderService,
    private readonly notificationService: NotificationService,
    private readonly changeDetectorRef: ChangeDetectorRef,
  ) {}

  ngOnInit() {
    this.fetchPurchaseOrder();
  }

  fetchPurchaseOrder() {
    this.pageState = PageState.loading;

    this.route.params
      .pipe(
        untilDestroyed(this),
        switchMap((params) => this.purchaseOrderService.getPurchaseOrder(params['purchaseOrderName'])),
        handleErrors([
          errorHandler(() => {
            this.notificationService.notify(NotificationType.Error, 'Error', 'Unknown error has occurred while loading purchase order');
            this.pageState = PageState.error;
          }),
        ]),
      )
      .subscribe((purchaseOrder) => {
        this.purchaseOrder = purchaseOrder;
        this.pageState = PageState.loaded;

        this.changeDetectorRef.markForCheck();
      });
  }
}
