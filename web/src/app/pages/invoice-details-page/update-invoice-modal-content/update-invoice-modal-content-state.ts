import { FormModalState } from '../../../../modules/forms/form-modal/form-modal-state';
import { Invoice } from '../../../../modules/invoice/invoice.model';

export interface UpdateInvoiceModalContentState extends FormModalState {
  invoice: Invoice;
}
