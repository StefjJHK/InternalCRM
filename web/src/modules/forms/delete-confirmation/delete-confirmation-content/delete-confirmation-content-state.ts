import { Observable } from 'rxjs';
import { FormModalState } from '../../form-modal/form-modal-state';

export interface DeleteConfirmationContentState extends FormModalState {
  title: string;
  confirmationValue: string;
  delete$: Observable<void>;
}
