import { ChangeDetectorRef, Component, Input } from '@angular/core';
import { SelectFieldOption } from './select-field-option';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { SelectValue } from './select-value';
import { BipFormControl } from '../../form-controls/bip-form-control';

@UntilDestroy()
@Component({
  selector: 'bip-select-field[bipFormControl][options]',
  templateUrl: './select-field.component.html',
  styleUrls: ['./select-field.component.less'],
})
export class SelectFieldComponent {
  @Input() bipFormControl!: BipFormControl<SelectValue | SelectValue[]>;

  @Input() options!: SelectFieldOption[];

  @Input() placeholder: string | null = null;

  @Input() mode: 'multiple' | 'default' = 'default';

  @Input() customInput = false;

  @Input() required = false;

  @Input() disabled = false;

  @Input() hideDisplayName = false;

  constructor(private changeDetectorRef: ChangeDetectorRef) {}

  ngOnInit() {
    this.bipFormControl.touchedChanges.pipe(untilDestroyed(this)).subscribe(() => {
      this.changeDetectorRef.markForCheck();
    });
  }

  addItem(input: HTMLInputElement): void {
    const value = input.value;

    if (this.bipFormControl.value.indexOf(value) === -1) {
      this.options = [
        ...this.options,
        {
          value,
          label: value,
        },
      ];
      this.bipFormControl.setValue([...this.bipFormControl.value, value]);
    }
  }
}
