import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductsPageComponent } from '../pages/products-page/products-page.component';
import { NotFoundPageComponent } from '../pages/not-found-page/not-found-page.component';
import { AppRoutes } from './app-routes';
import { CustomersPageComponent } from '../pages/customers-page/customers-page.component';
import { LeadsPageComponent } from '../pages/leads-page/leads-page.component';
import { PurchaseOrdersPageComponent } from '../pages/purchase-orders-page/purchase-orders-page.component';
import { InvoicesPageComponent } from '../pages/invoices-page/invoices-page.component';
import { PaymentsPageComponent } from '../pages/payments-page/payments-page.component';
import { SubscriptionsPageComponent } from '../pages/subscriptions-page/subscriptions-page.component';
import { ProductDetailsPageComponent } from '../pages/product-details-page/product-details-page.component';
import { DashboardPageComponent } from '../pages/dashboard-page/dashboard-page.component';
import { LayoutComponent } from '../layout/layout.component';
import { LoginPageComponent } from '../pages/login-page/login-page.component';
import { PageLayoutComponent } from '../layout/page-layout/page-layout.component';
import { UnauthorizedPageComponent } from '../pages/unauthorized-page/unauthorized-page.component';
import { PermissionsDeniedPageComponent } from '../pages/permissions-denied-page/permissions-denied-page.component';
import { CustomerDetailsPageComponent } from '../pages/customer-details-page/customer-details-page.component';
import { LeadDetailsPageComponent } from '../pages/lead-details-page/lead-details-page.component';
import { PurchaseOrderDetailsPageComponent } from '../pages/purchase-order-details-page/purchase-order-details-page.component';
import { InvoiceDetailsPageComponent } from '../pages/invoice-details-page/invoice-details-page.component';
import { BipGuards } from './quards';
import { permissionsFrom } from '../../utils/permissions-utils';
import { AdminPageComponent } from '../pages/admin-page/admin-page.component';

const routes: Routes = [
  {
    path: 'login',
    component: LayoutComponent,
    children: [
      {
        path: '',
        component: LoginPageComponent,
      },
    ],
  },
  {
    path: 'unauthorized',
    component: LayoutComponent,
    children: [
      {
        path: '',
        component: UnauthorizedPageComponent,
      },
    ],
  },
  {
    path: 'permissions-denied',
    component: LayoutComponent,
    children: [
      {
        path: '',
        component: PermissionsDeniedPageComponent,
      },
    ],
  },
  {
    path: '',
    component: PageLayoutComponent,
    children: [
      {
        path: 'products',
        canActivate: [BipGuards.authGuard, BipGuards.permissionsGuard],
        data: {
          permissions: permissionsFrom({ product: { canRead: true } }),
        },
        children: [
          {
            path: '',
            component: ProductsPageComponent,
          },
          {
            path: ':productName',
            component: ProductDetailsPageComponent,
          },
        ],
      },
      {
        path: 'admin',
        canActivate: [BipGuards.authGuard],
        component: AdminPageComponent,
      },
      {
        path: AppRoutes.Customers,
        canActivate: [BipGuards.authGuard, BipGuards.permissionsGuard],
        data: {
          permissions: permissionsFrom({ customer: { canRead: true } }),
        },
        children: [
          {
            path: '',
            component: CustomersPageComponent,
          },
          {
            path: ':customerName',
            component: CustomerDetailsPageComponent,
          },
        ],
      },
      {
        path: AppRoutes.Leads,
        canActivate: [BipGuards.authGuard, BipGuards.permissionsGuard],
        data: {
          permissions: permissionsFrom({ lead: { canRead: true } }),
        },
        children: [
          {
            path: '',
            component: LeadsPageComponent,
          },
          {
            path: ':leadName',
            component: LeadDetailsPageComponent,
          },
        ],
      },
      {
        path: AppRoutes.PurchaseOrders,
        canActivate: [BipGuards.authGuard, BipGuards.permissionsGuard],
        data: {
          permissions: permissionsFrom({ purchaseOrder: { canRead: true } }),
        },
        children: [
          {
            path: '',
            component: PurchaseOrdersPageComponent,
          },
          {
            path: ':purchaseOrderName',
            component: PurchaseOrderDetailsPageComponent,
          },
        ],
      },
      {
        path: AppRoutes.Invoices,
        canActivate: [BipGuards.authGuard, BipGuards.permissionsGuard],
        data: {
          permissions: permissionsFrom({ invoice: { canRead: true } }),
        },
        children: [
          {
            path: '',
            component: InvoicesPageComponent,
          },
          {
            path: ':invoiceNumber',
            component: InvoiceDetailsPageComponent,
          },
        ],
      },
      {
        path: AppRoutes.Payments,
        component: PaymentsPageComponent,
        canActivate: [BipGuards.authGuard, BipGuards.permissionsGuard],
        data: {
          permissions: permissionsFrom({ payment: { canRead: true } }),
        },
      },
      {
        path: AppRoutes.Subscriptions,
        component: SubscriptionsPageComponent,
        canActivate: [BipGuards.authGuard, BipGuards.permissionsGuard],
        data: {
          permissions: permissionsFrom({ subscription: { canRead: true } }),
        },
      },
      {
        path: 'dashboard',
        component: DashboardPageComponent,
        canActivate: [BipGuards.authGuard, BipGuards.permissionsGuard],
        data: {
          permissions: permissionsFrom({ analytics: { canRead: true } }),
        },
      },
    ],
  },
  { path: '**', component: NotFoundPageComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
