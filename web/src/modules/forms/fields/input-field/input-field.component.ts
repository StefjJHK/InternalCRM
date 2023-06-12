import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { BipFormControl } from '../../form-controls/bip-form-control';

@UntilDestroy()
@Component({
  selector: 'bip-input-field',
  templateUrl: './input-field.component.html',
  styleUrls: ['./input-field.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class InputFieldComponent implements OnInit {
  @Input() bipFormControl!: BipFormControl<string>;

  @Input() placeholder = '';

  @Input() hideDisplayName = false;

  @Input() type: 'text' | 'password' = 'text';

  @Input() required = false;

  @Input() icon?: string;

  constructor(private changeDetector: ChangeDetectorRef) {}

  ngOnInit() {
    this.bipFormControl.touchedChanges.pipe(untilDestroyed(this)).subscribe(() => {
      this.changeDetector.markForCheck();
    });
  }
}
