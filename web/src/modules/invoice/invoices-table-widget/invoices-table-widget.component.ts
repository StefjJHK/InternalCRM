import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { InvoiceService } from '../invoice.service';
import { Invoice } from '../invoice.model';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { errorHandler, handleErrors } from '../../operators/handleErrors';
import { WidgetState } from '../../widget/widget-state';

@UntilDestroy()
@Component({
  selector: 'bip-invoices-table-widget',
  templateUrl: './invoices-table-widget.component.html',
  styleUrls: ['./invoices-table-widget.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class InvoicesTableWidgetComponent implements OnInit {
  @Input() productName?: string;

  @Input() customerName?: string;

  @Input() purchaseOrderName?: string;

  widgetState: WidgetState = WidgetState.loaded;

  invoices: Invoice[] = [];

  constructor(private readonly invoiceService: InvoiceService, private readonly changeDetectorRef: ChangeDetectorRef) {}

  ngOnInit() {
    this.fetchInvoices();
  }

  fetchInvoices() {
    this.widgetState = WidgetState.loading;

    this.invoiceService
      .getInvoices({ product: this.productName, customer: this.customerName, purchaseOrder: this.purchaseOrderName })
      .pipe(
        untilDestroyed(this),
        handleErrors([
          errorHandler(() => {
            this.widgetState = WidgetState.error;

            this.changeDetectorRef.markForCheck();
          }),
        ]),
      )
      .subscribe((invoices: Invoice[]) => {
        this.invoices = invoices;
        this.widgetState = WidgetState.loaded;

        this.changeDetectorRef.markForCheck();
      });
  }
}
