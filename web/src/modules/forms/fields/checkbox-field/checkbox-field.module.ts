import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { CheckboxFieldComponent } from './checkbox-field.component';
import { NzFormModule } from 'ng-zorro-antd/form';
import { FieldErrorModule } from '../field-error/field-error.module';

@NgModule({
  declarations: [CheckboxFieldComponent],
  imports: [CommonModule, NzCheckboxModule, NzFormModule, FieldErrorModule],
  exports: [CheckboxFieldComponent],
})
export class CheckboxFieldModule {}
