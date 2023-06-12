import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { switchMap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { NotificationService } from '../../../modules/notification/notification.service';
import { errorHandler, handleErrors } from '../../../modules/operators/handleErrors';
import { NotificationType } from '../../../modules/notification/notification-type';
import { PageState } from '../../../modules/page/page-state';
import { CustomerService } from '../../../modules/customer/customer.service';
import { Customer } from '../../../modules/customer/customer.model';
import { Button } from '../../../modules/common-components/button/button.model';
import { AppRoutes } from '../../routing/app-routes';
import { DeleteConfirmationService } from '../../../modules/forms/delete-confirmation/delete-confirmation.service';
import { PermissionsService } from '../../../modules/permissions/permissions.service';
import { BipFormGroup } from '../../../modules/forms/form-controls/bip-form-group';
import { BipFormControl } from '../../../modules/forms/form-controls/bip-form-control';
import { BipValidators } from '../../../modules/forms/bip-validators';
import { FormState } from '../../../modules/forms/form-modal/form-modal-content/form-state';
import { FormModalService } from '../../../modules/forms/form-modal/form-modal.service';
import { UpdateCustomerModalContentState } from './update-customer-modal-content/update-customer-modal-content-state';
import { UpdateCustomerModalContentComponent } from './update-customer-modal-content/update-customer-modal-content.component';

@UntilDestroy()
@Component({
  selector: 'bip-product-details-page',
  templateUrl: './customer-details-page.component.html',
  styleUrls: ['./customer-details-page.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerDetailsPageComponent implements OnInit {
  headerButtons: Button[] = [];

  pageState: PageState = PageState.loaded;

  customer?: Customer;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly customerService: CustomerService,
    private readonly notificationService: NotificationService,
    private readonly deleteConfirmationService: DeleteConfirmationService,
    private readonly permissionsService: PermissionsService,
    private readonly formModalService: FormModalService,
    private readonly router: Router,
    private readonly changeDetectorRef: ChangeDetectorRef,
  ) {}

  ngOnInit() {
    if (this.permissionsService.containsPermissions({ customer: { canWrite: true } })) {
      this.headerButtons = [
        ...this.headerButtons,
        {
          type: 'default',
          text: 'Edit',

          onClick: () => this.onUpdateCustomer(),
        },
        {
          type: 'danger',
          text: 'Delete',

          onClick: () => this.onDeleteCustomer(),
        },
      ];
    }

    this.fetchCustomer();
  }

  fetchCustomer() {
    this.pageState = PageState.loading;

    this.route.params
      .pipe(
        untilDestroyed(this),
        switchMap((params) => this.customerService.getCustomer(params['customerName'])),
        handleErrors([
          errorHandler(() => {
            this.notificationService.notify(NotificationType.Error, 'Error', 'Unknown error has occurred while loading customer');
            this.pageState = PageState.error;
          }),
        ]),
      )
      .subscribe((customer) => {
        this.customer = customer;
        this.pageState = PageState.loaded;

        this.changeDetectorRef.markForCheck();
      });
  }

  private onUpdateCustomer() {
    const customer = this.customer!;
    const formGroup: BipFormGroup = new BipFormGroup({
      name: new BipFormControl<string>(customer.name, 'Name', { validators: [BipValidators.required()] }),
      contactName: new BipFormControl<string>(customer.contactName, 'Contact name', { validators: [BipValidators.required()] }),
      email: new BipFormControl<string>(customer.email, 'Email', { validators: [BipValidators.required(), BipValidators.email()] }),
      phone: new BipFormControl<string>(customer.phoneNumber, 'Phone', {
        validators: [BipValidators.required(), BipValidators.phoneNumber()],
      }),
    });

    const onSubmit = () => {
      this.notificationService.notify(NotificationType.Success, 'Success', `The customer ${customer.name} was updated`);
      this.router.navigate([AppRoutes.Customers, formGroup.bipControls.name.value]);
    };
    const submitEnabled = (formState: FormState) => {
      if (formState == FormState.Loading || formState == FormState.Submitting) {
        return false;
      }

      return (
        customer.name != formGroup.bipControls.name.value ||
        customer.email != formGroup.bipControls.email.value ||
        customer.phoneNumber != formGroup.bipControls.phone.value ||
        customer.contactName != formGroup.bipControls.contactName.value
      );
    };

    this.formModalService.show<UpdateCustomerModalContentComponent, UpdateCustomerModalContentState>(
      'Update customer',
      UpdateCustomerModalContentComponent,
      formGroup,
      onSubmit,
      submitEnabled,
      { customer },
    );
  }

  private onDeleteCustomer() {
    const customerName = this.customer!.name;
    const onDelete = (): void => {
      this.router.navigate([AppRoutes.Customers]);
      this.notificationService.notify(NotificationType.Success, 'Success', `Customer ${customerName} was deleted`);
    };

    this.deleteConfirmationService.confirmDelete(
      'Are you sure to delete customer?',
      customerName,
      this.customerService.deleteCustomer(customerName),
      onDelete,
    );
  }
}
