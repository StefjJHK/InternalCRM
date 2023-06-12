import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { FormModalContent } from '../../../../modules/forms/form-modal/form-modal-content';
import { AddCustomerRequest } from '../../../../modules/customer/add-customer.model';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { errorHandler, handleErrors } from '../../../../modules/operators/handleErrors';
import { NotificationType } from '../../../../modules/notification/notification-type';
import { Observable } from 'rxjs';
import { CustomerService } from '../../../../modules/customer/customer.service';
import { NotificationService } from '../../../../modules/notification/notification.service';

@UntilDestroy()
@Component({
  selector: 'bip-create-customer-modal-content',
  templateUrl: './create-customer-modal-content.component.html',
  styleUrls: ['./create-customer-modal-content.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CreateCustomerModalContentComponent extends FormModalContent {
  constructor(private readonly customerService: CustomerService, private readonly notificationService: NotificationService) {
    super();
  }

  onSubmit(): Observable<void> {
    const addCustomerRequest: AddCustomerRequest = {
      name: this.state.formGroup.bipControls.name.value,
      contactName: this.state.formGroup.bipControls.contactName.value,
      contactEmail: this.state.formGroup.bipControls.email.value,
      contactPhone: this.state.formGroup.bipControls.phone.value,
    };

    return this.customerService.addCustomer(addCustomerRequest).pipe(
      untilDestroyed(this),
      handleErrors([
        errorHandler(() => {
          this.notificationService.notify(NotificationType.Error, 'An error has occurred', 'The customer was not created');
        }),
      ]),
    );
  }
}
