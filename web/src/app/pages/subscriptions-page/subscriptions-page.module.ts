import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { ReactiveFormsModule } from '@angular/forms';
import { NzFormModule } from 'ng-zorro-antd/form';
import { FormModalModule } from 'src/modules/forms/form-modal/form-modal.module';
import { NzTypographyModule } from 'ng-zorro-antd/typography';
import { NzPageHeaderModule } from 'ng-zorro-antd/page-header';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzStatisticModule } from 'ng-zorro-antd/statistic';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { SubscriptionsPageComponent } from './subscriptions-page.component';
import { CreateSubscriptionModalContent } from './create-subscription-modal-content/create-subscription-modal-content';
import { PageModule } from '../../../modules/page/page.module';
import { DateRangePickerFieldModule } from '../../../modules/forms/fields/date-range-picker-field/date-range-picker-field.module';
import { InputFieldModule } from '../../../modules/forms/fields/input-field/input-field.module';
import { SelectFieldModule } from '../../../modules/forms/fields/select-field/select-field.module';
import { SubscriptionsTableWidgetModule } from '../../../modules/subscription/subscriptions-table-widget/subscriptions-table-widget.module';

@NgModule({
  declarations: [SubscriptionsPageComponent, CreateSubscriptionModalContent],
  imports: [
    CommonModule,
    InputFieldModule,
    FormModalModule,
    SelectFieldModule,
    DateRangePickerFieldModule,
    ReactiveFormsModule,
    SelectFieldModule,
    NzButtonModule,
    NzPageHeaderModule,
    NzIconModule,
    NzFormModule,
    NzTypographyModule,
    NzStatisticModule,
    NzSpaceModule,
    PageModule,
    FormModalModule,
    SubscriptionsTableWidgetModule,
  ],
  exports: [SubscriptionsPageComponent],
})
export class SubscriptionsPageModule {}
