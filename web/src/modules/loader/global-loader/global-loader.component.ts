import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { GlobalLoaderService } from './global-loader.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

@UntilDestroy()
@Component({
  selector: 'bip-global-loader',
  templateUrl: './global-loader.component.html',
  styleUrls: ['./global-loader.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class GlobalLoaderComponent implements OnInit {
  constructor(private readonly globalLoaderService: GlobalLoaderService, private readonly spinner: NgxSpinnerService) {}

  ngOnInit() {
    this.globalLoaderService.isLoading$.pipe(untilDestroyed(this)).subscribe((isLoading) => {
      if (isLoading) {
        this.spinner.show();
      } else {
        this.spinner.hide();
      }
    });
  }
}
