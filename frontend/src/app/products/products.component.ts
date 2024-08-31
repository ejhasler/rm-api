import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common'; 
import { ProductService, Product } from '../services/product.service';
import { MatDialog } from '@angular/material/dialog'; // Import MatDialogImport the dialog component

@Component({
  selector: 'app-product',
  standalone: true,
  imports: [CommonModule], // Add CommonModule to imports
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})
export class ProductComponent implements OnInit {
  products: Product[] = [];

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
      }
    });
  }
}
