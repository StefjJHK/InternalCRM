import { FormModalState } from '../../../../modules/forms/form-modal/form-modal-state';
import { Lead } from '../../../../modules/lead/lead.model';

export interface UpdateLeadModalContentState extends FormModalState {
  lead: Lead;
}
