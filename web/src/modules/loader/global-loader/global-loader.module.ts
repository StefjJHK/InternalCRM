import { NgModule } from '@angular/core';
import { GlobalLoaderComponent } from './global-loader.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [GlobalLoaderComponent],
  imports: [BrowserAnimationsModule, NgxSpinnerModule],
  exports: [GlobalLoaderComponent],
})
export class GlobalLoaderModule {}
