import { ChangeDetectionStrategy, Component, Input, SimpleChanges } from '@angular/core';
import { EChartsOption, LineSeriesOption } from 'echarts';

const lineEChartsOptionDefault: EChartsOption = {
  xAxis: {
    type: 'category',
  },
  yAxis: {
    type: 'value',
  },
  grid: {
    right: 0,
  },
  series: [
    {
      type: 'line',
      smooth: true,
    },
    {
      type: 'line',
      smooth: true,
    },
  ],
};

@Component({
  selector: 'bip-line-chart[legendData][seriesData]',
  templateUrl: './line-chart.component.html',
  styleUrls: ['./line-chart.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LineChartComponent {
  @Input() legendData!: string[];

  @Input() seriesData!: number[][];

  eChartsOptions: EChartsOption = lineEChartsOptionDefault;

  ngOnChanges(changes: SimpleChanges) {
    if (changes.legendData) {
      this.eChartsOptions = {
        ...this.eChartsOptions,
        xAxis: {
          //TODO: find type to cast
          ...(this.eChartsOptions.xAxis as any),
          data: this.legendData,
        },
      };
    }

    if (changes.seriesData) {
      this.eChartsOptions = {
        ...this.eChartsOptions,
        series: this.seriesData.map((seriesData, index) => {
          const seriesSettingsDefault = (lineEChartsOptionDefault.series as LineSeriesOption[])[index];

          return {
            ...seriesSettingsDefault,
            data: seriesData,
          };
        }),
      };
    }
  }
}
