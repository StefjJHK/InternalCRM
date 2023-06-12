import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginPageComponent } from './login-page.component';
import { PageModule } from '../../../modules/page/page.module';
import { CardModule } from '../../../modules/common-components/card/card.module';
import { NzImageModule } from 'ng-zorro-antd/image';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzTypographyModule } from 'ng-zorro-antd/typography';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { ReactiveFormsModule } from '@angular/forms';
import { ButtonModule } from '../../../modules/common-components/button/button.module';
import { InputFieldModule } from '../../../modules/forms/fields/input-field/input-field.module';

@NgModule({
  declarations: [LoginPageComponent],
  imports: [
    CommonModule,
    PageModule,
    CardModule,
    NzImageModule,
    NzGridModule,
    NzTypographyModule,
    NzSpaceModule,
    InputFieldModule,
    ReactiveFormsModule,
    NzTypographyModule,
    ButtonModule,
  ],
  exports: [LoginPageComponent],
})
export class LoginPageModule {}
