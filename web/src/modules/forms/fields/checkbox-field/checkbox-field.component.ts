import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { BipFormControl } from '../../form-controls/bip-form-control';

@UntilDestroy()
@Component({
  selector: 'bip-checkbox-field[bipFormControl]',
  templateUrl: './checkbox-field.component.html',
  styleUrls: ['./checkbox-field.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CheckboxFieldComponent {
  @Input() bipFormControl!: BipFormControl<boolean>;

  get isChecked(): boolean {
    return this.bipFormControl.value;
  }

  set isChecked(value: boolean) {
    this.bipFormControl.setValue(value);
  }

  constructor(private changeDetector: ChangeDetectorRef) {}

  ngOnInit() {
    this.bipFormControl.touchedChanges.pipe(untilDestroyed(this)).subscribe(() => {
      this.changeDetector.markForCheck();
    });
  }
}
