import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { FormState } from './form-state';

@Component({
  selector: 'bip-form-modal-content[formState]',
  templateUrl: './form-modal-content.component.html',
  styleUrls: ['./form-modal-content.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FormModalContentComponent {
  @Input() formState: FormState | null = null;

  FormState = FormState;
}
