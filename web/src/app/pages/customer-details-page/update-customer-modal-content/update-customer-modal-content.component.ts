import { ChangeDetectionStrategy, Component } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { errorHandler, handleErrors } from '../../../../modules/operators/handleErrors';
import { NotificationType } from '../../../../modules/notification/notification-type';
import { NotificationService } from '../../../../modules/notification/notification.service';
import { Observable } from 'rxjs';
import { FormModalContent } from '../../../../modules/forms/form-modal/form-modal-content';
import { UpdateCustomerModalContentState } from './update-customer-modal-content-state';
import { CustomerService } from '../../../../modules/customer/customer.service';
import { UpdateCustomerRequest } from '../../../../modules/customer/update-customer.model';

@UntilDestroy()
@Component({
  selector: 'bip-update-customer-modal-content',
  templateUrl: './update-customer-modal-content.component.html',
  styleUrls: ['./update-customer-modal-content.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UpdateCustomerModalContentComponent extends FormModalContent<UpdateCustomerModalContentState> {
  constructor(private readonly customerService: CustomerService, private readonly notificationService: NotificationService) {
    super();
  }

  onSubmit(): Observable<void> {
    const controls = this.state.formGroup.bipControls;
    const updateCustomerRequest: UpdateCustomerRequest = {
      name: controls.name.value,
      contactEmail: controls.email.value,
      contactPhone: controls.phone.value,
      contactName: controls.contactName.value,
    };

    return this.customerService.updateCustomer(this.state.customer.name, updateCustomerRequest).pipe(
      untilDestroyed(this),
      handleErrors([
        errorHandler(() => {
          this.notificationService.notify(
            NotificationType.Error,
            'An error has occurred',
            `The customer ${this.state.customer.name} was not updated`,
          );
        }),
      ]),
    );
  }
}
