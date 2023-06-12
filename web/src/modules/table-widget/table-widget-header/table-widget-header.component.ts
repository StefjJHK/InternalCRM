import { Component, Input } from '@angular/core';
import { Button } from '../../common-components/button/button.model';

@Component({
  selector: 'bip-table-widget-header[title]',
  templateUrl: './table-widget-header.component.html',
  styleUrls: ['./table-widget-header.component.less'],
})
export class TableWidgetHeaderComponent {
  @Input() title!: string;

  @Input() buttons?: Button[];
}
