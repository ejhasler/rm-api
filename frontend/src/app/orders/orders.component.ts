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
    menuItemIdsString: ''  // This will be used to facilitate input and display
  };
  isEditMode: boolean = false;  // To track if the modal is in edit mode
  currentOrderId?: number;  // To store the order ID being edited

  constructor(private orderService: OrderService) {}

  ngOnInit(): void {
    this.fetchOrders();
  }

  fetchOrders(): void {
    this.orderService.getOrders().subscribe({
      next: (data: Order[]) => {
        this.orders = data.map(order => ({
          ...order,
          menuItemIdsString: (order.menuItemIds || []).join(', ')  // Safely handle undefined menuItemIds
        }));
      },
      error: (error: any) => {
        console.error('Error fetching orders:', error);
      },
    });
  }

  openAddOrderDialog(): void {
    this.isEditMode = false;  // Set to add mode
    this.resetForm();  // Clear the form

    // Open the modal for adding an order
    const modalElement = document.getElementById('myModal');
    if (modalElement) {
      const dialogRef = new bootstrap.Modal(modalElement);
      dialogRef.show();
    }
  }

  openEditOrderDialog(order: Order): void {
    this.isEditMode = true;  // Set to edit mode
    this.currentOrderId = order.id;
    this.newOrder = { ...order };  // Populate the form with the selected order's data

    // Convert menuItemIds array to a comma-separated string
    this.newOrder.menuItemIdsString = order.menuItemIds.join(', ');

    // Open the modal for editing the order
    const modalElement = document.getElementById('myModal');
    if (modalElement) {
      const dialogRef = new bootstrap.Modal(modalElement);
      dialogRef.show();
    }
  }

  submitOrder(): void {
    // Ensure menuItemIdsString is not undefined before splitting and mapping
    if (this.newOrder.menuItemIdsString) {
      this.newOrder.menuItemIds = this.newOrder.menuItemIdsString.split(',').map(id => parseInt(id.trim(), 10));
    } else {
      this.newOrder.menuItemIds = [];  // If menuItemIdsString is undefined, set menuItemIds to an empty array
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
              console.error('Error details:', error.error); // Log detailed error message
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
          this.resetForm(); // Reset form after submission
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
          this.orders = this.orders.filter(order => order.id !== orderId); // Remove the deleted order from the list
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
