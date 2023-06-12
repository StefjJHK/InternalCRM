import { NgModule } from '@angular/core';
import { SideMenuComponent } from './side-menu/side-menu.component';
import { CommonModule } from '@angular/common';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { PageLayoutComponent } from './page-layout.component';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NgHttpLoaderModule } from 'ng-http-loader';
import { HeaderComponent } from './header/header.component';
import { NzImageModule } from 'ng-zorro-antd/image';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzTypographyModule } from 'ng-zorro-antd/typography';
import { RouterModule } from '@angular/router';
import { PermissionsModule } from '../../../modules/permissions/permissions.module';

@NgModule({
  declarations: [PageLayoutComponent, SideMenuComponent, HeaderComponent],
  imports: [
    CommonModule,
    NgHttpLoaderModule,
    NzLayoutModule,
    NzMenuModule,
    NzIconModule,
    NzImageModule,
    NzGridModule,
    NzTypographyModule,
    RouterModule,
    PermissionsModule,
  ],
  exports: [PageLayoutComponent],
})
export class PageLayoutModule {}
