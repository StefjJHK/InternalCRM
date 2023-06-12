import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormModalService } from '../../../modules/forms/form-modal/form-modal.service';
import { CreateProductModalContentComponent } from './create-product-modal-content/create-product-modal-content.component';
import { BipFormGroup } from '../../../modules/forms/form-controls/bip-form-group';
import { BipFormControl } from '../../../modules/forms/form-controls/bip-form-control';
import { BipValidators } from '../../../modules/forms/bip-validators';
import { ProductService } from '../../../modules/product/product.service';
import { combineLatest, map } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { ProductCard } from './product-card.model';
import { NotificationService } from '../../../modules/notification/notification.service';
import { NotificationType } from '../../../modules/notification/notification-type';
import { StatisticsService } from '../../../modules/statistics/statistics.service';
import { Button } from '../../../modules/common-components/button/button.model';
import { errorHandler, handleErrors } from '../../../modules/operators/handleErrors';
import { Product } from '../../../modules/product/product.model';
import { ProductStatistics } from '../../../modules/statistics/product-statistics.model';
import { PageState } from '../../../modules/page/page-state';
import { Router } from '@angular/router';
import { PermissionsService } from '../../../modules/permissions/permissions.service';

@UntilDestroy()
@Component({
  selector: 'bip-products-page',
  templateUrl: './products-page.component.html',
  styleUrls: ['./products-page.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProductsPageComponent implements OnInit {
  headerButtons: Button[] = [];

  pageState: PageState = PageState.loaded;

  productCards: ProductCard[] = [];

  constructor(
    private readonly formModalService: FormModalService,
    private readonly productService: ProductService,
    private readonly statisticsService: StatisticsService,
    private readonly notificationService: NotificationService,
    private readonly router: Router,
    private readonly permissionsService: PermissionsService,
    private readonly changeDetectorRef: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    if (this.permissionsService.containsPermissions({ product: { canWrite: true } })) {
      this.headerButtons = [
        ...this.headerButtons,
        {
          text: 'Add new',
          type: 'primary',
          icon: 'plus',

          onClick: () => this.onCreateProductClick(),
        },
      ];
    }

    this.fetchProductsCard();
  }

  onProductDetailsClick(productName: string) {
    this.router.navigate(['products', productName]);
  }

  fetchProductsCard() {
    this.pageState = PageState.loading;

    combineLatest([this.productService.getProducts({}), this.statisticsService.getProductStatistics()])
      .pipe(
        untilDestroyed(this),
        handleErrors([
          errorHandler(() => {
            this.notificationService.notify(NotificationType.Error, 'Error', 'Unknown error has occurred while loading statistics');

            this.pageState = PageState.error;

            this.changeDetectorRef.markForCheck();
          }),
        ]),
        map((group): ProductCard[] =>
          group[0].map((product: Product) => {
            const statistics = group[1].find((statistics: ProductStatistics) => statistics.productName === product.name)!;

            return {
              productName: product.name,
              title: product.name,
              iconUri: product.iconUri,
              totalCustomers: statistics.totalCustomers,
              totalSubscriptions: statistics.totalSubscriptions,
            };
          }),
        ),
      )
      .subscribe((productCards) => {
        this.productCards = productCards;
        this.pageState = PageState.loaded;

        this.changeDetectorRef.markForCheck();
      });
  }

  private onCreateProductClick() {
    const formGroup: BipFormGroup = new BipFormGroup({
      name: new BipFormControl<string>('', 'Name', { validators: [BipValidators.required()] }),
      project: new BipFormControl<File | null>(null, 'IntelliLock Project'),
      icon: new BipFormControl<File | null>(null, 'Image (icon)'),
    });

    const onSubmit = () => {
      this.notificationService.notify(NotificationType.Success, 'Success', `The product ${formGroup.bipControls.name.value} was created`);

      this.changeDetectorRef.markForCheck();

      this.fetchProductsCard();
    };

    this.formModalService.show('Create product', CreateProductModalContentComponent, formGroup, onSubmit);
  }
}
