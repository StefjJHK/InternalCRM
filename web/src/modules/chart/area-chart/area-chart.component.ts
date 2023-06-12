import { ChangeDetectionStrategy, Component, Input, SimpleChanges } from '@angular/core';
import { EChartsOption, LineSeriesOption } from 'echarts';
import { ChartData } from '../chart-data.model';

const seriesDefault: LineSeriesOption = {
  type: 'line',
  stack: 'Total',
  smooth: true,
  lineStyle: {},
  showSymbol: false,
  areaStyle: {
    opacity: 0.8,
  },
  emphasis: {
    focus: 'series',
  },
};

const areaEChartsOptionDefault: EChartsOption = {
  color: ['#80FFA5'],
  grid: {
    left: 0,
    right: 0,
    top: 0,
    bottom: 0,
  },
  yAxis: [
    {
      type: 'value',
      show: false,
    },
  ],
  xAxis: [
    {
      type: 'category',
      show: false,
      boundaryGap: false,
    },
  ],
  series: seriesDefault,
};

@Component({
  selector: 'bip-area-chart[data]',
  templateUrl: './area-chart.component.html',
  styleUrls: ['./area-chart.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AreaChartComponent {
  @Input() data!: ChartData;

  eChartsOptions: EChartsOption = areaEChartsOptionDefault;

  ngOnChanges(changes: SimpleChanges) {
    if (changes.data) {
      this.eChartsOptions.series = {
        ...seriesDefault,
        data: this.data.chartSeries.map((x) => x.value),
      };
    }
  }
}
