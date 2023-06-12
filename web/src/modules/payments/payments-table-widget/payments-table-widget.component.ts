import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { errorHandler, handleErrors } from '../../operators/handleErrors';
import { WidgetState } from '../../widget/widget-state';
import { PaymentService } from '../payment.service';
import { Payment } from '../payment.model';
import { Button } from '../../common-components/button/button.model';
import { NotificationType } from '../../notification/notification-type';
import { NotificationService } from '../../notification/notification.service';
import { DeleteConfirmationService } from '../../forms/delete-confirmation/delete-confirmation.service';
import { BipFormGroup } from '../../forms/form-controls/bip-form-group';
import { BipFormControl } from '../../forms/form-controls/bip-form-control';
import { BipValidators } from '../../forms/bip-validators';
import { FormState } from '../../forms/form-modal/form-modal-content/form-state';
import { FormModalService } from '../../forms/form-modal/form-modal.service';
import { UpdatePaymentModalContentComponent } from './update-payment-modal-content/update-payment-modal-content.component';
import { UpdatePaymentModalContentState } from './update-payment-modal-content/update-payment-modal-content-state';

@UntilDestroy()
@Component({
  selector: 'bip-payments-table-widget',
  templateUrl: './payments-table-widget.component.html',
  styleUrls: ['./payments-table-widget.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PaymentsTableWidgetComponent implements OnInit {
  @Input() purchaseOrderName?: string;

  @Input() invoiceName?: string;

  @Input() subscriptionName?: string;

  @Input() productName?: string;

  @Input() customerName?: string;

  widgetState: WidgetState = WidgetState.loaded;

  payments: Payment[] = [];

  constructor(
    private readonly paymentService: PaymentService,
    private readonly notificationService: NotificationService,
    private readonly deleteConfirmationService: DeleteConfirmationService,
    private readonly formModalService: FormModalService,
    private readonly changeDetectorRef: ChangeDetectorRef,
  ) {}

  ngOnInit() {
    this.fetchPayments();
  }

  fetchPayments() {
    this.widgetState = WidgetState.loading;

    this.paymentService
      .getPayments({
        customer: this.customerName,
        purchaseOrder: this.purchaseOrderName,
        invoice: this.invoiceName,
        product: this.productName,
        subscription: this.subscriptionName,
      })
      .pipe(
        untilDestroyed(this),
        handleErrors([
          errorHandler(() => {
            this.widgetState = WidgetState.error;

            this.changeDetectorRef.markForCheck();
          }),
        ]),
      )
      .subscribe((payments: Payment[]) => {
        this.payments = payments;
        this.widgetState = WidgetState.loaded;

        this.changeDetectorRef.markForCheck();
      });
  }

  private updatePayment(payment: Payment) {
    const formGroup: BipFormGroup = new BipFormGroup({
      paymentNumber: new BipFormControl<string>(payment.number, 'Payment', { validators: [BipValidators.required()] }),
      amount: new BipFormControl<number>(payment.amount, 'Amount', { validators: [BipValidators.required()] }),
      receivedDate: new BipFormControl<Date>(payment.receivedDate, 'Received date', { validators: [BipValidators.required()] }),
    });

    const onSubmit = () => {
      this.notificationService.notify(NotificationType.Success, 'Success', `The payment ${payment.number} was updated`);
      this.fetchPayments();
    };
    const submitEnabled = (formState: FormState) => {
      if (formState == FormState.Loading || formState == FormState.Submitting) {
        return false;
      }

      return (
        payment.number != formGroup.bipControls.paymentNumber.value ||
        payment.amount != formGroup.bipControls.amount.value ||
        payment.receivedDate != formGroup.bipControls.receivedDate.value
      );
    };

    this.formModalService.show<UpdatePaymentModalContentComponent, UpdatePaymentModalContentState>(
      'Update payment',
      UpdatePaymentModalContentComponent,
      formGroup,
      onSubmit,
      submitEnabled,
      { payment },
    );
  }

  private deletePayment(payment: Payment) {
    const onDelete = (): void => {
      this.notificationService.notify(NotificationType.Success, 'Success', `Payment ${payment.number} was deleted`);

      this.fetchPayments();
    };

    this.deleteConfirmationService.confirmDelete(
      'Are you sure to delete payment?',
      payment.number,
      this.paymentService.deletePayment(payment.invoiceNumber, payment.number),
      onDelete,
    );
  }

  createUpdateAction(payment: Payment): Button {
    return {
      text: 'edit',
      type: 'text',

      onClick: () => this.updatePayment(payment),
    };
  }

  createDeleteAction(payment: Payment): Button {
    return {
      text: 'delete',
      type: 'text',

      onClick: () => this.deletePayment(payment),
    };
  }
}
