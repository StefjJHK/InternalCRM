import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzPageHeaderModule } from 'ng-zorro-antd/page-header';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { PageComponent } from './page.component';
import { PageHeaderComponent } from './page-header/page-header.component';
import { PageHeaderContentComponent } from './page-header/page-header-content/page-header-content.component';
import { LoaderModule } from '../loader/loader.module';
import { ButtonModule } from '../common-components/button/button.module';
import { NzResultModule } from 'ng-zorro-antd/result';

@NgModule({
  declarations: [PageComponent, PageHeaderComponent, PageHeaderContentComponent],
  imports: [CommonModule, LoaderModule, ButtonModule, NzPageHeaderModule, NzSpaceModule, NzBreadCrumbModule, NzResultModule],
  exports: [PageHeaderComponent, PageHeaderComponent, PageHeaderContentComponent, PageComponent],
})
export class PageModule {}
