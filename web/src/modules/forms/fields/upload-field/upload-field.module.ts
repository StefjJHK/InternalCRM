import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzUploadModule } from 'ng-zorro-antd/upload';
import { UploadFieldComponent } from './upload-field.component';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzTypographyModule } from 'ng-zorro-antd/typography';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { FieldErrorModule } from '../field-error/field-error.module';

@NgModule({
  declarations: [UploadFieldComponent],
  imports: [CommonModule, NzUploadModule, NzFormModule, NzIconModule, NzButtonModule, NzTypographyModule, NzSpaceModule, FieldErrorModule],
  exports: [UploadFieldComponent],
})
export class UploadFieldModule {}
