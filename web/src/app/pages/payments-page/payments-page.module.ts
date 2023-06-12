import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { ReactiveFormsModule } from '@angular/forms';
import { NzFormModule } from 'ng-zorro-antd/form';
import { FormModalModule } from 'src/modules/forms/form-modal/form-modal.module';
import { NzTypographyModule } from 'ng-zorro-antd/typography';
import { NzPageHeaderModule } from 'ng-zorro-antd/page-header';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzStatisticModule } from 'ng-zorro-antd/statistic';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { PaymentsPageComponent } from './payments-page.component';
import { CreatePaymentModalContent } from './create-payment-modal-content/create-payment-modal-content';
import { PageModule } from '../../../modules/page/page.module';
import { PaymentsTableWidgetModule } from '../../../modules/payments/payments-table-widget/payments-table-widget.module';
import { InputFieldModule } from '../../../modules/forms/fields/input-field/input-field.module';
import { SelectFieldModule } from '../../../modules/forms/fields/select-field/select-field.module';
import { DatePickerFieldModule } from '../../../modules/forms/fields/date-picker-field/date-picker-field.module';

@NgModule({
  declarations: [PaymentsPageComponent, CreatePaymentModalContent],
  imports: [
    CommonModule,
    InputFieldModule,
    FormModalModule,
    SelectFieldModule,
    DatePickerFieldModule,
    ReactiveFormsModule,
    SelectFieldModule,
    NzButtonModule,
    NzPageHeaderModule,
    NzIconModule,
    NzFormModule,
    NzTypographyModule,
    NzStatisticModule,
    NzSpaceModule,
    PageModule,
    FormModalModule,
    PaymentsTableWidgetModule,
  ],
  exports: [PaymentsPageComponent],
})
export class PaymentsPageModule {}
