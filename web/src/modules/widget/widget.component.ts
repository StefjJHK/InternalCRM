import { ChangeDetectionStrategy, Component, ContentChild, Input } from '@angular/core';
import { WidgetState } from './widget-state';
import { WidgetHeaderComponent } from './widget-header/widget-header.component';

@Component({
  selector: 'bip-widget[widgetState]',
  templateUrl: './widget.component.html',
  styleUrls: ['./widget.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WidgetComponent {
  @ContentChild(WidgetHeaderComponent) widgetHeader?: WidgetHeaderComponent;

  @Input() widgetState!: WidgetState;

  @Input() disableBodyPaddings = false;

  WidgetState = WidgetState;
}
