import { ChangeDetectionStrategy, Component, OnInit, ViewChild } from '@angular/core';
import { FormModalService } from '../../../modules/forms/form-modal/form-modal.service';
import { CreatePurchaseOrderModalContent } from './create-purchase-order-modal-content/create-purchase-order-modal-content';
import { BipFormGroup } from '../../../modules/forms/form-controls/bip-form-group';
import { BipFormControl } from '../../../modules/forms/form-controls/bip-form-control';
import { BipValidators } from '../../../modules/forms/bip-validators';
import { NotificationService } from '../../../modules/notification/notification.service';
import { NotificationType } from '../../../modules/notification/notification-type';
import { Button } from '../../../modules/common-components/button/button.model';
import { PageState } from '../../../modules/page/page-state';
import { PurchaseOrdersTableWidgetComponent } from '../../../modules/purchase-order/purchase-orders-table-widget/purchase-orders-table-widget.component';
import { PermissionsService } from '../../../modules/permissions/permissions.service';

@Component({
  selector: 'bip-leads-page',
  templateUrl: './purchase-orders-page.component.html',
  styleUrls: ['./purchase-orders-page.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PurchaseOrdersPageComponent implements OnInit {
  @ViewChild(PurchaseOrdersTableWidgetComponent) purchaseOrdersTableWidget!: PurchaseOrdersTableWidgetComponent;

  headerActions: Button[] = [];

  pageState: PageState = PageState.loaded;

  constructor(
    private readonly formModalService: FormModalService,
    private readonly notificationService: NotificationService,
    private readonly permissionsService: PermissionsService,
  ) {}

  ngOnInit() {
    if (this.permissionsService.containsPermissions({ purchaseOrder: { canWrite: true } })) {
      this.headerActions = [
        ...this.headerActions,
        {
          text: 'Add New',
          type: 'primary',
          icon: 'plus',

          onClick: () => this.onCreatePurchaseOrder(),
        },
      ];
    }
  }

  private onCreatePurchaseOrder() {
    const formGroup: BipFormGroup = new BipFormGroup({
      customerName: new BipFormControl<string>('', 'Customer', { validators: [BipValidators.required()] }),
      productName: new BipFormControl<string>('', 'Product', { validators: [BipValidators.required()] }),
      number: new BipFormControl<string>('', 'Number', { validators: [BipValidators.required()] }),
      amount: new BipFormControl<number | null>(null, 'Amount', { validators: [BipValidators.required()] }),
      receivedDate: new BipFormControl<Date | null>(null, 'Received date', { validators: [BipValidators.required()] }),
      dueDate: new BipFormControl<Date | null>(null, 'Due date', { validators: [BipValidators.required()] }),
    });

    const onSubmit = () => {
      this.notificationService.notify(
        NotificationType.Success,
        'Success',
        `The purchase order ${formGroup.bipControls.number.value} was created`,
      );
      this.purchaseOrdersTableWidget.fetchPurchaseOrders();
    };

    this.formModalService.show('New purchase order', CreatePurchaseOrderModalContent, formGroup, onSubmit);
  }
}
