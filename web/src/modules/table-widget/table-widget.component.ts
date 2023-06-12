import { Component, ContentChild, Input } from '@angular/core';
import { TableWidgetHeaderComponent } from './table-widget-header/table-widget-header.component';
import { WidgetState } from '../widget/widget-state';

@Component({
  selector: 'bip-table-widget[widgetState]',
  templateUrl: './table-widget.component.html',
  styleUrls: ['./table-widget.component.less'],
})
export class TableWidgetComponent {
  @ContentChild(TableWidgetHeaderComponent) tableWidgetHeader?: TableWidgetHeaderComponent;

  @Input() widgetState!: WidgetState;
}
