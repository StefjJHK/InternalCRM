import { NgModule } from '@angular/core';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { ModalContentComponent } from './modal-content/modal-content.component';

@NgModule({
  declarations: [ModalContentComponent],
  imports: [NzModalModule],
  exports: [ModalContentComponent],
})
export class ModalModule {}
