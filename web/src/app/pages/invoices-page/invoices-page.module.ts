import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { ReactiveFormsModule } from '@angular/forms';
import { NzFormModule } from 'ng-zorro-antd/form';
import { FormModalModule } from 'src/modules/forms/form-modal/form-modal.module';
import { NzTypographyModule } from 'ng-zorro-antd/typography';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzStatisticModule } from 'ng-zorro-antd/statistic';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { InvoicesPageComponent } from './invoices-page.component';
import { CreateInvoiceModalContent } from './create-invoice-modal-content/create-invoice-modal-content';
import { PageModule } from '../../../modules/page/page.module';
import { TableWidgetModule } from '../../../modules/table-widget/table-widget.module';
import { InvoicesTableWidgetModule } from '../../../modules/invoice/invoices-table-widget/invoices-table-widget.module';
import { InputFieldModule } from '../../../modules/forms/fields/input-field/input-field.module';
import { SelectFieldModule } from '../../../modules/forms/fields/select-field/select-field.module';
import { DatePickerFieldModule } from '../../../modules/forms/fields/date-picker-field/date-picker-field.module';

@NgModule({
  declarations: [InvoicesPageComponent, CreateInvoiceModalContent],
  imports: [
    CommonModule,
    InputFieldModule,
    FormModalModule,
    SelectFieldModule,
    DatePickerFieldModule,
    ReactiveFormsModule,
    SelectFieldModule,
    NzButtonModule,
    NzFormModule,
    NzTypographyModule,
    NzDividerModule,
    NzStatisticModule,
    NzSpaceModule,
    PageModule,
    FormModalModule,
    TableWidgetModule,
    InvoicesTableWidgetModule,
  ],
  exports: [InvoicesPageComponent],
})
export class InvoicesPageModule {}
