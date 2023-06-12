import { ChangeDetectionStrategy, Component, OnInit, ViewChild } from '@angular/core';
import { FormModalService } from '../../../modules/forms/form-modal/form-modal.service';
import { BipFormGroup } from '../../../modules/forms/form-controls/bip-form-group';
import { BipFormControl } from '../../../modules/forms/form-controls/bip-form-control';
import { BipValidators } from '../../../modules/forms/bip-validators';
import { UntilDestroy } from '@ngneat/until-destroy';
import { CreatePaymentModalContent } from './create-payment-modal-content/create-payment-modal-content';
import { NotificationService } from '../../../modules/notification/notification.service';
import { NotificationType } from '../../../modules/notification/notification-type';
import { Button } from '../../../modules/common-components/button/button.model';
import { PaymentsTableWidgetComponent } from '../../../modules/payments/payments-table-widget/payments-table-widget.component';
import { PageState } from '../../../modules/page/page-state';
import { PermissionsService } from '../../../modules/permissions/permissions.service';

@UntilDestroy()
@Component({
  selector: 'bip-leads-page',
  templateUrl: './payments-page.component.html',
  styleUrls: ['./payments-page.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PaymentsPageComponent implements OnInit {
  @ViewChild(PaymentsTableWidgetComponent) paymentsTableWidget!: PaymentsTableWidgetComponent;

  pageState: PageState = PageState.loaded;

  headerButtons: Button[] = [];

  constructor(
    private readonly formModalService: FormModalService,
    private readonly notificationService: NotificationService,
    private readonly permissionsService: PermissionsService,
  ) {}

  ngOnInit() {
    if (this.permissionsService.containsPermissions({ payment: { canWrite: true } })) {
      this.headerButtons = [
        ...this.headerButtons,
        {
          text: 'Add New',
          type: 'primary',
          icon: 'plus',

          onClick: () => this.onCreatePaymentClick(),
        },
      ];
    }
  }

  private onCreatePaymentClick() {
    const formGroup: BipFormGroup = new BipFormGroup({
      paymentNumber: new BipFormControl<string>('', 'Payment number', { validators: [BipValidators.required()] }),
      invoiceNumber: new BipFormControl<string>('', 'Invoice', { validators: [BipValidators.required()] }),
      amount: new BipFormControl<number | null>(null, 'Amount', { validators: [BipValidators.required()] }),
      receivedDate: new BipFormControl<Date | null>(null, 'Received date', { validators: [BipValidators.required()] }),
    });

    const onSubmit = () => {
      this.notificationService.notify(
        NotificationType.Success,
        'Success',
        `The payment ${formGroup.bipControls.paymentNumber.value} was created`,
      );
      this.paymentsTableWidget.fetchPayments();
    };

    this.formModalService.show('New payment', CreatePaymentModalContent, formGroup, onSubmit);
  }
}
