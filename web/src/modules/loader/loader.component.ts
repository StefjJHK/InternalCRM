import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'bip-loader[isVisible]',
  templateUrl: './loader.component.html',
  styleUrls: ['./loader.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LoaderComponent {
  @Input() set isVisible(value: boolean) {
    if (value) {
      this.spinner.show();
    } else {
      this.spinner.hide();
    }
  }

  constructor(private readonly spinner: NgxSpinnerService) {}
}
