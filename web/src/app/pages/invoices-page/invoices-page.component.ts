import { ChangeDetectionStrategy, Component, OnInit, ViewChild } from '@angular/core';
import { FormModalService } from '../../../modules/forms/form-modal/form-modal.service';
import { BipFormGroup } from '../../../modules/forms/form-controls/bip-form-group';
import { BipFormControl } from '../../../modules/forms/form-controls/bip-form-control';
import { BipValidators } from '../../../modules/forms/bip-validators';
import { CreateInvoiceModalContent } from './create-invoice-modal-content/create-invoice-modal-content';
import { NotificationService } from '../../../modules/notification/notification.service';
import { NotificationType } from '../../../modules/notification/notification-type';
import { Button } from '../../../modules/common-components/button/button.model';
import { InvoicesTableWidgetComponent } from '../../../modules/invoice/invoices-table-widget/invoices-table-widget.component';
import { PageState } from '../../../modules/page/page-state';
import { PermissionsService } from '../../../modules/permissions/permissions.service';

@Component({
  selector: 'bip-invoices-page',
  templateUrl: './invoices-page.component.html',
  styleUrls: ['./invoices-page.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class InvoicesPageComponent implements OnInit {
  @ViewChild(InvoicesTableWidgetComponent) invoicesTableWidget!: InvoicesTableWidgetComponent;

  onCreateInvoiceClick = () => {
    const formGroup: BipFormGroup = new BipFormGroup({
      customerName: new BipFormControl<string>('', 'Customer', { validators: [BipValidators.required()] }),
      productName: new BipFormControl<string>('', 'Product', { validators: [BipValidators.required()] }),
      purchaseOrderNumber: new BipFormControl<string>('', 'Purchase order'),
      number: new BipFormControl<string>('', 'Number', { validators: [BipValidators.required()] }),
      amount: new BipFormControl<number | null>(null, 'Amount', { validators: [BipValidators.required()] }),
      receivedDate: new BipFormControl<Date | null>(null, 'Received date', { validators: [BipValidators.required()] }),
      dueDate: new BipFormControl<Date | null>(null, 'Due date', { validators: [BipValidators.required()] }),
    });

    const onSubmit = () => {
      this.notificationService.notify(NotificationType.Success, 'Success', `The invoice ${formGroup.bipControls.number.value} was created`);
      this.invoicesTableWidget.fetchInvoices();
    };

    this.formModalService.show('Add New', CreateInvoiceModalContent, formGroup, onSubmit);
  };

  headerButtons: Button[] = [];

  pageState: PageState = PageState.loaded;

  constructor(
    private readonly formModalService: FormModalService,
    private readonly notificationService: NotificationService,
    private readonly permissionsService: PermissionsService,
  ) {}

  ngOnInit() {
    if (this.permissionsService.containsPermissions({ invoice: { canWrite: true } })) {
      this.headerButtons = [
        ...this.headerButtons,
        {
          text: 'Add New',
          type: 'primary',
          icon: 'plus',

          onClick: this.onCreateInvoiceClick,
        },
      ];
    }
  }
}
