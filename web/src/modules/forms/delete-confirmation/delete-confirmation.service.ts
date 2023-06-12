import { Injectable } from '@angular/core';
import { ModalService } from '../../modal/modal.service';
import { BipFormGroup } from '../form-controls/bip-form-group';
import { FormState } from '../form-modal/form-modal-content/form-state';
import { BehaviorSubject, Observable } from 'rxjs';
import { BipFormControl } from '../form-controls/bip-form-control';
import { BipValidators } from '../bip-validators';
import { DeleteConfirmationContentComponent } from './delete-confirmation-content/delete-confirmation-content.component';
import { DeleteConfirmationContentState } from './delete-confirmation-content/delete-confirmation-content-state';
import { errorHandler, handleErrors } from '../../operators/handleErrors';

@Injectable({
  providedIn: 'root',
})
export class DeleteConfirmationService {
  constructor(private readonly modalService: ModalService) {}

  confirmDelete(title: string, confirmationValue: string, delete$: Observable<void>, onDelete: () => void): void {
    let formState: FormState = FormState.Filling;

    const formGroup = new BipFormGroup({
      confirmationValue: new BipFormControl<string>('', `Enter ${confirmationValue}`, {
        validators: BipValidators.mustBe(confirmationValue),
      }),
    });
    const formState$: BehaviorSubject<FormState> = new BehaviorSubject<FormState>(formState);

    const onConfirm = () => {
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

            onDelete();
          });
      }
    };

    const modal = this.modalService.confirm<DeleteConfirmationContentComponent, DeleteConfirmationContentState>(
      title,
      DeleteConfirmationContentComponent,
      { formGroup, formState$, confirmationValue, delete$, title },
      onConfirm,
    );

    formState$.subscribe((state) => {
      formState = state;

      if (formState == FormState.Submit) {
        modal.close();
      }
    });
  }
}
