import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateCustomerModalContentComponent } from './create-customer-modal-content/create-customer-modal-content.component';
import { ReactiveFormsModule } from '@angular/forms';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzTypographyModule } from 'ng-zorro-antd/typography';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { NzStatisticModule } from 'ng-zorro-antd/statistic';
import { CustomersPageComponent } from './customers-page.component';
import { PageModule } from '../../../modules/page/page.module';
import { FormModalModule } from '../../../modules/forms/form-modal/form-modal.module';
import { CustomersTableWidgetModule } from '../../../modules/customer/customers-table-widget/customers-table-widget.module';
import { InputFieldModule } from '../../../modules/forms/fields/input-field/input-field.module';

@NgModule({
  declarations: [CreateCustomerModalContentComponent, CustomersPageComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NzButtonModule,
    NzIconModule,
    NzFormModule,
    NzTypographyModule,
    NzDividerModule,
    NzSpaceModule,
    NzStatisticModule,
    NzSpaceModule,
    InputFieldModule,
    PageModule,
    FormModalModule,
    CustomersTableWidgetModule,
  ],
  exports: [CustomersPageComponent],
})
export class CustomersPageModule {}
