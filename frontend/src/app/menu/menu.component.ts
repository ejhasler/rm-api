import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MenuService, Menu } from '../services/menu.service';
import { HttpErrorResponse } from '@angular/common/http';


declare var bootstrap: any;

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
  ],
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss'],
})
export class MenuComponent implements OnInit {
  menus: Menu[] = [];
  newMenu: Menu = {
    name: '',
    productIds: [],
    productIdsString: '' 
  };
  isEditMode: boolean = false;  
  currentMenuId?: number; 

  constructor(private menuService: MenuService) {}

  ngOnInit(): void {
    this.fetchMenus();
  }

  fetchMenus(): void {
    this.menuService.getMenus().subscribe({
      next: (data: Menu[]) => {
        this.menus = data.map(menu => ({
          ...menu,
          productIdsString: (menu.productIds || []).join(', ') 
        }));
      },
      error: (error: any) => {
        console.error('Error fetching menus:', error);
      },
    });
  }

  openAddMenuDialog(): void {
    this.isEditMode = false; 
    this.resetForm(); 

    // Open the modal for adding a menu
    const modalElement = document.getElementById('menuModal');
    if (modalElement) {
      const dialogRef = new bootstrap.Modal(modalElement);
      dialogRef.show();
    }
  }

  openEditMenuDialog(menu: Menu): void {
    this.isEditMode = true;
    this.currentMenuId = menu.id;
    this.newMenu = { ...menu }; 

    // Convert productIds array to a comma-separated string
    this.newMenu.productIdsString = menu.productIds.join(', ');

    // Open the modal for editing the menu
    const modalElement = document.getElementById('menuModal');
    if (modalElement) {
      const dialogRef = new bootstrap.Modal(modalElement);
      dialogRef.show();
    }
  }

  submitMenu(): void {
    if (this.newMenu.productIdsString) {
      this.newMenu.productIds = this.newMenu.productIdsString.split(',').map(id => parseInt(id.trim(), 10));
    } else {
      this.newMenu.productIds = [];
    }

    if (this.isEditMode && this.currentMenuId !== undefined) {
      // Log the payload before sending
      const payload = { ...this.newMenu, id: this.currentMenuId };
      console.log('Updating menu with payload:', payload);

      // Update existing menu
      this.menuService.updateMenu(this.currentMenuId, payload).subscribe({
        next: (updatedMenu: Menu | null) => {
          if (updatedMenu === null) {
            console.error('Error: updatedMenu is null.');
            return;
          }

          const index = this.menus.findIndex(m => m.id === updatedMenu.id);
          if (index !== -1) {
            this.menus[index] = { ...updatedMenu, productIdsString: updatedMenu.productIds.join(', ') };
          } else {
            console.error(`Error: Menu with id ${updatedMenu.id} not found in the menus array.`);
          }
          this.resetForm();
        },
        error: (error: HttpErrorResponse) => {
          console.error('Error updating menu:', error);
          if (error.status === 400) {
            console.error('Bad Request: Please check the submitted data.');
            if (error.error) {
              console.error('Error details:', error.error); 
            }
          }
        },
      });
    } else {
      // Add new menu
      this.menuService.addMenu(this.newMenu).subscribe({
        next: (newMenu: Menu | null) => {
          if (newMenu === null) {
            console.error('Error: newMenu is null.');
            return;
          }

          this.menus.push({ ...newMenu, productIdsString: newMenu.productIds.join(', ') });
          this.resetForm();
        },
        error: (error: any) => {
          console.error('Error adding menu:', error);
        },
      });
    }
  }

  deleteMenu(menuId: number): void {
    if (confirm('Are you sure you want to delete this menu?')) {
      this.menuService.deleteMenu(menuId).subscribe({
        next: () => {
          this.menus = this.menus.filter(menu => menu.id !== menuId); 
          console.log(`Menu with id ${menuId} deleted successfully.`);
        },
        error: (error: HttpErrorResponse) => {
          console.error('Error deleting menu:', error);
        }
      });
    }
  }

  resetForm(): void {
    this.newMenu = {
      name: '',
      productIds: [],
      productIdsString: ''
    };
    this.currentMenuId = undefined;
    this.isEditMode = false;
  }
}
