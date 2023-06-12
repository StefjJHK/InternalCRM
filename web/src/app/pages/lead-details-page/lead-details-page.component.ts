import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { switchMap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { NotificationService } from '../../../modules/notification/notification.service';
import { errorHandler, handleErrors } from '../../../modules/operators/handleErrors';
import { NotificationType } from '../../../modules/notification/notification-type';
import { PageState } from '../../../modules/page/page-state';
import { LeadService } from '../../../modules/lead/lead.service';
import { Lead } from '../../../modules/lead/lead.model';
import { Button } from '../../../modules/common-components/button/button.model';
import { DeleteConfirmationService } from '../../../modules/forms/delete-confirmation/delete-confirmation.service';
import { AppRoutes } from '../../routing/app-routes';
import { PermissionsService } from '../../../modules/permissions/permissions.service';
import { BipFormGroup } from '../../../modules/forms/form-controls/bip-form-group';
import { BipFormControl } from '../../../modules/forms/form-controls/bip-form-control';
import { BipValidators } from '../../../modules/forms/bip-validators';
import { FormState } from '../../../modules/forms/form-modal/form-modal-content/form-state';
import { FormModalService } from '../../../modules/forms/form-modal/form-modal.service';
import { UpdateLeadModalContentComponent } from './update-lead-modal-content/update-lead-modal-content.component';
import { UpdateLeadModalContentState } from './update-lead-modal-content/update-lead-modal-content-state';

@UntilDestroy()
@Component({
  selector: 'bip-lead-details-page',
  templateUrl: './lead-details-page.component.html',
  styleUrls: ['./lead-details-page.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LeadDetailsPageComponent implements OnInit {
  headerButtons: Button[] = [];

  pageState: PageState = PageState.loaded;

  lead?: Lead;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly leadService: LeadService,
    private readonly notificationService: NotificationService,
    private readonly router: Router,
    private readonly deleteConfirmationService: DeleteConfirmationService,
    private readonly permissionsService: PermissionsService,
    private readonly formModalService: FormModalService,
    private readonly changeDetectorRef: ChangeDetectorRef,
  ) {}

  ngOnInit() {
    if (this.permissionsService.containsPermissions({ lead: { canWrite: true } })) {
      this.headerButtons = [
        ...this.headerButtons,
        {
          type: 'default',
          text: 'Edit',

          onClick: () => this.onUpdateLead(),
        },
        {
          type: 'danger',
          text: 'Delete',

          onClick: () => this.onDeleteLead(),
        },
      ];
    }

    this.fetchLead();
  }

  fetchLead() {
    this.pageState = PageState.loading;

    this.route.params
      .pipe(
        untilDestroyed(this),
        switchMap((params) => this.leadService.getLead(params['leadName'])),
        handleErrors([
          errorHandler(() => {
            this.notificationService.notify(NotificationType.Error, 'Error', 'Unknown error has occurred while loading lead');
            this.pageState = PageState.error;
          }),
        ]),
      )
      .subscribe((lead: Lead) => {
        this.lead = lead;
        this.pageState = PageState.loaded;

        this.changeDetectorRef.markForCheck();
      });
  }

  private onUpdateLead() {
    const lead = this.lead!;
    const formGroup: BipFormGroup = new BipFormGroup({
      name: new BipFormControl<string>(lead.name, 'Name', { validators: [BipValidators.required()] }),
      contactName: new BipFormControl<string>(lead.contactName, 'Contact name', { validators: [BipValidators.required()] }),
      cost: new BipFormControl<number>(lead.cost, 'Cost', { validators: [BipValidators.required()] }),
      dateRange: new BipFormControl<Date[]>([lead.startDate, lead.endDate], 'On date', { validators: [BipValidators.required()] }),
      phone: new BipFormControl<string>(lead.phoneNumber, 'Phone', { validators: [BipValidators.required(), BipValidators.phoneNumber()] }),
      email: new BipFormControl<string>(lead.email, 'Email', { validators: [BipValidators.required(), BipValidators.email()] }),
    });

    const onSubmit = () => {
      this.notificationService.notify(NotificationType.Success, 'Success', `The lead ${lead.name} was updated`);
      this.router.navigate([AppRoutes.Leads, formGroup.bipControls.name.value]);
    };
    const submitEnabled = (formState: FormState) => {
      if (formState == FormState.Loading || formState == FormState.Submitting) {
        return false;
      }

      return (
        lead.name != formGroup.bipControls.name.value ||
        lead.contactName != formGroup.bipControls.contactName.value ||
        lead.cost != formGroup.bipControls.cost.value ||
        lead.startDate != formGroup.bipControls.dateRange.value[0] ||
        lead.endDate != formGroup.bipControls.dateRange.value[1] ||
        lead.email != formGroup.bipControls.email.value ||
        lead.phoneNumber != formGroup.bipControls.phone.value
      );
    };

    this.formModalService.show<UpdateLeadModalContentComponent, UpdateLeadModalContentState>(
      'Update lead',
      UpdateLeadModalContentComponent,
      formGroup,
      onSubmit,
      submitEnabled,
      { lead },
    );
  }

  private onDeleteLead() {
    const leadName = this.lead!.name;
    const onDelete = (): void => {
      this.router.navigate([AppRoutes.Leads]);
      this.notificationService.notify(NotificationType.Success, 'Success', `Lead ${leadName} was deleted`);
    };

    this.deleteConfirmationService.confirmDelete('Are you sure to delete lead?', leadName, this.leadService.deleteLead(leadName), onDelete);
  }
}
