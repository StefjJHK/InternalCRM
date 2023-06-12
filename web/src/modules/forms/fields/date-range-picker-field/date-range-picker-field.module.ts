import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DateRangePickerFieldComponent } from './date-range-picker-field.component';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzFormModule } from 'ng-zorro-antd/form';
import { FormsModule } from '@angular/forms';
import { FieldErrorModule } from '../field-error/field-error.module';

@NgModule({
  declarations: [DateRangePickerFieldComponent],
  imports: [CommonModule, NzFormModule, FormsModule, NzDatePickerModule, FieldErrorModule],
  exports: [DateRangePickerFieldComponent],
})
export class DateRangePickerFieldModule {}
