import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { DatePickerFieldComponent } from './date-picker-field.component';
import { FormsModule } from '@angular/forms';
import { NzFormModule } from 'ng-zorro-antd/form';
import { FieldErrorModule } from '../field-error/field-error.module';

@NgModule({
  declarations: [DatePickerFieldComponent],
  imports: [CommonModule, NzDatePickerModule, FormsModule, NzFormModule, FieldErrorModule],
  exports: [DatePickerFieldComponent],
})
export class DatePickerFieldModule {}
