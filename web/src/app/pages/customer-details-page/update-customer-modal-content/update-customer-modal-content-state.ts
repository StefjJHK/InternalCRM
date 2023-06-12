import { FormModalState } from '../../../../modules/forms/form-modal/form-modal-state';
import { Customer } from '../../../../modules/customer/customer.model';

export interface UpdateCustomerModalContentState extends FormModalState {
  customer: Customer;
}
