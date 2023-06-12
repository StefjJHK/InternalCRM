import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InputFieldComponent } from './input-field.component';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { ReactiveFormsModule } from '@angular/forms';
import { NzTypographyModule } from 'ng-zorro-antd/typography';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { FieldErrorModule } from '../field-error/field-error.module';
import { NzIconModule } from 'ng-zorro-antd/icon';

@NgModule({
  declarations: [InputFieldComponent],
  imports: [
    CommonModule,
    NzFormModule,
    NzInputModule,
    ReactiveFormsModule,
    NzTypographyModule,
    NzSpaceModule,
    FieldErrorModule,
    NzIconModule,
  ],
  exports: [InputFieldComponent],
})
export class InputFieldModule {}
