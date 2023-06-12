import { ChangeDetectionStrategy, Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { BarSeriesOption, EChartsOption } from 'echarts';
import { ChartData } from '../chart-data.model';

const barSeriesEChartsOptionDefault: BarSeriesOption = {
  type: 'bar',
  barWidth: '40px',
  color: '#4AC7FC',
};
const barEChartsOptionDefault: EChartsOption = {
  grid: {
    left: 0,
    right: 0,
    top: 0,
    bottom: 0,
  },
  xAxis: {
    type: 'category',
    show: false,
  },
  yAxis: {
    type: 'value',
    show: false,
  },
  series: barSeriesEChartsOptionDefault,
};

@Component({
  selector: 'bip-bar-chart[data]',
  templateUrl: './bar-chart.component.html',
  styleUrls: ['./bar-chart.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BarChartComponent implements OnChanges {
  @Input() data!: ChartData;

  eChartsOptions: EChartsOption = barEChartsOptionDefault;

  ngOnChanges(changes: SimpleChanges) {
    if (changes.data) {
      this.eChartsOptions = {
        ...this.eChartsOptions,
        series: {
          ...barSeriesEChartsOptionDefault,
          data: this.data.chartSeries.map((x) => x.value),
        },
      };
    }
  }
}
