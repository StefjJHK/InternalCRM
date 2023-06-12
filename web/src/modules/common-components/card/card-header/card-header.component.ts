import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Button } from '../../button/button.model';

@Component({
  selector: 'bip-card-header',
  templateUrl: './card-header.component.html',
  styleUrls: ['./card-header.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CardHeaderComponent {
  @Input() buttons?: Button[];
}
