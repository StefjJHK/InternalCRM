import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ProductService } from '../../../../modules/product/product.service';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { AddLeadRequest } from '../../../../modules/lead/add-lead-request.model';
import { errorHandler, handleErrors } from '../../../../modules/operators/handleErrors';
import { NotificationType } from '../../../../modules/notification/notification-type';
import { LeadService } from '../../../../modules/lead/lead.service';
import { NotificationService } from '../../../../modules/notification/notification.service';
import { FormModalContent } from '../../../../modules/forms/form-modal/form-modal-content';
import { Observable } from 'rxjs';
import { FormState } from '../../../../modules/forms/form-modal/form-modal-content/form-state';
import { Product } from '../../../../modules/product/product.model';
import { SelectFieldOption } from '../../../../modules/forms/fields/select-field/select-field-option';

@UntilDestroy()
@Component({
  selector: 'bip-create-subscription-modal-content',
  templateUrl: './create-lead-modal-content.component.html',
  styleUrls: ['./create-lead-modal-content.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CreateLeadModalContentComponent extends FormModalContent implements OnInit {
  productSelectOptions: SelectFieldOption[] = [];

  constructor(
    private readonly productService: ProductService,
    private readonly leadService: LeadService,
    private readonly notificationService: NotificationService,
    private readonly changeDetectionRef: ChangeDetectorRef,
  ) {
    super();
  }

  ngOnInit() {
    this.state.formState$.next(FormState.Loading);

    this.productService
      .getProducts({})
      .pipe(untilDestroyed(this), handleErrors([errorHandler(() => this.state.formState$.next(FormState.Error))]))
      .subscribe((products) => {
        this.productSelectOptions = products.map((product: Product) => ({
          label: product.name,
          value: product.name,
        }));

        this.state.formState$.next(FormState.Filling);

        this.changeDetectionRef.markForCheck();
      });
  }

  onSubmit(): Observable<void> {
    const addLeadRequest: AddLeadRequest = {
      name: this.state.formGroup.bipControls.name.value,
      productName: this.state.formGroup.bipControls.productName.value,
      contactName: this.state.formGroup.bipControls.contactName.value,
      contactPhone: this.state.formGroup.bipControls.phone.value,
      contactEmail: this.state.formGroup.bipControls.email.value,
      cost: this.state.formGroup.bipControls.cost.value,
      startDate: this.state.formGroup.bipControls.dateRange.value[0].toISOString(),
      endDate: this.state.formGroup.bipControls.dateRange.value[1].toISOString(),
    };

    return this.leadService.addLead(addLeadRequest).pipe(
      untilDestroyed(this),
      handleErrors([
        errorHandler(() => {
          this.notificationService.notify(NotificationType.Error, 'An error has occurred', 'The lead was not created');
        }),
      ]),
    );
  }
}
