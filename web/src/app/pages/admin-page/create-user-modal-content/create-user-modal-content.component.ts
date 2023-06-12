import { ChangeDetectionStrategy, Component } from '@angular/core';
import { FormModalContent } from '../../../../modules/forms/form-modal/form-modal-content';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { errorHandler, handleErrors } from '../../../../modules/operators/handleErrors';
import { NotificationType } from '../../../../modules/notification/notification-type';
import { Observable } from 'rxjs';
import { NotificationService } from '../../../../modules/notification/notification.service';
import { UserService } from '../../../../modules/user/user.service';
import { AddUserRequest } from '../../../../modules/user/add-user-request.model';
import { RadioFieldOption } from '../../../../modules/forms/fields/radio-field/radio-field-option';

@UntilDestroy()
@Component({
  selector: 'bip-create-user-modal-content',
  templateUrl: './create-user-modal-content.component.html',
  styleUrls: ['./create-user-modal-content.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CreateUserModalContentComponent extends FormModalContent {
  permissionRadioFieldOptions: RadioFieldOption[] = [
    {
      label: 'Yes',
      value: true,
    },
    {
      label: 'No',
      value: false,
    },
  ];

  constructor(private readonly userService: UserService, private readonly notificationService: NotificationService) {
    super();
  }

  onSubmit(): Observable<void> {
    const controls = this.state.formGroup.bipControls;
    const addUserRequest: AddUserRequest = {
      username: controls.username.value,
      password: controls.password.value,
      permissions: {
        analytics: {
          canWrite: controls.analyticsCanWrite.value,
          canRead: controls.analyticsCanRead.value,
        },
        product: {
          canWrite: controls.productCanWrite.value,
          canRead: controls.productCanRead.value,
        },
        customer: {
          canWrite: controls.customerCanWrite.value,
          canRead: controls.customerCanRead.value,
        },
        lead: {
          canWrite: controls.leadCanWrite.value,
          canRead: controls.leadCanRead.value,
        },
        purchaseOrder: {
          canWrite: controls.purchaseOrderCanWrite.value,
          canRead: controls.purchaseOrderCanRead.value,
        },
        invoice: {
          canWrite: controls.invoiceCanWrite.value,
          canRead: controls.invoiceCanRead.value,
        },
        payment: {
          canWrite: controls.paymentCanWrite.value,
          canRead: controls.paymentCanRead.value,
        },
        subscription: {
          canWrite: controls.subscriptionCanWrite.value,
          canRead: controls.subscriptionCanRead.value,
        },
      },
    };

    return this.userService.addUser(addUserRequest).pipe(
      untilDestroyed(this),
      handleErrors([
        errorHandler(() => {
          this.notificationService.notify(NotificationType.Error, 'An error has occurred', 'The user was not created');
        }),
      ]),
    );
  }
}
