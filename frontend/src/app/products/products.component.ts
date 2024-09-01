import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProductService, Product } from '../services/product.service';
import { MatDialog } from '@angular/material/dialog';
import { HttpErrorResponse } from '@angular/common/http';

// Declare bootstrap as a global variable
declare var bootstrap: any;

@Component({
  selector: 'app-product',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
  ],
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss'],
})
export class ProductComponent implements OnInit {
  products: Product[] = [];
  newProduct: Product = {
    name: '',
    portionCount: 0,
    unit: '',
    portionSize: 0
  };
  isEditMode: boolean = false;  // To track if the modal is in edit mode
  currentProductId?: number;  // To store the product ID being edited

  constructor(private productService: ProductService, public dialog: MatDialog) {}

  ngOnInit(): void {
    this.fetchProducts();
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

  openAddProductDialog(): void {
    this.isEditMode = false;  // Set to add mode
    this.resetForm();  // Clear the form

    // Open the modal for adding a product
    const modalElement = document.getElementById('myModal');
    if (modalElement) {
      const dialogRef = new bootstrap.Modal(modalElement);
      dialogRef.show();
    }
  }

  openEditProductDialog(product: Product): void {
    this.isEditMode = true;  // Set to edit mode
    this.currentProductId = product.id;
    this.newProduct = { ...product };  // Populate the form with the selected product's data

    // Open the modal for editing the product
    const modalElement = document.getElementById('myModal');
    if (modalElement) {
      const dialogRef = new bootstrap.Modal(modalElement);
      dialogRef.show();
    }
  }

  submitProduct(): void {
    if (this.isEditMode && this.currentProductId !== undefined) {
      // Log the payload before sending
      const payload = { ...this.newProduct, id: this.currentProductId };
      console.log('Updating product with payload:', payload);
  
      // Update existing product
      this.productService.updateProduct(this.currentProductId, payload).subscribe({
        next: (updatedProduct: Product | null) => {
          if (updatedProduct === null) {
            console.error('Error: updatedProduct is null.');
            return;
          }

          const index = this.products.findIndex(p => p.id === updatedProduct.id);
          if (index !== -1) {
            this.products[index] = updatedProduct;  // Update the product in the list
          } else {
            console.error(`Error: Product with id ${updatedProduct.id} not found in the products array.`);
          }
          this.resetForm();
        },
        error: (error: HttpErrorResponse) => {
          console.error('Error updating product:', error);
          if (error.status === 400) {
            console.error('Bad Request: Please check the submitted data.');
            if (error.error) {
              console.error('Error details:', error.error); // Log detailed error message
            }
          }
        },
      });
    } else {
      // Add new product
      this.productService.addProduct(this.newProduct).subscribe({
        next: (newProduct: Product | null) => {
          if (newProduct === null) {
            console.error('Error: newProduct is null.');
            return;
          }
  
          this.products.push(newProduct);
          this.resetForm(); // Reset form after submission
        },
        error: (error: any) => {
          console.error('Error adding product:', error);
        },
      });
    }
  }

  deleteProduct(productId: number): void {
    if (confirm('Are you sure you want to delete this product?')) {
      this.productService.deleteProduct(productId).subscribe({
        next: () => {
          this.products = this.products.filter(product => product.id !== productId); // Remove the deleted product from the list
          console.log(`Product with id ${productId} deleted successfully.`);
        },
        error: (error: HttpErrorResponse) => {
          console.error('Error deleting product:', error);
        }
      });
    }
  }

  resetForm(): void {
    this.newProduct = {
      name: '',
      portionCount: 0,
      unit: '',
      portionSize: 0
    };
    this.currentProductId = undefined;
    this.isEditMode = false;
  }
}
