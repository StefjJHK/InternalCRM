import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { AddProductRequest } from '../../../../modules/product/add-product-request.model';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { errorHandler, handleErrors } from '../../../../modules/operators/handleErrors';
import { NotificationType } from '../../../../modules/notification/notification-type';
import { ProductService } from '../../../../modules/product/product.service';
import { NotificationService } from '../../../../modules/notification/notification.service';
import { Observable } from 'rxjs';
import { FormModalContent } from '../../../../modules/forms/form-modal/form-modal-content';

@UntilDestroy()
@Component({
  selector: 'bip-create-subscription-modal-content',
  templateUrl: './create-product-modal-content.component.html',
  styleUrls: ['./create-product-modal-content.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CreateProductModalContentComponent extends FormModalContent {
  constructor(private readonly productService: ProductService, private readonly notificationService: NotificationService) {
    super();
  }

  onSubmit(): Observable<void> {
    const addProductRequest: AddProductRequest = {
      name: this.state.formGroup.bipControls.name.value,
      icon: this.state.formGroup.bipControls.icon.value,
      ilProject: this.state.formGroup.bipControls.project.value,
    };

    return this.productService.addProduct(addProductRequest).pipe(
      untilDestroyed(this),
      handleErrors([
        errorHandler(() => {
          this.notificationService.notify(NotificationType.Error, 'An error has occurred', 'The product was not created');
        }),
      ]),
    );
  }
}
