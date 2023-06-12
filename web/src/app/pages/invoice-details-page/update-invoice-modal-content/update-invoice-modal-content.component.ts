import { ChangeDetectionStrategy, Component } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { errorHandler, handleErrors } from '../../../../modules/operators/handleErrors';
import { NotificationType } from '../../../../modules/notification/notification-type';
import { NotificationService } from '../../../../modules/notification/notification.service';
import { Observable } from 'rxjs';
import { FormModalContent } from '../../../../modules/forms/form-modal/form-modal-content';
import { UpdateInvoiceModalContentState } from './update-invoice-modal-content-state';
import { InvoiceService } from '../../../../modules/invoice/invoice.service';
import { UpdateInvoiceRequest } from '../../../../modules/invoice/update-invoice-request.model';

@UntilDestroy()
@Component({
  selector: 'bip-update-invoice-modal-content',
  templateUrl: './update-invoice-modal-content.component.html',
  styleUrls: ['./update-invoice-modal-content.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UpdateInvoiceModalContentComponent extends FormModalContent<UpdateInvoiceModalContentState> {
  constructor(private readonly invoiceService: InvoiceService, private readonly notificationService: NotificationService) {
    super();
  }

  onSubmit(): Observable<void> {
    const controls = this.state.formGroup.bipControls;
    const updateInvoiceRequest: UpdateInvoiceRequest = {
      number: controls.number.value,
      amount: controls.amount.value,
      receivedDate: controls.receivedDate.value,
      dueDate: controls.dueDate.value,
    };

    return this.invoiceService.updateInvoice(this.state.invoice.number, updateInvoiceRequest).pipe(
      untilDestroyed(this),
      handleErrors([
        errorHandler(() => {
          this.notificationService.notify(
            NotificationType.Error,
            'An error has occurred',
            `The invoice ${this.state.invoice.number} was not updated`,
          );
        }),
      ]),
    );
  }
}
