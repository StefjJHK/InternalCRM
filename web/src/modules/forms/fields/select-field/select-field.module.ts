import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { SelectFieldComponent } from './select-field.component';
import { FormsModule } from '@angular/forms';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzFormModule } from 'ng-zorro-antd/form';
import { FieldErrorModule } from '../field-error/field-error.module';

@NgModule({
  declarations: [SelectFieldComponent],
  imports: [CommonModule, NzSelectModule, FormsModule, NzDividerModule, NzIconModule, NzInputModule, NzFormModule, FieldErrorModule],
  exports: [SelectFieldComponent],
})
export class SelectFieldModule {}
