import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Product } from '../../../modules/product/product.model';
import { ProductService } from '../../../modules/product/product.service';
import { switchMap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { NotificationService } from '../../../modules/notification/notification.service';
import { errorHandler, handleErrors } from '../../../modules/operators/handleErrors';
import { NotificationType } from '../../../modules/notification/notification-type';
import { PageState } from '../../../modules/page/page-state';
import { Button } from '../../../modules/common-components/button/button.model';
import { DeleteConfirmationService } from '../../../modules/forms/delete-confirmation/delete-confirmation.service';
import { AppRoutes } from '../../routing/app-routes';
import { PermissionsService } from '../../../modules/permissions/permissions.service';
import { BipFormGroup } from '../../../modules/forms/form-controls/bip-form-group';
import { BipFormControl } from '../../../modules/forms/form-controls/bip-form-control';
import { BipValidators } from '../../../modules/forms/bip-validators';
import { FormModalService } from '../../../modules/forms/form-modal/form-modal.service';
import { UpdateProductModalContentComponent } from './update-product-modal-content/update-product-modal-content.component';
import { UpdateProductModalContentState } from './update-product-modal-content/update-product-modal-content-state';
import { FormState } from '../../../modules/forms/form-modal/form-modal-content/form-state';

@UntilDestroy()
@Component({
  selector: 'bip-product-details-page',
  templateUrl: './product-details-page.component.html',
  styleUrls: ['./product-details-page.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProductDetailsPageComponent implements OnInit {
  headerButtons: Button[] = [];

  pageState: PageState = PageState.loaded;

  product?: Product;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly productService: ProductService,
    private readonly deleteConfirmationService: DeleteConfirmationService,
    private readonly notificationService: NotificationService,
    private readonly formModalService: FormModalService,
    private readonly router: Router,
    private readonly permissionsService: PermissionsService,
    private readonly changeDetectorRef: ChangeDetectorRef,
  ) {}

  ngOnInit() {
    if (this.permissionsService.containsPermissions({ product: { canWrite: true } })) {
      this.headerButtons = [
        ...this.headerButtons,
        {
          type: 'default',
          text: 'Edit',

          onClick: () => this.onUpdateProduct(),
        },
        {
          type: 'danger',
          text: 'Delete',

          onClick: () => this.onDeleteProduct(),
        },
      ];
    }

    this.fetchProductDetails();
  }

  fetchProductDetails() {
    this.pageState = PageState.loading;

    this.route.params
      .pipe(
        untilDestroyed(this),
        switchMap((params) => this.productService.getProduct(params.productName)),
        handleErrors([
          errorHandler(() => {
            this.notificationService.notify(NotificationType.Error, 'Error', 'Unknown error has occurred while loading product'),
              (this.pageState = PageState.error);
          }),
        ]),
      )
      .subscribe((product) => {
        this.product = product;
        this.pageState = PageState.loaded;

        this.changeDetectorRef.markForCheck();
      });
  }

  private onUpdateProduct() {
    const formGroup: BipFormGroup = new BipFormGroup({
      name: new BipFormControl<string>(this.product!.name, 'Name', { validators: [BipValidators.required()] }),
      project: new BipFormControl<File | null>(null, 'IntelliLock Project'),
      icon: new BipFormControl<File | null>(null, 'Image (icon)'),
    });

    const onSubmit = () => {
      this.notificationService.notify(NotificationType.Success, 'Success', `The product ${formGroup.bipControls.name.value} was updated`);
      this.router.navigate([AppRoutes.Products, formGroup.bipControls.name.value]);
    };
    const submitEnabled = (formState: FormState) => {
      if (formState == FormState.Loading || formState == FormState.Submitting) {
        return false;
      }

      return this.product!.name != formGroup.bipControls.name.value;
    };

    this.formModalService.show<UpdateProductModalContentComponent, UpdateProductModalContentState>(
      'Update product',
      UpdateProductModalContentComponent,
      formGroup,
      onSubmit,
      submitEnabled,
      {
        product: this.product!,
      },
    );
  }

  private onDeleteProduct() {
    const productName = this.product!.name;

    const onDelete = (): void => {
      this.router.navigate([AppRoutes.Products]);
      this.notificationService.notify(NotificationType.Success, 'Success', `Product ${productName} was deleted`);
    };

    this.deleteConfirmationService.confirmDelete(
      'Are you sure to delete product?',
      productName,
      this.productService.deleteProduct(productName),
      onDelete,
    );
  }
}
