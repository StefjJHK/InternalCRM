import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './routing/app-routing.module';
import { AppRootComponent } from './app-root.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AntDesignModule } from '../modules/ant-design/ant-design.module';
import { NotFoundPageModule } from './pages/not-found-page/not-found-page.module';
import { ProductsPageModule } from './pages/products-page/products-page.module';
import { ENVIRONMENT } from '../modules/environment';
import { environment } from '../modules/environment/environment';
import { CustomersPageModule } from './pages/customers-page/customers-page.module';
import { LeadsPageModule } from './pages/leads-page/leads-page.module';
import { PurchaseOrdersPageModule } from './pages/purchase-orders-page/purchase-orders-page.module';
import { InvoicesPageModule } from './pages/invoices-page/invoices-page.module';
import { PaymentsPageModule } from './pages/payments-page/payments-page.module';
import { SubscriptionsPageModule } from './pages/subscriptions-page/subscriptions-page.module';
import { ProductDetailsPageModule } from './pages/product-details-page/product-details-page.module';
import { ChartModule } from '../modules/chart/chart.module';
import { DashboardPageModule } from './pages/dashboard-page/dashboard-page.module';
import { LoginPageModule } from './pages/login-page/login-page.module';
import { LayoutModule } from './layout/layout.module';
import { PageLayoutModule } from './layout/page-layout/page-layout.module';
import { AuthModule } from '../modules/auth/auth.module';
import { UnauthorizedPageModule } from './pages/unauthorized-page/unauthorized-page.module';
import { PermissionsDeniedPageModule } from './pages/permissions-denied-page/permissions-denied-page.module';
import { CustomerDetailsPageModule } from './pages/customer-details-page/customer-details-page.module';
import { LeadDetailsPageModule } from './pages/lead-details-page/lead-details-page.module';
import { PurchaseOrderDetailsPageModule } from './pages/purchase-order-details-page/purchase-order-details-page.module';
import { InvoiceDetailsPageModule } from './pages/invoice-details-page/invoice-details-page.module';
import { AdminPageModule } from './pages/admin-page/admin-page.module';

@NgModule({
  declarations: [AppRootComponent],
  imports: [
    LayoutModule,
    PageLayoutModule,
    AppRoutingModule,
    LoginPageModule,
    AdminPageModule,
    ProductsPageModule,
    NotFoundPageModule,
    CustomersPageModule,
    LeadsPageModule,
    PurchaseOrdersPageModule,
    InvoicesPageModule,
    PaymentsPageModule,
    SubscriptionsPageModule,
    ProductDetailsPageModule,
    InvoiceDetailsPageModule,
    DashboardPageModule,
    CustomerDetailsPageModule,
    LeadDetailsPageModule,
    PurchaseOrderDetailsPageModule,
    BrowserAnimationsModule,
    UnauthorizedPageModule,
    PermissionsDeniedPageModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    AuthModule.forRoot(),
    AntDesignModule.forRoot(),
    ChartModule.forRoot(),
  ],
  providers: [{ provide: ENVIRONMENT, useValue: environment }],
  bootstrap: [AppRootComponent],
})
export class AppRootModule {}
