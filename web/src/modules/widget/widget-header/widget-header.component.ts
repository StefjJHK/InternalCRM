import { Component, Input } from '@angular/core';
import { Button } from '../../common-components/button/button.model';

@Component({
  selector: 'bip-widget-header',
  templateUrl: './widget-header.component.html',
  styleUrls: ['./widget-header.component.less'],
})
export class WidgetHeaderComponent {
  @Input() title?: string;

  @Input() buttons?: Button[];
}
