import { ChangeDetectorRef, Component, Input } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { BipFormControl } from '../../form-controls/bip-form-control';

@UntilDestroy()
@Component({
  selector: 'bip-date-range-picker-field[bipFormControl]',
  templateUrl: './date-range-picker-field.component.html',
  styleUrls: ['./date-range-picker-field.component.less'],
})
export class DateRangePickerFieldComponent {
  @Input() bipFormControl!: BipFormControl<Date[]>;

  @Input() required = false;

  @Input() hideDisplayName = false;

  @Input() showTime = false;

  @Input() mode: 'year' | 'date' = 'date';

  get dates(): Date[] {
    return this.bipFormControl.value;
  }

  set dates(dates: Date[]) {
    this.bipFormControl.setValue(dates);
  }

  constructor(private changeDetector: ChangeDetectorRef) {}

  ngOnInit() {
    this.bipFormControl.touchedChanges.pipe(untilDestroyed(this)).subscribe(() => {
      this.changeDetector.markForCheck();
    });
  }
}
