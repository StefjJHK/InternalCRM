import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IfPermissionsDirective } from './if-permission.directive';

@NgModule({
  declarations: [IfPermissionsDirective],
  imports: [CommonModule],
  exports: [IfPermissionsDirective],
})
export class PermissionsModule {}
