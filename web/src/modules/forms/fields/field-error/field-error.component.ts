import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { BipFormControl } from '../../form-controls/bip-form-control';

@Component({
  selector: 'bip-field-error[bipFormControl]',
  templateUrl: './field-error.component.html',
  styleUrls: ['./field-error.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FieldErrorComponent {
  @Input() bipFormControl!: BipFormControl<any>;
}
