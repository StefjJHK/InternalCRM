import { NgModule } from '@angular/core';
import { NgxSpinnerModule } from 'ngx-spinner';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoaderComponent } from './loader.component';

@NgModule({
  declarations: [LoaderComponent],
  imports: [BrowserAnimationsModule, NgxSpinnerModule],
  exports: [LoaderComponent],
})
export class LoaderModule {}
