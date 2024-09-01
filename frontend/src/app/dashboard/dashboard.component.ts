import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { OrderService, Order } from '../services/order.service';
import { MenuService, Menu } from '../services/menu.service';
import { ProductService, Product } from '../services/product.service';

// Declare bootstrap as a global variable
declare var bootstrap: any;

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  orders: Order[] = [];
  menus: Menu[] = [];
  products: Product[] = [];

  constructor(
    private orderService: OrderService,
    private menuService: MenuService,
    private productService: ProductService
  ) {}

  ngOnInit(): void {
    this.fetchOrders();
    this.fetchMenus();
    this.fetchProducts();
  }

  fetchOrders(): void {
    this.orderService.getOrders().subscribe({
      next: (data: Order[]) => {
        this.orders = data;
      },
      error: (error: any) => {
        console.error('Error fetching orders:', error);
      },
    });
  }

  fetchMenus(): void {
    this.menuService.getMenus().subscribe({
      next: (data: Menu[]) => {
        this.menus = data;
      },
      error: (error: any) => {
        console.error('Error fetching menus:', error);
      },
    });
  }

  fetchProducts(): void {
    this.productService.getProducts().subscribe({
      next: (data: Product[]) => {
        this.products = data;
      },
      error: (error: any) => {
        console.error('Error fetching products:', error);
      },
    });
  }
}
