import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { InvoiceService } from '../../../../modules/invoice/invoice.service';
import { AddPaymentRequest } from '../../../../modules/payments/add-payment-request.modal';
import { errorHandler, handleErrors } from '../../../../modules/operators/handleErrors';
import { NotificationType } from '../../../../modules/notification/notification-type';
import { FormModalContent } from '../../../../modules/forms/form-modal/form-modal-content';
import { NotificationService } from '../../../../modules/notification/notification.service';
import { PaymentService } from '../../../../modules/payments/payment.service';
import { FormState } from '../../../../modules/forms/form-modal/form-modal-content/form-state';
import { Invoice } from '../../../../modules/invoice/invoice.model';
import { SelectFieldOption } from '../../../../modules/forms/fields/select-field/select-field-option';

@UntilDestroy()
@Component({
  selector: 'bip-create-subscription-modal-content',
  templateUrl: './create-payment-modal-content.html',
  styleUrls: ['./create-payment-modal-content.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CreatePaymentModalContent extends FormModalContent implements OnInit {
  invoiceSelectOptions: SelectFieldOption[] = [];

  constructor(
    private readonly invoiceService: InvoiceService,
    private readonly notificationService: NotificationService,
    private readonly paymentService: PaymentService,
    private readonly changeDetectionRef: ChangeDetectorRef,
  ) {
    super();
  }

  ngOnInit() {
    this.state.formState$.next(FormState.Loading);

    this.invoiceService
      .getInvoices({})
      .pipe(untilDestroyed(this), handleErrors([errorHandler(() => this.state.formState$.next(FormState.Error))]))
      .subscribe((invoices) => {
        this.invoiceSelectOptions = invoices.map((invoice: Invoice) => ({
          label: invoice.number,
          value: invoice.number,
        }));

        this.state.formState$.next(FormState.Filling);

        this.changeDetectionRef.markForCheck();
      });
  }

  onSubmit() {
    const invoiceNumber = this.state.formGroup.bipControls.invoiceNumber.value;
    const addPaymentRequest: AddPaymentRequest = {
      number: this.state.formGroup.bipControls.paymentNumber.value,
      amount: this.state.formGroup.bipControls.amount.value,
      receivedDate: this.state.formGroup.bipControls.receivedDate.value.toISOString(),
    };

    return this.paymentService.addPayment(invoiceNumber, addPaymentRequest).pipe(
      untilDestroyed(this),
      handleErrors([
        errorHandler(() => {
          this.notificationService.notify(NotificationType.Error, 'An error has occurred', 'The payment was not created');
        }),
      ]),
    );
  }
}
