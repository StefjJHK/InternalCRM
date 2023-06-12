import { ModuleWithProviders, NgModule } from '@angular/core';
import { registerLocaleData } from '@angular/common';
import en from '@angular/common/locales/en';
import { en_US, NZ_I18N } from 'ng-zorro-antd/i18n';
import { NZ_CONFIG } from 'ng-zorro-antd/core/config';
import { ngZorroConfig } from './ant-design.config';
import { HttpClientModule } from '@angular/common/http';
import { NzNotificationModule } from 'ng-zorro-antd/notification';

registerLocaleData(en);

@NgModule({
  imports: [HttpClientModule, NzNotificationModule],
})
export class AntDesignModule {
  static forRoot(): ModuleWithProviders<AntDesignModule> {
    return {
      ngModule: AntDesignModule,
      providers: [
        { provide: NZ_I18N, useValue: en_US },
        { provide: NZ_CONFIG, useValue: ngZorroConfig },
      ],
    };
  }
}
