import { FormControl, FormControlState } from '@angular/forms';
import { BipFormControlOptions } from './bip-form-control-options';
import { Observable } from 'rxjs';
import { EventEmitter } from '@angular/core';

export class BipFormControl<T = unknown> extends FormControl {
  private readonly _touchedChanges: EventEmitter<boolean> = new EventEmitter<boolean>();

  name: string;

  readonly: boolean;

  readonly touchedChanges!: Observable<boolean>;

  constructor(value: T | FormControlState<T>, name: string, opts?: BipFormControlOptions | null) {
    super(value, opts);
    this.name = name;
    this.readonly = opts?.readonly ?? false;

    this.touchedChanges = this._touchedChanges.asObservable();
  }

  getErrors(): string[] | null {
    if (!this.errors) return null;

    return Object.entries(this.errors).map((x) => x[1].message as string);
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
