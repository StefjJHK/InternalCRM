import { UpdatePaymentModalContentState } from './update-payment-modal-content-state';
import { FormModalContent } from '../../../forms/form-modal/form-modal-content';
import { NotificationService } from '../../../notification/notification.service';
import { Observable } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { NotificationType } from '../../../notification/notification-type';
import { errorHandler, handleErrors } from '../../../operators/handleErrors';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { PaymentService } from '../../payment.service';
import { UpdatePaymentRequest } from '../../update-payment-request.model';

@UntilDestroy()
@Component({
  selector: 'bip-update-invoice-modal-content',
  templateUrl: './update-payment-modal-content.component.html',
  styleUrls: ['./update-payment-modal-content.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UpdatePaymentModalContentComponent extends FormModalContent<UpdatePaymentModalContentState> {
  constructor(private readonly paymentService: PaymentService, private readonly notificationService: NotificationService) {
    super();
  }

  onSubmit(): Observable<void> {
    const controls = this.state.formGroup.bipControls;
    const updatePaymentRequest: UpdatePaymentRequest = {
      number: controls.paymentNumber.value,
      amount: controls.amount.value,
      receivedDate: controls.receivedDate.value,
    };

    return this.paymentService.updatePayment(this.state.payment.invoiceNumber, this.state.payment.number, updatePaymentRequest).pipe(
      untilDestroyed(this),
      handleErrors([
        errorHandler(() => {
          this.notificationService.notify(
            NotificationType.Error,
            'An error has occurred',
            `The payment ${this.state.payment.number} was not updated`,
          );
        }),
      ]),
    );
  }
}
