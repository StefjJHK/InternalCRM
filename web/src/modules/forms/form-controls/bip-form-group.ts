import { BipFormControl } from './bip-form-control';
import { FormGroup } from '@angular/forms';
import { EventEmitter } from '@angular/core';
import { Observable } from 'rxjs';

export class BipFormGroup extends FormGroup {
  private readonly _touchedChanges: EventEmitter<boolean> = new EventEmitter<boolean>();

  readonly touchedChanges!: Observable<boolean>;

  constructor(controls: { [key: string]: BipFormControl }) {
    super(controls);

    this.touchedChanges = this._touchedChanges.asObservable();
  }

  public get bipControls(): { [key: string]: BipFormControl } {
    return this.controls as { [key: string]: BipFormControl };
  }

  submitForm(): void {
    this.markAllAsTouched();
  }

  override markAsTouched(opts?: { onlySelf?: boolean }): void {
    super.markAsTouched(opts);
    this._touchedChanges.emit(this.touched);
  }

  override markAllAsTouched(): void {
    super.markAllAsTouched();
    this._touchedChanges.emit(this.touched);
  }
}
