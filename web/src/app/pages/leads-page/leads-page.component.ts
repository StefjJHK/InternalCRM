import { ChangeDetectionStrategy, Component, OnInit, ViewChild } from '@angular/core';
import { FormModalService } from '../../../modules/forms/form-modal/form-modal.service';
import { CreateLeadModalContentComponent } from './create-lead-modal-content/create-lead-modal-content.component';
import { BipFormGroup } from '../../../modules/forms/form-controls/bip-form-group';
import { BipFormControl } from '../../../modules/forms/form-controls/bip-form-control';
import { BipValidators } from '../../../modules/forms/bip-validators';
import { NotificationService } from '../../../modules/notification/notification.service';
import { NotificationType } from '../../../modules/notification/notification-type';
import { Button } from '../../../modules/common-components/button/button.model';
import { LeadsTableWidgetComponent } from '../../../modules/lead/leads-table-widget/leads-table-widget.component';
import { PageState } from '../../../modules/page/page-state';
import { PermissionsService } from '../../../modules/permissions/permissions.service';

@Component({
  selector: 'bip-leads-page',
  templateUrl: './leads-page.component.html',
  styleUrls: ['./leads-page.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LeadsPageComponent implements OnInit {
  @ViewChild(LeadsTableWidgetComponent) leadsTableWidget!: LeadsTableWidgetComponent;

  pageState: PageState = PageState.loaded;

  headerButtons: Button[] = [];

  constructor(
    private readonly formModalService: FormModalService,
    private readonly notificationService: NotificationService,
    private readonly permissionsService: PermissionsService,
  ) {}

  ngOnInit() {
    if (this.permissionsService.containsPermissions({ lead: { canWrite: true } })) {
      this.headerButtons = [
        ...this.headerButtons,
        {
          text: 'Add New',
          type: 'primary',
          icon: 'plus',

          onClick: () => this.onCreateLeadClick(),
        },
      ];
    }
  }

  private onCreateLeadClick() {
    const formGroup: BipFormGroup = new BipFormGroup({
      name: new BipFormControl<string>('', 'Name', { validators: [BipValidators.required()] }),
      contactName: new BipFormControl<string>('', 'Contact name', { validators: [BipValidators.required()] }),
      productName: new BipFormControl<string>('', 'Product', { validators: [BipValidators.required()] }),
      cost: new BipFormControl<number | null>(null, 'Cost', { validators: [BipValidators.required()] }),
      dateRange: new BipFormControl<Date[]>([], 'On date', { validators: [BipValidators.required()] }),
      phone: new BipFormControl<string>('', 'Phone', { validators: [BipValidators.required(), BipValidators.phoneNumber()] }),
      email: new BipFormControl<string>('', 'Email', { validators: [BipValidators.required(), BipValidators.email()] }),
    });

    const onSubmit = () => {
      this.notificationService.notify(NotificationType.Success, 'Success', `The lead ${formGroup.bipControls.name.value} was created`);
      this.leadsTableWidget.fetchLeads();
    };

    this.formModalService.show('Create lead', CreateLeadModalContentComponent, formGroup, onSubmit);
  }
}
