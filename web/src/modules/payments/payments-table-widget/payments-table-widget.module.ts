import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaymentsTableWidgetComponent } from './payments-table-widget.component';
import { TableWidgetModule } from '../../table-widget/table-widget.module';
import { NzTabsModule } from 'ng-zorro-antd/tabs';
import { NzTableModule } from 'ng-zorro-antd/table';
import { ButtonModule } from '../../common-components/button/button.module';
import { UpdatePaymentModalContentComponent } from './update-payment-modal-content/update-payment-modal-content.component';
import { FormModalModule } from '../../forms/form-modal/form-modal.module';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { InputFieldModule } from '../../forms/fields/input-field/input-field.module';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { DatePickerFieldModule } from '../../forms/fields/date-picker-field/date-picker-field.module';
import { NzFormModule } from 'ng-zorro-antd/form';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [PaymentsTableWidgetComponent, UpdatePaymentModalContentComponent],
  imports: [
    CommonModule,
    TableWidgetModule,
    NzTabsModule,
    NzTableModule,
    ButtonModule,
    FormModalModule,
    NzSpaceModule,
    InputFieldModule,
    NzGridModule,
    DatePickerFieldModule,
    NzFormModule,
    ReactiveFormsModule,
  ],
  exports: [PaymentsTableWidgetComponent],
})
export class PaymentsTableWidgetModule {}
