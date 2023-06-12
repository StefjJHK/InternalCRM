import { Component, Input } from '@angular/core';
import { Button } from './button.model';

@Component({
  selector: 'bip-button[model]',
  templateUrl: './button.component.html',
  styleUrls: ['./button.component.less'],
})
export class ButtonComponent {
  @Input() model!: Button;

  onClick() {
    this.model.onClick();
  }
}
