import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzResultModule } from 'ng-zorro-antd/result';
import { UnauthorizedPageComponent } from './unauthorized-page.component';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { CardModule } from '../../../modules/common-components/card/card.module';

@NgModule({
  declarations: [UnauthorizedPageComponent],
  imports: [CommonModule, NzResultModule, NzButtonModule, NzGridModule, CardModule],
  exports: [UnauthorizedPageComponent],
})
export class UnauthorizedPageModule {}
