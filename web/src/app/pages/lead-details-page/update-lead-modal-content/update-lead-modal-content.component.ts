import { ChangeDetectionStrategy, Component } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { errorHandler, handleErrors } from '../../../../modules/operators/handleErrors';
import { NotificationType } from '../../../../modules/notification/notification-type';
import { NotificationService } from '../../../../modules/notification/notification.service';
import { Observable } from 'rxjs';
import { FormModalContent } from '../../../../modules/forms/form-modal/form-modal-content';
import { UpdateLeadModalContentState } from './update-lead-modal-content-state';
import { LeadService } from '../../../../modules/lead/lead.service';
import { UpdateLeadRequest } from '../../../../modules/lead/update-lead-request.model';

@UntilDestroy()
@Component({
  selector: 'bip-update-lead-modal-content',
  templateUrl: './update-lead-modal-content.component.html',
  styleUrls: ['./update-lead-modal-content.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UpdateLeadModalContentComponent extends FormModalContent<UpdateLeadModalContentState> {
  constructor(private readonly leadService: LeadService, private readonly notificationService: NotificationService) {
    super();
  }

  onSubmit(): Observable<void> {
    const controls = this.state.formGroup.bipControls;
    const updateLeadRequest: UpdateLeadRequest = {
      name: controls.name.value,
      contactName: controls.contactName.value,
      contactPhone: controls.phone.value,
      contactEmail: controls.email.value,
      cost: controls.cost.value,
      startDate: controls.dateRange.value[0],
      endDate: controls.dateRange.value[1],
    };

    return this.leadService.updateLead(this.state.lead.name, updateLeadRequest).pipe(
      untilDestroyed(this),
      handleErrors([
        errorHandler(() => {
          this.notificationService.notify(
            NotificationType.Error,
            'An error has occurred',
            `The lead ${this.state.lead.name} was not updated`,
          );
        }),
      ]),
    );
  }
}
