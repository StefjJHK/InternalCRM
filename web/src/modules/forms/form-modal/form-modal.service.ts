import { Injectable } from '@angular/core';
import { ModalService } from '../../modal/modal.service';
import { ModalActionRoles } from '../../modal/modal-action-roles';
import { BipFormGroup } from '../form-controls/bip-form-group';
import { FormState } from './form-modal-content/form-state';
import { BehaviorSubject } from 'rxjs';
import { FormModalContent } from './form-modal-content';
import { FormModalState } from './form-modal-state';
import { errorHandler, handleErrors } from '../../operators/handleErrors';

@Injectable({
  providedIn: 'root',
})
export class FormModalService {
  constructor(private modalService: ModalService) {}

  show<TFormModalContent extends FormModalContent<TFormModalState>, TFormModalState extends FormModalState>(
    title: string,
    formContent: { new (...args: any[]): TFormModalContent },
    formGroup: BipFormGroup,
    onSubmit?: () => void,
    submitEnabled?: (formState: FormState) => boolean,
    state?: Omit<TFormModalState, 'formGroup' | 'formState$'>,
  ): void {
    let formState: FormState = FormState.Filling;

    const formState$: BehaviorSubject<FormState> = new BehaviorSubject<FormState>(formState);

    const onSubmitWrapper = (): void => {
      if (onSubmit) {
        onSubmit();
      }
    };

    const modal = this.modalService.show<TFormModalContent, TFormModalState>(
      title,
      formContent,
      { ...state, formGroup, formState$ } as TFormModalState,
      [
        {
          id: 'cancel',
          title: 'Cancel',
          type: 'default',
          role: ModalActionRoles.Cancel,

          onClick() {},
        },
        {
          id: 'submit',
          title: 'Submit',
          type: 'primary',
          role: ModalActionRoles.Confirm,

          onClick() {
            formGroup.markAllAsTouched();

            if (formGroup.valid) {
              formState$.next(FormState.Submitting);
              modal
                .getContentComponent()
                .onSubmit()
                .pipe(
                  handleErrors([
                    errorHandler(() => {
                      formState$.next(FormState.Filling);
                    }),
                  ]),
                )
                .subscribe(() => {
                  formState$.next(FormState.Submit);

                  onSubmitWrapper();
                });
            }
          },
          disabled: () => {
            if (submitEnabled) {
              return !submitEnabled(formState);
            }

            return formState == FormState.Loading || formState == FormState.Submitting;
          },
        },
      ],
    );

    formState$.subscribe((state) => {
      formState = state;

      if (formState == FormState.Submit) {
        modal.close();
      }
    });
  }
}
