import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PurchaseOrderDetailsPageComponent } from './purchase-order-details-page.component';
import { PageModule } from '../../../modules/page/page.module';
import { TabsetModule } from '../../../modules/common-components/tabset/tabset.module';
import { NzDescriptionsModule } from 'ng-zorro-antd/descriptions';
import { InvoicesTableWidgetModule } from '../../../modules/invoice/invoices-table-widget/invoices-table-widget.module';

@NgModule({
  declarations: [PurchaseOrderDetailsPageComponent],
  imports: [CommonModule, PageModule, TabsetModule, NzDescriptionsModule, InvoicesTableWidgetModule],
  exports: [PurchaseOrderDetailsPageComponent],
})
export class PurchaseOrderDetailsPageModule {}
