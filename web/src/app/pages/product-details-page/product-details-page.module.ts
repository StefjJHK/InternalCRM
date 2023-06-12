import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductDetailsPageComponent } from './product-details-page.component';
import { ProductCustomersWidgetModule } from '../../../modules/product/product-customers-widget/product-customers-widget.module';
import { PageModule } from '../../../modules/page/page.module';
import { TabsetModule } from '../../../modules/common-components/tabset/tabset.module';
import { SubscriptionsTableWidgetModule } from '../../../modules/subscription/subscriptions-table-widget/subscriptions-table-widget.module';
import { DeleteConfirmationModule } from '../../../modules/forms/delete-confirmation/delete-confirmation.module';
import { UpdateProductModalContentComponent } from './update-product-modal-content/update-product-modal-content.component';
import { FormModalModule } from '../../../modules/forms/form-modal/form-modal.module';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { InputFieldModule } from '../../../modules/forms/fields/input-field/input-field.module';
import { UploadFieldModule } from '../../../modules/forms/fields/upload-field/upload-field.module';
import { NzTypographyModule } from 'ng-zorro-antd/typography';
import { ReactiveFormsModule } from '@angular/forms';
import { NzFormModule } from 'ng-zorro-antd/form';

@NgModule({
  declarations: [ProductDetailsPageComponent, UpdateProductModalContentComponent],
  imports: [
    CommonModule,
    ProductCustomersWidgetModule,
    PageModule,
    TabsetModule,
    SubscriptionsTableWidgetModule,
    DeleteConfirmationModule,
    FormModalModule,
    NzGridModule,
    InputFieldModule,
    UploadFieldModule,
    NzTypographyModule,
    ReactiveFormsModule,
    NzFormModule,
  ],
  exports: [ProductDetailsPageComponent],
})
export class ProductDetailsPageModule {}
