import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonComponent } from './button.component';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';

@NgModule({
  declarations: [ButtonComponent],
  imports: [CommonModule, NzSpaceModule, NzButtonModule, NzIconModule],
  exports: [ButtonComponent],
})
export class ButtonModule {}
