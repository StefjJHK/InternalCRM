import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FieldErrorComponent } from './field-error.component';
import { NzSpaceModule } from 'ng-zorro-antd/space';

@NgModule({
  declarations: [FieldErrorComponent],
  imports: [CommonModule, NzSpaceModule],
  exports: [FieldErrorComponent],
})
export class FieldErrorModule {}
