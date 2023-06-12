import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RadioFieldComponent } from './radio-field.component';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { FieldErrorModule } from '../field-error/field-error.module';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzRadioModule } from 'ng-zorro-antd/radio';
import { FormsModule } from '@angular/forms';
import { NzSpaceModule } from 'ng-zorro-antd/space';

@NgModule({
  declarations: [RadioFieldComponent],
  imports: [CommonModule, NzGridModule, FieldErrorModule, NzFormModule, NzRadioModule, FormsModule, NzSpaceModule],
  exports: [RadioFieldComponent],
})
export class RadioFieldModule {}
