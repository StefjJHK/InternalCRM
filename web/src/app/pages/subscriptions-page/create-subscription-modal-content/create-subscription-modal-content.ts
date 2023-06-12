import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { InvoiceService } from '../../../../modules/invoice/invoice.service';
import { AddSubscriptionRequest } from '../../../../modules/subscription/add-subscription-request.model';
import { errorHandler, handleErrors } from '../../../../modules/operators/handleErrors';
import { NotificationType } from '../../../../modules/notification/notification-type';
import { FormModalContent } from '../../../../modules/forms/form-modal/form-modal-content';
import { Observable } from 'rxjs';
import { SubscriptionService } from '../../../../modules/subscription/subscription.service';
import { NotificationService } from '../../../../modules/notification/notification.service';
import { FormState } from '../../../../modules/forms/form-modal/form-modal-content/form-state';
import { Invoice } from '../../../../modules/invoice/invoice.model';
import { SelectFieldOption } from '../../../../modules/forms/fields/select-field/select-field-option';

@UntilDestroy()
@Component({
  selector: 'bip-create-subscription-modal-content',
  templateUrl: './create-subscription-modal-content.html',
  styleUrls: ['./create-subscription-modal-content.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CreateSubscriptionModalContent extends FormModalContent implements OnInit {
  invoiceSelectOptions: SelectFieldOption[] = [];

  constructor(
    private readonly invoiceService: InvoiceService,
    private readonly subscriptionService: SubscriptionService,
    private readonly notificationService: NotificationService,
    private readonly changeDetectionRef: ChangeDetectorRef,
  ) {
    super();
  }

  ngOnInit() {
    this.state.formState$.next(FormState.Loading);

    this.invoiceService
      .getInvoices({})
      .pipe(untilDestroyed(this), handleErrors([errorHandler(() => this.state.formState$.next(FormState.Error))]))
      .subscribe((invoices) => {
        this.invoiceSelectOptions = invoices.map((invoice: Invoice) => ({
          label: invoice.number,
          value: invoice.number,
        }));

        this.state.formState$.next(FormState.Filling);
        this.changeDetectionRef.markForCheck();
      });
  }

  onSubmit(): Observable<void> {
    const invoiceNumber = this.state.formGroup.bipControls.invoiceNumber.value;
    const addSubscriptionRequest: AddSubscriptionRequest = {
      subLegalEntity: this.state.formGroup.bipControls.subLegalEntity.value,
      cost: this.state.formGroup.bipControls.cost.value,
      validFrom: this.state.formGroup.bipControls.dateRange.value[0].toISOString(),
      validUntil: this.state.formGroup.bipControls.dateRange.value[1].toISOString(),
    };

    return this.subscriptionService.addSubscription(invoiceNumber, addSubscriptionRequest).pipe(
      untilDestroyed(this),
      handleErrors([
        errorHandler(() => {
          this.notificationService.notify(NotificationType.Error, 'An error has occurred', 'The subscription was not created');
        }),
      ]),
    );
  }
}
