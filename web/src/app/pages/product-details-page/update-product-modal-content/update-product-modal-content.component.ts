import { ChangeDetectionStrategy, Component } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { errorHandler, handleErrors } from '../../../../modules/operators/handleErrors';
import { NotificationType } from '../../../../modules/notification/notification-type';
import { ProductService } from '../../../../modules/product/product.service';
import { NotificationService } from '../../../../modules/notification/notification.service';
import { Observable } from 'rxjs';
import { FormModalContent } from '../../../../modules/forms/form-modal/form-modal-content';
import { UpdateProductRequest } from '../../../../modules/product/update-product-request.model';
import { UpdateProductModalContentState } from './update-product-modal-content-state';

@UntilDestroy()
@Component({
  selector: 'bip-update-subscription-modal-content',
  templateUrl: './update-product-modal-content.component.html',
  styleUrls: ['./update-product-modal-content.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UpdateProductModalContentComponent extends FormModalContent<UpdateProductModalContentState> {
  constructor(private readonly productService: ProductService, private readonly notificationService: NotificationService) {
    super();
  }

  onSubmit(): Observable<void> {
    const updateProductRequest: UpdateProductRequest = {
      name: this.state.formGroup.bipControls.name.value,
      icon: this.state.formGroup.bipControls.icon.value,
      ilProject: this.state.formGroup.bipControls.project.value,
    };

    return this.productService.updateProduct(this.state.product.name, updateProductRequest).pipe(
      untilDestroyed(this),
      handleErrors([
        errorHandler(() => {
          this.notificationService.notify(
            NotificationType.Error,
            'An error has occurred',
            `The product ${this.state.product.name} was not updated`,
          );
        }),
      ]),
    );
  }
}
