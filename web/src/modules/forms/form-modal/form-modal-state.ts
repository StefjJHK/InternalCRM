import { BipFormGroup } from '../form-controls/bip-form-group';
import { FormState } from './form-modal-content/form-state';
import { BehaviorSubject } from 'rxjs';

export interface FormModalState {
  formGroup: BipFormGroup;
  formState$: BehaviorSubject<FormState>;
}
