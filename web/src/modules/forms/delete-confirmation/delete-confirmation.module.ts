import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DeleteConfirmationContentComponent } from './delete-confirmation-content/delete-confirmation-content.component';
import { FormModalModule } from '../form-modal/form-modal.module';
import { NzFormModule } from 'ng-zorro-antd/form';
import { ReactiveFormsModule } from '@angular/forms';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { InputFieldModule } from '../fields/input-field/input-field.module';
import { NzTypographyModule } from 'ng-zorro-antd/typography';
import { NzIconModule } from 'ng-zorro-antd/icon';

@NgModule({
  declarations: [DeleteConfirmationContentComponent],
  imports: [
    CommonModule,
    FormModalModule,
    NzFormModule,
    ReactiveFormsModule,
    NzSpaceModule,
    InputFieldModule,
    NzTypographyModule,
    NzIconModule,
  ],
  exports: [DeleteConfirmationContentComponent],
})
export class DeleteConfirmationModule {}
