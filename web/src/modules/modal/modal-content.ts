import { inject } from '@angular/core';
import { NZ_MODAL_DATA } from 'ng-zorro-antd/modal';

export class ModalContent<TState> {
  readonly state: TState = inject(NZ_MODAL_DATA);
}
