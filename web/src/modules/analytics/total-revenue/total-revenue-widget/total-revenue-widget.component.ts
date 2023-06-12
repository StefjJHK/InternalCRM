import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { TotalRevenueService } from '../total-revenue.service';
import { WidgetState } from '../../../widget/widget-state';
import { TotalRevenueRequest } from '../total-revenue-request.model';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { errorHandler, handleErrors } from '../../../operators/handleErrors';
import { TotalRevenue } from '../total-revenue.model';
import { ProductService } from '../../../product/product.service';
import { Product } from '../../../product/product.model';
import { BipFormGroup } from '../../../forms/form-controls/bip-form-group';
import { BipFormControl } from '../../../forms/form-controls/bip-form-control';
import { SelectFieldOption } from '../../../forms/fields/select-field/select-field-option';
import { BipValidators } from '../../../forms/bip-validators';

@UntilDestroy()
@Component({
  selector: 'bip-total-revenue-widget',
  templateUrl: './total-revenue-widget.component.html',
  styleUrls: ['./total-revenue-widget.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TotalRevenueWidgetComponent implements OnInit {
  chartLegend?: string[] = [];

  chartSeries?: number[][] = [];

  widgetState: WidgetState = WidgetState.loaded;

  productsSelectFieldOptions: SelectFieldOption[] = [];

  quarterSelectFieldOptions: SelectFieldOption[] = [
    { value: 'all', label: ' All' },
    { value: 'Q1', label: 'Q1' },
    { value: 'Q2', label: 'Q2' },
    { value: 'Q3', label: 'Q3' },
    { value: 'Q4', label: 'Q4' },
  ];

  formGroup: BipFormGroup = new BipFormGroup({
    product: new BipFormControl<string>('', 'Product', { validators: [BipValidators.required()] }),
    date: new BipFormControl<Date>(new Date(), 'Year', { validators: [BipValidators.required()] }),
    quarter: new BipFormControl<string>(this.quarterSelectFieldOptions[0].value.toString(), 'Quarter', {
      validators: [BipValidators.required()],
    }),
  });

  constructor(
    private readonly totalRevenueService: TotalRevenueService,
    private readonly productService: ProductService,
    private readonly changeDetectorRef: ChangeDetectorRef,
  ) {}

  ngOnInit() {
    this.fetchProducts();
    this.fetchTotalRevenue();

    this.formGroup.valueChanges.subscribe(() => {
      if (this.formGroup.valid) {
        this.fetchTotalRevenue();
      }
    });
  }

  fetchProducts() {
    this.widgetState = WidgetState.loading;

    this.productService
      .getProducts({})
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
        this.productsSelectFieldOptions = [
          {
            value: '',
            label: 'All products',
          },
          ...products.map((product) => ({
            label: product.name,
            value: product.name,
          })),
        ];
        this.widgetState = WidgetState.loaded;

        this.changeDetectorRef.markForCheck();
      });
  }

  fetchTotalRevenue() {
    this.widgetState = WidgetState.loading;

    const request: TotalRevenueRequest = {
      quarter: this.formGroup.bipControls.quarter.value,
      year: this.formGroup.bipControls.date.value.getFullYear(),
      productName: this.formGroup.bipControls.product.value,
    };

    this.totalRevenueService
      .getTotalRevenue(request)
      .pipe(
        untilDestroyed(this),
        handleErrors([
          errorHandler(() => {
            this.widgetState = WidgetState.error;

            this.changeDetectorRef.markForCheck();
          }),
        ]),
      )
      .subscribe((totalRevenue: TotalRevenue) => {
        this.chartLegend = totalRevenue.revenue.chartSeries.map((x) => x.label);
        this.chartSeries = [totalRevenue.revenue.chartSeries.map((x) => x.value)];

        this.widgetState = WidgetState.loaded;

        this.changeDetectorRef.markForCheck();
      });
  }
}
