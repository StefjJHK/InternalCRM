import { Injectable } from '@angular/core';
import { ModalButtonOptions, NzModalRef, NzModalService } from 'ng-zorro-antd/modal';
import { ModalAction } from './modal-action';
import { ModalActionRoles } from './modal-action-roles';
import { ModalContent } from './modal-content';

@Injectable({
  providedIn: 'root',
})
export class ModalService {
  constructor(private modalService: NzModalService) {}

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  show<TModalContent extends ModalContent<TModalState>, TModalState>(
    title?: string,
    modalContent?: { new (...args: any[]): TModalContent },
    state?: TModalState,
    actions?: ModalAction[],
  ): NzModalRef<TModalContent> {
    const modal = this.modalService.create<TModalContent>({
      nzTitle: title,
      nzContent: modalContent,
      nzClosable: false,
      nzMaskClosable: false,
      nzCentered: true,
      nzData: state,
      nzFooter:
        actions?.map(
          (action): ModalButtonOptions => ({
            id: action.id,
            label: action.title,
            type: action.type,
            disabled: action.disabled,
            onClick() {
              action.onClick();

              if (action.role === ModalActionRoles.Cancel) {
                modal.destroy();
              }
            },
          }),
        ) ?? [],
    });

    return modal;
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  confirm<TModalContent extends ModalContent<TModalState>, TModalState>(
    title?: string,
    modalContent?: { new (...args: any[]): TModalContent },
    state?: TModalState,
    onConfirm?: () => void,
  ): NzModalRef<TModalContent> {
    const modal = this.modalService.confirm<TModalContent>({
      nzTitle: title,
      nzContent: modalContent,
      nzClosable: false,
      nzMaskClosable: false,
      nzCentered: true,
      nzData: state,
      nzOkText: 'Yes',
      nzOkType: 'primary',
      nzOkDanger: true,
      nzOnOk: () => {
        if (onConfirm) {
          onConfirm();
        }

        return false;
      },
      nzCancelText: 'No',
    });

    return modal;
  }
}
