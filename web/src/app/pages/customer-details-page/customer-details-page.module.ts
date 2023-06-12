import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CustomerDetailsPageComponent } from './customer-details-page.component';
import { PageModule } from '../../../modules/page/page.module';
import { TabsetModule } from '../../../modules/common-components/tabset/tabset.module';
import { NzDescriptionsModule } from 'ng-zorro-antd/descriptions';
import { ProductsTableWidgetModule } from '../../../modules/product/products-table-widget/products-table-widget.module';
import { PurchaseOrdersTableWidgetModule } from '../../../modules/purchase-order/purchase-orders-table-widget/purchase-orders-table-widget.module';
import { InvoicesTableWidgetModule } from '../../../modules/invoice/invoices-table-widget/invoices-table-widget.module';
import { SubscriptionsTableWidgetModule } from '../../../modules/subscription/subscriptions-table-widget/subscriptions-table-widget.module';
import { PaymentsTableWidgetModule } from '../../../modules/payments/payments-table-widget/payments-table-widget.module';
import { UpdateCustomerModalContentComponent } from './update-customer-modal-content/update-customer-modal-content.component';
import { FormModalModule } from '../../../modules/forms/form-modal/form-modal.module';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { InputFieldModule } from '../../../modules/forms/fields/input-field/input-field.module';
import { NzFormModule } from 'ng-zorro-antd/form';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [CustomerDetailsPageComponent, UpdateCustomerModalContentComponent],
  imports: [
    CommonModule,
    PageModule,
    TabsetModule,
    NzDescriptionsModule,
    ProductsTableWidgetModule,
    PurchaseOrdersTableWidgetModule,
    InvoicesTableWidgetModule,
    SubscriptionsTableWidgetModule,
    PaymentsTableWidgetModule,
    FormModalModule,
    NzSpaceModule,
    InputFieldModule,
    NzSpaceModule,
    NzFormModule,
    ReactiveFormsModule,
  ],
  exports: [CustomerDetailsPageComponent],
})
export class CustomerDetailsPageModule {}
