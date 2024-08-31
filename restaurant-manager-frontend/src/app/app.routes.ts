// app.routes.ts
import { Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { StockManagementComponent } from './stock-management/stock-management.component';
import { MenuManagementComponent } from './menu-management/menu-management.component';
import { OrderManagementComponent } from './order-management/order-management.component';

export const routes: Routes = [
  { path: 'dashboard', component: DashboardComponent },
  { path: 'stock-management', component: StockManagementComponent },
  { path: 'menu-management', component: MenuManagementComponent },
  { path: 'order-management', component: OrderManagementComponent },
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
];
