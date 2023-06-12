import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

@Component({
  selector: 'bip-chart-widget-header[title]',
  templateUrl: './chart-widget-header.component.html',
  styleUrls: ['./chart-widget-header.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ChartWidgetHeaderComponent {
  @Input() title!: string;
}
