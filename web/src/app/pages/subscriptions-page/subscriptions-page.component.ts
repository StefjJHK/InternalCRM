import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { FormModalService } from '../../../modules/forms/form-modal/form-modal.service';
import { BipFormGroup } from '../../../modules/forms/form-controls/bip-form-group';
import { BipFormControl } from '../../../modules/forms/form-controls/bip-form-control';
import { BipValidators } from '../../../modules/forms/bip-validators';
import { CreateSubscriptionModalContent } from './create-subscription-modal-content/create-subscription-modal-content';
import { NotificationService } from '../../../modules/notification/notification.service';
import { NotificationType } from '../../../modules/notification/notification-type';
import { Button } from '../../../modules/common-components/button/button.model';
import { PageState } from '../../../modules/page/page-state';
import { PermissionsService } from '../../../modules/permissions/permissions.service';

@Component({
  selector: 'bip-leads-page',
  templateUrl: './subscriptions-page.component.html',
  styleUrls: ['./subscriptions-page.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SubscriptionsPageComponent implements OnInit {
  pageState: PageState = PageState.loaded;

  headerActions: Button[] = [];

  constructor(
    private readonly formModalService: FormModalService,
    private readonly notificationService: NotificationService,
    private readonly permissionsService: PermissionsService,
  ) {}

  ngOnInit() {
    if (this.permissionsService.containsPermissions({ subscription: { canWrite: true } })) {
      this.headerActions = [
        ...this.headerActions,
        {
          text: 'Add New',
          type: 'primary',
          icon: 'plus',

          onClick: () => this.onCreateSubscriptionClick(),
        },
      ];
    }
  }

  private onCreateSubscriptionClick() {
    const formGroup: BipFormGroup = new BipFormGroup({
      invoiceNumber: new BipFormControl<string>('', 'Invoice', { validators: [BipValidators.required()] }),
      subLegalEntity: new BipFormControl<string>('', 'Sublegal', { validators: [BipValidators.required()] }),
      cost: new BipFormControl<number | null>(null, 'Cost', { validators: [BipValidators.required()] }),
      dateRange: new BipFormControl<Date[]>([], 'Valid From-until', { validators: [BipValidators.required()] }),
    });

    const onSubmit = () => {
      this.notificationService.notify(
        NotificationType.Success,
        'Success',
        `The subscription ${formGroup.bipControls.invoiceNumber} was created`,
      );
    };

    this.formModalService.show('New subscription', CreateSubscriptionModalContent, formGroup, onSubmit);
  }
}
