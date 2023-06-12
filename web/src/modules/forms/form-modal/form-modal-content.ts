import { ModalContent } from '../../modal/modal-content';
import { FormModalState } from './form-modal-state';
import { Observable } from 'rxjs';

export abstract class FormModalContent<TFormModalState extends FormModalState = FormModalState> extends ModalContent<TFormModalState> {
  abstract onSubmit(): Observable<void>;
}
