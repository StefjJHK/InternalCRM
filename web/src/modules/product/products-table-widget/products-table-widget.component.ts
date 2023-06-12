import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { errorHandler, handleErrors } from '../../operators/handleErrors';
import { WidgetState } from '../../widget/widget-state';
import { Product } from '../product.model';
import { ProductService } from '../product.service';

@UntilDestroy()
@Component({
  selector: 'bip-products-table-widget',
  templateUrl: './products-table-widget.component.html',
  styleUrls: ['./products-table-widget.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProductsTableWidgetComponent implements OnInit {
  @Input() customerName?: string;

  widgetState: WidgetState = WidgetState.loaded;

  products?: Product[];

  constructor(private readonly productService: ProductService, private readonly changeDetectorRef: ChangeDetectorRef) {}

  ngOnInit() {
    this.fetchProducts();
  }

  fetchProducts() {
    this.widgetState = WidgetState.loading;

    this.productService
      .getProducts({ customer: this.customerName })
      .pipe(
        untilDestroyed(this),
        handleErrors([
          errorHandler(() => {
            this.widgetState = WidgetState.error;

            this.changeDetectorRef.markForCheck();
          }),
        ]),
      )
      .subscribe((products: Product[]) => {
        this.products = products;
        this.widgetState = WidgetState.loaded;

        this.changeDetectorRef.markForCheck();
      });
  }
}
