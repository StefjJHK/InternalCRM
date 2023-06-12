import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminPageComponent } from './admin-page.component';
import { PageModule } from '../../../modules/page/page.module';
import { UsersTableWidgetModule } from '../../../modules/user/users-table-widget/users-table-widget.module';
import { CreateUserModalContentComponent } from './create-user-modal-content/create-user-modal-content.component';
import { FormModalModule } from '../../../modules/forms/form-modal/form-modal.module';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { ReactiveFormsModule } from '@angular/forms';
import { InputFieldModule } from '../../../modules/forms/fields/input-field/input-field.module';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { RadioFieldModule } from '../../../modules/forms/fields/radio-field/radio-field.module';

@NgModule({
  declarations: [AdminPageComponent, CreateUserModalContentComponent],
  imports: [
    CommonModule,
    PageModule,
    UsersTableWidgetModule,
    FormModalModule,
    NzFormModule,
    NzSpaceModule,
    ReactiveFormsModule,
    InputFieldModule,
    RadioFieldModule,

    NzGridModule,
  ],
  exports: [AdminPageComponent],
})
export class AdminPageModule {}
