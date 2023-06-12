import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { BipFormControl } from '../../form-controls/bip-form-control';

@UntilDestroy()
@Component({
  selector: 'bip-date-picker-field[bipFormControl]',
  templateUrl: './date-picker-field.component.html',
  styleUrls: ['./date-picker-field.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DatePickerFieldComponent {
  @Input() bipFormControl!: BipFormControl<Date>;

  @Input() required = false;

  @Input() hideDisplayName = false;

  @Input() showTime = false;

  @Input() mode: 'year' | 'date' = 'date';

  get date(): Date {
    return this.bipFormControl.value;
  }

  set date(date: Date) {
    this.bipFormControl.setValue(date);
  }

  constructor(private changeDetector: ChangeDetectorRef) {}

  ngOnInit() {
    this.bipFormControl.touchedChanges.pipe(untilDestroyed(this)).subscribe(() => {
      this.changeDetector.markForCheck();
    });
  }
}
