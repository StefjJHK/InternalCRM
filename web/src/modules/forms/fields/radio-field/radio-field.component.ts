import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { BipFormControl } from '../../form-controls/bip-form-control';
import { RadioFieldOption } from './radio-field-option';

@UntilDestroy()
@Component({
  selector: 'bip-radio-field[bipFormControl][options]',
  templateUrl: './radio-field.component.html',
  styleUrls: ['./radio-field.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RadioFieldComponent {
  @Input() bipFormControl!: BipFormControl<string | boolean | number>;

  @Input() options!: RadioFieldOption[];

  @Input() hideDisplayName = false;

  constructor(private changeDetector: ChangeDetectorRef) {}

  ngOnInit() {
    this.bipFormControl.touchedChanges.pipe(untilDestroyed(this)).subscribe(() => {
      this.changeDetector.markForCheck();
    });
  }
}
