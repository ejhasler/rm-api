import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ProductComponent } from './products/products.component';
import { MenuComponent } from './menu/menu.component';
import { OrdersComponent } from './orders/orders.component';

export const routes: Routes = [
  { path: 'dashboard', component: DashboardComponent },
  { path: 'products', component: ProductComponent },
  { path: 'menu', component: MenuComponent },
  { path: 'orders', component: OrdersComponent },
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' }, // Redirect to dashboard as default
  { path: '**', redirectTo: '/dashboard' } // Wildcard route for a 404 page or redirect
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
