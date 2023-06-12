import { Component } from '@angular/core';
import { FormModalContent } from '../../form-modal/form-modal-content';
import { Observable } from 'rxjs';
import { DeleteConfirmationContentState } from './delete-confirmation-content-state';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { errorHandler, handleErrors } from '../../../operators/handleErrors';
import { NotificationType } from '../../../notification/notification-type';
import { NotificationService } from '../../../notification/notification.service';

@UntilDestroy()
@Component({
  selector: 'bip-delete-confirmation-content',
  templateUrl: './delete-confirmation-content.component.html',
  styleUrls: ['./delete-confirmation-content.component.less'],
})
export class DeleteConfirmationContentComponent extends FormModalContent<DeleteConfirmationContentState> {
  constructor(private readonly notificationService: NotificationService) {
    super();
  }

  override onSubmit(): Observable<void> {
    return this.state.delete$.pipe(
      untilDestroyed(this),
      handleErrors([
        errorHandler(() => {
          this.notificationService.notify(NotificationType.Error, 'An error has occurred', 'Deletion failed');
        }),
      ]),
    );
  }
}
