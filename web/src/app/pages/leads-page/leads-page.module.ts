import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LeadsPageComponent } from './leads-page.component';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { ReactiveFormsModule } from '@angular/forms';
import { CreateLeadModalContentComponent } from './create-lead-modal-content/create-lead-modal-content.component';
import { NzFormModule } from 'ng-zorro-antd/form';
import { FormModalModule } from 'src/modules/forms/form-modal/form-modal.module';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzStatisticModule } from 'ng-zorro-antd/statistic';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { PageModule } from '../../../modules/page/page.module';
import { LeadsTableWidgetModule } from '../../../modules/lead/leads-table-widget/leads-table-widget.module';
import { DateRangePickerFieldModule } from '../../../modules/forms/fields/date-range-picker-field/date-range-picker-field.module';
import { InputFieldModule } from '../../../modules/forms/fields/input-field/input-field.module';
import { SelectFieldModule } from '../../../modules/forms/fields/select-field/select-field.module';

@NgModule({
  declarations: [LeadsPageComponent, CreateLeadModalContentComponent],
  imports: [
    CommonModule,
    InputFieldModule,
    FormModalModule,
    SelectFieldModule,
    DateRangePickerFieldModule,
    ReactiveFormsModule,
    NzButtonModule,
    NzIconModule,
    NzFormModule,
    NzDividerModule,
    NzStatisticModule,
    NzSpaceModule,
    PageModule,
    FormModalModule,
    LeadsTableWidgetModule,
  ],
  exports: [LeadsPageComponent],
})
export class LeadsPageModule {}
