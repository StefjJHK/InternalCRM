import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductsPageComponent } from './products-page.component';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { ReactiveFormsModule } from '@angular/forms';
import { CreateProductModalContentComponent } from './create-product-modal-content/create-product-modal-content.component';
import { NzFormModule } from 'ng-zorro-antd/form';
import { FormModalModule } from 'src/modules/forms/form-modal/form-modal.module';
import { NzTypographyModule } from 'ng-zorro-antd/typography';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { CardModule } from 'src/modules/common-components/card/card.module';
import { NzStatisticModule } from 'ng-zorro-antd/statistic';
import { LoaderModule } from '../../../modules/loader/loader.module';
import { PageModule } from '../../../modules/page/page.module';
import { CheckboxFieldModule } from '../../../modules/forms/fields/checkbox-field/checkbox-field.module';
import { InputFieldModule } from '../../../modules/forms/fields/input-field/input-field.module';
import { UploadFieldModule } from '../../../modules/forms/fields/upload-field/upload-field.module';

@NgModule({
  declarations: [ProductsPageComponent, CreateProductModalContentComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NzButtonModule,
    NzFormModule,
    NzTypographyModule,
    NzDividerModule,
    NzSpaceModule,
    NzStatisticModule,
    InputFieldModule,
    UploadFieldModule,
    CheckboxFieldModule,
    FormModalModule,
    CardModule,
    LoaderModule,
    PageModule,
  ],
  exports: [ProductsPageComponent],
})
export class ProductsPageModule {}
