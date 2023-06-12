import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ProductService } from '../../../../modules/product/product.service';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { CustomerService } from '../../../../modules/customer/customer.service';
import { PurchaseOrderService } from '../../../../modules/purchase-order/purchase-order.service';
import { PurchaseOrder } from '../../../../modules/purchase-order/purchase-order.model';
import { AddInvoiceRequest } from '../../../../modules/invoice/add-invoice-request.model';
import { errorHandler, handleErrors } from '../../../../modules/operators/handleErrors';
import { NotificationType } from '../../../../modules/notification/notification-type';
import { Observable, zip } from 'rxjs';
import { InvoiceService } from '../../../../modules/invoice/invoice.service';
import { NotificationService } from '../../../../modules/notification/notification.service';
import { FormModalContent } from '../../../../modules/forms/form-modal/form-modal-content';
import { FormState } from '../../../../modules/forms/form-modal/form-modal-content/form-state';
import { Product } from '../../../../modules/product/product.model';
import { Customer } from '../../../../modules/customer/customer.model';
import { SelectFieldOption } from '../../../../modules/forms/fields/select-field/select-field-option';

@UntilDestroy()
@Component({
  selector: 'bip-create-subscription-modal-content',
  templateUrl: './create-invoice-modal-content.html',
  styleUrls: ['./create-invoice-modal-content.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CreateInvoiceModalContent extends FormModalContent implements OnInit {
  purchaseOrders: PurchaseOrder[] = [];

  purchaseOrderSelectOptions: SelectFieldOption[] = [];

  productSelectOptions: SelectFieldOption[] = [];

  customerSelectOptions: SelectFieldOption[] = [];

  constructor(
    private readonly productService: ProductService,
    private readonly customerService: CustomerService,
    private readonly purchaseOrderService: PurchaseOrderService,
    private readonly invoiceService: InvoiceService,
    private readonly notificationService: NotificationService,
    private readonly changeDetectionRef: ChangeDetectorRef,
  ) {
    super();
  }

  ngOnInit() {
    this.state.formGroup.bipControls.purchaseOrderNumber.valueChanges
      .pipe(untilDestroyed(this))
      .subscribe((purchaseOrderNumber: string | null) => {
        if (purchaseOrderNumber != null) {
          const purchaseOrder = this.purchaseOrders.find((x) => x.number === purchaseOrderNumber)!;

          this.state.formGroup.bipControls.productName.setValue(purchaseOrder.productName);
          this.state.formGroup.bipControls.customerName.setValue(purchaseOrder.customerName);
        } else {
          this.state.formGroup.bipControls.productName.setValue(null);
          this.state.formGroup.bipControls.customerName.setValue(null);
        }
      });

    this.state.formState$.next(FormState.Loading);

    zip(this.purchaseOrderService.getPurchaseOrders({}), this.productService.getProducts({}), this.customerService.getCustomers({}))
      .pipe(untilDestroyed(this), handleErrors([errorHandler(() => this.state.formState$.next(FormState.Error))]))
      .subscribe(([purchaseOrders, products, customers]) => {
        this.purchaseOrders = purchaseOrders;
        this.purchaseOrderSelectOptions = purchaseOrders.map((purchaseOrder: PurchaseOrder) => ({
          label: purchaseOrder.number,
          value: purchaseOrder.number,
        }));
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
    const addInvoiceRequest: AddInvoiceRequest = {
      customerName: this.state.formGroup.bipControls.customerName.value,
      productName: this.state.formGroup.bipControls.productName.value,
      purchaseOrderNumber: this.state.formGroup.bipControls.purchaseOrderNumber.value,
      number: this.state.formGroup.bipControls.number.value,
      amount: this.state.formGroup.bipControls.amount.value,
      receivedDate: this.state.formGroup.bipControls.receivedDate.value.toISOString(),
      dueDate: this.state.formGroup.bipControls.dueDate.value.toISOString(),
    };

    return this.invoiceService.addInvoice(addInvoiceRequest).pipe(
      untilDestroyed(this),
      handleErrors([
        errorHandler(() => {
          this.notificationService.notify(NotificationType.Error, 'An error has occurred', 'The invoice was not created');
        }),
      ]),
    );
  }
}
