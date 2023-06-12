import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InvoiceDetailsPageComponent } from './invoice-details-page.component';
import { PageModule } from '../../../modules/page/page.module';
import { TabsetModule } from '../../../modules/common-components/tabset/tabset.module';
import { NzDescriptionsModule } from 'ng-zorro-antd/descriptions';
import { InvoicesTableWidgetModule } from '../../../modules/invoice/invoices-table-widget/invoices-table-widget.module';
import { SubscriptionsTableWidgetModule } from '../../../modules/subscription/subscriptions-table-widget/subscriptions-table-widget.module';
import { UpdateInvoiceModalContentComponent } from './update-invoice-modal-content/update-invoice-modal-content.component';
import { FormModalModule } from '../../../modules/forms/form-modal/form-modal.module';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { InputFieldModule } from '../../../modules/forms/fields/input-field/input-field.module';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { DatePickerFieldModule } from '../../../modules/forms/fields/date-picker-field/date-picker-field.module';
import { ReactiveFormsModule } from '@angular/forms';
import { NzFormModule } from 'ng-zorro-antd/form';

@NgModule({
  declarations: [InvoiceDetailsPageComponent, UpdateInvoiceModalContentComponent],
  imports: [
    CommonModule,
    PageModule,
    TabsetModule,
    NzDescriptionsModule,
    InvoicesTableWidgetModule,
    SubscriptionsTableWidgetModule,
    FormModalModule,
    NzSpaceModule,
    InputFieldModule,
    NzGridModule,
    DatePickerFieldModule,
    ReactiveFormsModule,
    NzFormModule,
  ],
  exports: [InvoiceDetailsPageComponent],
})
export class InvoiceDetailsPageModule {}
