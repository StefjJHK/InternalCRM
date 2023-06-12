import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WidgetComponent } from './widget.component';
import { WidgetHeaderComponent } from './widget-header/widget-header.component';
import { CardModule } from '../common-components/card/card.module';
import { NzResultModule } from 'ng-zorro-antd/result';
import { LoaderModule } from '../loader/loader.module';

@NgModule({
  declarations: [WidgetComponent, WidgetHeaderComponent],
  imports: [CommonModule, CardModule, LoaderModule, NzResultModule],
  exports: [WidgetComponent, WidgetHeaderComponent],
})
export class WidgetModule {}
