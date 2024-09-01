import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { OrderService, Order } from '../services/order.service';
import { HttpErrorResponse } from '@angular/common/http';

// Declare bootstrap as a global variable
declare var bootstrap: any;

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
  ],
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss'],
})
export class OrdersComponent implements OnInit {
  orders: Order[] = [];
  newOrder: Order = {
    dateTime: '',
    menuItemIds: [],
    menuItemIdsString: ''  
  };
  isEditMode: boolean = false;
  currentOrderId?: number;  

  constructor(private orderService: OrderService) {}

  ngOnInit(): void {
    this.fetchOrders();
  }

  fetchOrders(): void {
    this.orderService.getOrders().subscribe({
      next: (data: Order[]) => {
        this.orders = data.map(order => ({
          ...order,
          menuItemIdsString: (order.menuItemIds || []).join(', ') 
        }));
      },
      error: (error: any) => {
        console.error('Error fetching orders:', error);
      },
    });
  }

  openAddOrderDialog(): void {
    this.isEditMode = false; 
    this.resetForm(); 

    // Open the modal for adding an order
    const modalElement = document.getElementById('myModal');
    if (modalElement) {
      const dialogRef = new bootstrap.Modal(modalElement);
      dialogRef.show();
    }
  }

  openEditOrderDialog(order: Order): void {
    this.isEditMode = true;  
    this.currentOrderId = order.id;
    this.newOrder = { ...order };

    
    this.newOrder.menuItemIdsString = order.menuItemIds.join(', ');

    // Open the modal for editing the order
    const modalElement = document.getElementById('myModal');
    if (modalElement) {
      const dialogRef = new bootstrap.Modal(modalElement);
      dialogRef.show();
    }
  }

  submitOrder(): void {
    if (this.newOrder.menuItemIdsString) {
      this.newOrder.menuItemIds = this.newOrder.menuItemIdsString.split(',').map(id => parseInt(id.trim(), 10));
    } else {
      this.newOrder.menuItemIds = []; 
    }

    if (this.isEditMode && this.currentOrderId !== undefined) {
      // Log the payload before sending
      const payload = { ...this.newOrder, id: this.currentOrderId };
      console.log('Updating order with payload:', payload);
  
      // Update existing order
      this.orderService.updateOrder(this.currentOrderId, payload).subscribe({
        next: (updatedOrder: Order | null) => {
          if (updatedOrder === null) {
            console.error('Error: updatedOrder is null.');
            return;
          }

          const index = this.orders.findIndex(o => o.id === updatedOrder.id);
          if (index !== -1) {
            this.orders[index] = { ...updatedOrder, menuItemIdsString: updatedOrder.menuItemIds.join(', ') };  // Update the order in the list
          } else {
            console.error(`Error: Order with id ${updatedOrder.id} not found in the orders array.`);
          }
          this.resetForm();
        },
        error: (error: HttpErrorResponse) => {
          console.error('Error updating order:', error);
          if (error.status === 400) {
            console.error('Bad Request: Please check the submitted data.');
            if (error.error) {
              console.error('Error details:', error.error); 
            }
          }
        },
      });
    } else {
      // Add new order
      this.orderService.addOrder(this.newOrder).subscribe({
        next: (newOrder: Order | null) => {
          if (newOrder === null) {
            console.error('Error: newOrder is null.');
            return;
          }

          this.orders.push({ ...newOrder, menuItemIdsString: newOrder.menuItemIds.join(', ') });
          this.resetForm(); 
        },
        error: (error: any) => {
          console.error('Error adding order:', error);
        },
      });
    }
  }

  deleteOrder(orderId: number): void {
    if (confirm('Are you sure you want to delete this order?')) {
      this.orderService.deleteOrder(orderId).subscribe({
        next: () => {
          this.orders = this.orders.filter(order => order.id !== orderId); 
          console.log(`Order with id ${orderId} deleted successfully.`);
        },
        error: (error: HttpErrorResponse) => {
          console.error('Error deleting order:', error);
        }
      });
    }
  }

  resetForm(): void {
    this.newOrder = {
      dateTime: '',
      menuItemIds: [],
      menuItemIdsString: ''
    };
    this.currentOrderId = undefined;
    this.isEditMode = false;
  }
}
