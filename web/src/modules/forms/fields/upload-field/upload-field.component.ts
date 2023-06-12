import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input } from '@angular/core';
import { NzUploadFile, NzUploadXHRArgs } from 'ng-zorro-antd/upload';
import { empty, Subscription } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { BipFormControl } from '../../form-controls/bip-form-control';

@UntilDestroy()
@Component({
  selector: 'bip-upload-field[bipFormControl]',
  templateUrl: './upload-field.component.html',
  styleUrls: ['./upload-field.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UploadFieldComponent {
  @Input() bipFormControl!: BipFormControl<File | null>;

  @Input() hideDisplayName = false;

  @Input() required = false;

  @Input() accept?: string | string[];

  get files(): NzUploadFile[] {
    return this.bipFormControl.value ? [this.bipFormControl.value] : [];
  }

  get acceptFormatsMessage(): string {
    if (Array.isArray(this.accept)) {
      return this.accept.join(', ');
    }

    return this.accept as string;
  }

  constructor(private changeDetector: ChangeDetectorRef) {}

  ngOnInit() {
    this.bipFormControl.touchedChanges.pipe(untilDestroyed(this)).subscribe(() => {
      this.changeDetector.markForCheck();
    });
  }

  handleSet = (params: NzUploadXHRArgs): Subscription => {
    this.bipFormControl.setValue(params.file);

    return empty().subscribe();
  };

  handleRemove = (file: NzUploadFile): boolean => {
    this.bipFormControl.setValue(null);

    return true;
  };
}
