import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzResultModule } from 'ng-zorro-antd/result';
import { PermissionsDeniedPageComponent } from './permissions-denied-page.component';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { CardModule } from '../../../modules/common-components/card/card.module';

@NgModule({
  declarations: [PermissionsDeniedPageComponent],
  imports: [CommonModule, NzResultModule, NzButtonModule, NzGridModule, CardModule],
  exports: [PermissionsDeniedPageComponent],
})
export class PermissionsDeniedPageModule {}
