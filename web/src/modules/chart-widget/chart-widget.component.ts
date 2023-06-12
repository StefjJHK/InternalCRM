import { ChangeDetectionStrategy, Component, ContentChild, Input } from '@angular/core';
import { WidgetState } from '../widget/widget-state';
import { ChartWidgetHeaderComponent } from './chart-widget-header/chart-widget-header.component';
import { ChartWidgetFilterComponent } from './chart-widget-filter/chart-widget-filter.component';

@Component({
  selector: 'bip-chart-widget[widgetState][data]',
  templateUrl: './chart-widget.component.html',
  styleUrls: ['./chart-widget.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ChartWidgetComponent {
  @Input() widgetState!: WidgetState;

  @Input() data?: unknown[];

  @ContentChild(ChartWidgetHeaderComponent) header?: ChartWidgetHeaderComponent;

  @ContentChild(ChartWidgetFilterComponent) filter?: ChartWidgetFilterComponent;
}
