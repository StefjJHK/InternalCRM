import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LeadDetailsPageComponent } from './lead-details-page.component';
import { PageModule } from '../../../modules/page/page.module';
import { TabsetModule } from '../../../modules/common-components/tabset/tabset.module';
import { NzDescriptionsModule } from 'ng-zorro-antd/descriptions';
import { UpdateLeadModalContentComponent } from './update-lead-modal-content/update-lead-modal-content.component';
import { FormModalModule } from '../../../modules/forms/form-modal/form-modal.module';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { InputFieldModule } from '../../../modules/forms/fields/input-field/input-field.module';
import { DateRangePickerFieldModule } from '../../../modules/forms/fields/date-range-picker-field/date-range-picker-field.module';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [LeadDetailsPageComponent, UpdateLeadModalContentComponent],
  imports: [
    CommonModule,
    PageModule,
    TabsetModule,
    NzDescriptionsModule,
    FormModalModule,
    NzSpaceModule,
    InputFieldModule,
    DateRangePickerFieldModule,
    NzFormModule,
    NzGridModule,
    ReactiveFormsModule,
  ],
  exports: [LeadDetailsPageComponent],
})
export class LeadDetailsPageModule {}
