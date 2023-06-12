import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardComponent } from './card.component';
import { NzCardModule } from 'ng-zorro-antd/card';
import { CardHeaderComponent } from './card-header/card-header.component';
import { ButtonModule } from '../button/button.module';
import { NzSpaceModule } from 'ng-zorro-antd/space';

@NgModule({
  declarations: [CardComponent, CardHeaderComponent],
  imports: [CommonModule, NzCardModule, ButtonModule, NzSpaceModule],
  exports: [CardComponent, CardHeaderComponent],
})
export class CardModule {}
