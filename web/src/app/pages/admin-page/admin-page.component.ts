import { ChangeDetectionStrategy, Component, OnInit, ViewChild } from '@angular/core';
import { PageState } from '../../../modules/page/page-state';
import { UserAccessService } from '../../../modules/user/user-access.service';
import { User } from '../../../modules/user/user.model';
import { Button } from '../../../modules/common-components/button/button.model';
import { BipFormControl } from '../../../modules/forms/form-controls/bip-form-control';
import { BipValidators } from '../../../modules/forms/bip-validators';
import { BipFormGroup } from '../../../modules/forms/form-controls/bip-form-group';
import { FormModalService } from '../../../modules/forms/form-modal/form-modal.service';
import { CreateUserModalContentComponent } from './create-user-modal-content/create-user-modal-content.component';
import { NotificationService } from '../../../modules/notification/notification.service';
import { NotificationType } from '../../../modules/notification/notification-type';
import { UsersTableWidgetComponent } from '../../../modules/user/users-table-widget/users-table-widget.component';

@Component({
  selector: 'bip-admin-page',
  templateUrl: './admin-page.component.html',
  styleUrls: ['./admin-page.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AdminPageComponent implements OnInit {
  @ViewChild(UsersTableWidgetComponent) usersTableWidget!: UsersTableWidgetComponent;

  pageState: PageState = PageState.loaded;

  user: User | null = null;

  headerButtons: Button[] = [
    {
      text: 'Add New',
      type: 'primary',

      icon: 'plus',

      onClick: () => this.onCreateUser(),
    },
  ];

  constructor(
    private readonly userAccessService: UserAccessService,
    private readonly formModalService: FormModalService,
    private readonly notificationService: NotificationService,
  ) {}

  ngOnInit() {
    this.user = this.userAccessService.user;
  }

  private onCreateUser() {
    const formGroup = new BipFormGroup({
      username: new BipFormControl<string>('', 'Username', { validators: BipValidators.required() }),
      password: new BipFormControl<string>('', 'Password', { validators: BipValidators.required() }),
      analyticsCanWrite: new BipFormControl<boolean>(false, 'Analytics can write'),
      analyticsCanRead: new BipFormControl<boolean>(false, 'Analytics can read'),
      productCanWrite: new BipFormControl<boolean>(false, 'Product can write'),
      productCanRead: new BipFormControl<boolean>(false, 'Product can read'),
      customerCanWrite: new BipFormControl<boolean>(false, 'Customer can write'),
      customerCanRead: new BipFormControl<boolean>(false, 'Customer can read'),
      leadCanWrite: new BipFormControl<boolean>(false, 'Lead can write'),
      leadCanRead: new BipFormControl<boolean>(false, 'Lead can read'),
      purchaseOrderCanWrite: new BipFormControl<boolean>(false, 'Purchase order can write'),
      purchaseOrderCanRead: new BipFormControl<boolean>(false, 'Purchase order can read'),
      invoiceCanWrite: new BipFormControl<boolean>(false, 'Invoice can write'),
      invoiceCanRead: new BipFormControl<boolean>(false, 'Invoice can read'),
      paymentCanWrite: new BipFormControl<boolean>(false, 'Payment can write'),
      paymentCanRead: new BipFormControl<boolean>(false, 'Invoice order can read'),
      subscriptionCanWrite: new BipFormControl<boolean>(false, 'Subscription can write'),
      subscriptionCanRead: new BipFormControl<boolean>(false, 'Subscription can read'),
    });

    const onSubmit = (): void => {
      this.notificationService.notify(NotificationType.Success, 'Success', `user ${formGroup.bipControls.username.value} was created.`);

      this.usersTableWidget.fetchUsers();
    };

    this.formModalService.show('Create user', CreateUserModalContentComponent, formGroup, onSubmit);
  }
}
