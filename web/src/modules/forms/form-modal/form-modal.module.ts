import { NgModule } from '@angular/core';
import { ModalModule } from '../../modal/modal.module';
import { FormModalContentComponent } from './form-modal-content/form-modal-content.component';
import { LoaderModule } from '../../loader/loader.module';
import { CommonModule } from '@angular/common';
import { NzResultModule } from 'ng-zorro-antd/result';

@NgModule({
  declarations: [FormModalContentComponent],
  imports: [CommonModule, ModalModule, LoaderModule, NzResultModule],
  exports: [FormModalContentComponent],
})
export class FormModalModule {}
