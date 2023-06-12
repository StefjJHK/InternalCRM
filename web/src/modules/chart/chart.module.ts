import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import * as echarts from 'echarts';
import { NgxEchartsModule } from 'ngx-echarts';

@NgModule({
  declarations: [],
  imports: [CommonModule, NgxEchartsModule],
  exports: [NgxEchartsModule],
})
export class ChartModule {
  static forRoot(): ModuleWithProviders<ChartModule> {
    return {
      ngModule: ChartModule,
      providers: [
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        ...NgxEchartsModule.forRoot({
          echarts,
        }).providers!,
      ],
    };
  }
}
