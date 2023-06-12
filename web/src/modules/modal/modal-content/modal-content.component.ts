import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'bip-modal-content',
  templateUrl: './modal-content.component.html',
  styleUrls: ['./modal-content.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ModalContentComponent {}
