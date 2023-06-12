import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { ReactiveFormsModule } from '@angular/forms';
import { NzFormModule } from 'ng-zorro-antd/form';
import { FormModalModule } from 'src/modules/forms/form-modal/form-modal.module';
import { NzTypographyModule } from 'ng-zorro-antd/typography';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzStatisticModule } from 'ng-zorro-antd/statistic';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { PurchaseOrdersPageComponent } from './purchase-orders-page.component';
import { CreatePurchaseOrderModalContent } from './create-purchase-order-modal-content/create-purchase-order-modal-content';
import { PageModule } from '../../../modules/page/page.module';
import { PurchaseOrdersTableWidgetModule } from '../../../modules/purchase-order/purchase-orders-table-widget/purchase-orders-table-widget.module';
import { InputFieldModule } from '../../../modules/forms/fields/input-field/input-field.module';
import { SelectFieldModule } from '../../../modules/forms/fields/select-field/select-field.module';
import { DatePickerFieldModule } from '../../../modules/forms/fields/date-picker-field/date-picker-field.module';

@NgModule({
  declarations: [PurchaseOrdersPageComponent, CreatePurchaseOrderModalContent],
  imports: [
    CommonModule,
    InputFieldModule,
    FormModalModule,
    SelectFieldModule,
    DatePickerFieldModule,
    ReactiveFormsModule,
    SelectFieldModule,
    NzButtonModule,
    NzFormModule,
    NzTypographyModule,
    NzDividerModule,
    NzStatisticModule,
    NzSpaceModule,
    PageModule,
    FormModalModule,
    PurchaseOrdersTableWidgetModule,
  ],
  exports: [PurchaseOrdersPageComponent],
})
export class PurchaseOrdersPageModule {}
