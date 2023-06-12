import { ChangeDetectionStrategy, Component, OnInit, ViewChild } from '@angular/core';
import { FormModalService } from '../../../modules/forms/form-modal/form-modal.service';
import { BipFormGroup } from '../../../modules/forms/form-controls/bip-form-group';
import { BipFormControl } from '../../../modules/forms/form-controls/bip-form-control';
import { BipValidators } from '../../../modules/forms/bip-validators';
import { CreateCustomerModalContentComponent } from './create-customer-modal-content/create-customer-modal-content.component';
import { NotificationService } from '../../../modules/notification/notification.service';
import { NotificationType } from '../../../modules/notification/notification-type';
import { Button } from '../../../modules/common-components/button/button.model';
import { CustomersTableWidgetComponent } from '../../../modules/customer/customers-table-widget/customers-table-widget.component';
import { PageState } from '../../../modules/page/page-state';
import { PermissionsService } from '../../../modules/permissions/permissions.service';

@Component({
  selector: 'bip-customers-page',
  templateUrl: './customers-page.component.html',
  styleUrls: ['./customers-page.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomersPageComponent implements OnInit {
  @ViewChild(CustomersTableWidgetComponent) customersTableWidget!: CustomersTableWidgetComponent;

  onCreateCustomerClick = () => {
    const formGroup: BipFormGroup = new BipFormGroup({
      name: new BipFormControl<string>('', 'Name', { validators: [BipValidators.required()] }),
      contactName: new BipFormControl<string>('', 'Contact name', { validators: [BipValidators.required()] }),
      email: new BipFormControl<string>('', 'Email', { validators: [BipValidators.required(), BipValidators.email()] }),
      phone: new BipFormControl<string>('', 'Phone', { validators: [BipValidators.required(), BipValidators.phoneNumber()] }),
    });

    const onSubmit = () => {
      this.notificationService.notify(NotificationType.Success, 'Success', `The customer ${formGroup.bipControls.name.value} was created`);
      this.customersTableWidget.fetchCustomers();
    };

    this.formModalService.show('New Customer', CreateCustomerModalContentComponent, formGroup, onSubmit);
  };

  pageState: PageState = PageState.loaded;

  headerButtons: Button[] = [];

  constructor(
    private readonly formModalService: FormModalService,
    private readonly notificationService: NotificationService,
    private readonly permissionsService: PermissionsService,
  ) {}

  ngOnInit() {
    if (this.permissionsService.containsPermissions({ customer: { canWrite: true } })) {
      this.headerButtons = [
        ...this.headerButtons,
        {
          text: 'Add New',
          type: 'primary',
          icon: 'plus',
          onClick: this.onCreateCustomerClick,
        },
      ];
    }
  }
}
