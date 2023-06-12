import { FormModalState } from '../../../../modules/forms/form-modal/form-modal-state';
import { Product } from '../../../../modules/product/product.model';

export interface UpdateProductModalContentState extends FormModalState {
  product: Product;
}
