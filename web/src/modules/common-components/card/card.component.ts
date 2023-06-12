import { ChangeDetectionStrategy, Component, ContentChild, Input } from '@angular/core';
import { CardHeaderComponent } from './card-header/card-header.component';
import { NgStyleInterface } from 'ng-zorro-antd/core/types';

@Component({
  selector: 'bip-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CardComponent {
  cardBodyStyles: NgStyleInterface = {};

  @ContentChild(CardHeaderComponent) cardHeader?: CardHeaderComponent;

  @Input() set disableBodyPaddings(value: boolean) {
    if (value) {
      this.cardBodyStyles.padding = '0';
    }
  }
}
