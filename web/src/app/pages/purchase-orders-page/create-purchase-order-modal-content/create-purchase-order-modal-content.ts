import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ProductService } from '../../../../modules/product/product.service';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { CustomerService } from '../../../../modules/customer/customer.service';
import { AddPurchaseOrderRequest } from '../../../../modules/purchase-order/add-purchase-order-request';
import { errorHandler, handleErrors } from '../../../../modules/operators/handleErrors';
import { NotificationType } from '../../../../modules/notification/notification-type';
import { Observable, zip } from 'rxjs';
import { FormModalContent } from '../../../../modules/forms/form-modal/form-modal-content';
import { PurchaseOrderService } from '../../../../modules/purchase-order/purchase-order.service';
import { NotificationService } from '../../../../modules/notification/notification.service';
import { FormState } from '../../../../modules/forms/form-modal/form-modal-content/form-state';
import { Product } from '../../../../modules/product/product.model';
import { Customer } from '../../../../modules/customer/customer.model';
import { SelectFieldOption } from '../../../../modules/forms/fields/select-field/select-field-option';

@UntilDestroy()
@Component({
  selector: 'bip-create-subscription-modal-content',
  templateUrl: './create-purchase-order-modal-content.html',
  styleUrls: ['./create-purchase-order-modal-content.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CreatePurchaseOrderModalContent extends FormModalContent implements OnInit {
  productSelectOptions: SelectFieldOption[] = [];

  customerSelectOptions: SelectFieldOption[] = [];

  constructor(
    private readonly productService: ProductService,
    private readonly customerService: CustomerService,
    private readonly purchaseOrderService: PurchaseOrderService,
    private readonly notificationService: NotificationService,
    private readonly changeDetectionRef: ChangeDetectorRef,
  ) {
    super();
  }

  ngOnInit() {
    this.state.formState$.next(FormState.Loading);

    zip(this.productService.getProducts({}), this.customerService.getCustomers({}))
      .pipe(untilDestroyed(this), handleErrors([errorHandler(() => this.state.formState$.next(FormState.Error))]))
      .subscribe(([products, customers]) => {
        this.productSelectOptions = products.map((product: Product) => ({
          label: product.name,
          value: product.name,
        }));
        this.customerSelectOptions = customers.map((customer: Customer) => ({
          label: customer.name,
          value: customer.name,
        }));

        this.state.formState$.next(FormState.Filling);

        this.changeDetectionRef.markForCheck();
      });
  }

  onSubmit(): Observable<void> {
    const addPurchaseOrderRequest: AddPurchaseOrderRequest = {
      customerName: this.state.formGroup.bipControls.customerName.value,
      productName: this.state.formGroup.bipControls.productName.value,
      number: this.state.formGroup.bipControls.number.value,
      amount: this.state.formGroup.bipControls.amount.value,
      receivedDate: this.state.formGroup.bipControls.receivedDate.value.toISOString(),
      dueDate: this.state.formGroup.bipControls.dueDate.value.toISOString(),
    };

    return this.purchaseOrderService.addPurchaseOrder(addPurchaseOrderRequest).pipe(
      untilDestroyed(this),
      handleErrors([
        errorHandler(() => {
          this.notificationService.notify(NotificationType.Error, 'An error has occurred', 'The purchase order was not created');
        }),
      ]),
    );
  }
}
