import { Payment } from '../../payment.model';
import { FormModalState } from '../../../forms/form-modal/form-modal-state';

export interface UpdatePaymentModalContentState extends FormModalState {
  payment: Payment;
}
