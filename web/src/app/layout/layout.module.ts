import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutComponent } from './layout.component';
import { GlobalLoaderModule } from '../../modules/loader/global-loader/global-loader.module';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [LayoutComponent],
  imports: [CommonModule, GlobalLoaderModule, RouterModule],
  exports: [LayoutComponent],
})
export class LayoutModule {}
