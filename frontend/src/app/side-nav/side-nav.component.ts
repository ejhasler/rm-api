import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';  // Import RouterModule

@Component({
  selector: 'app-side-nav',
  standalone: true,
  imports: [CommonModule, RouterModule],  // Add RouterModule here
  templateUrl: './side-nav.component.html',
  styleUrls: ['./side-nav.component.scss']
})
export class SideNavComponent implements OnInit {
  @Input() sideNavStatus: boolean = false;

  list = [
    {
      number: '1',
      name: 'Dashboard',
      icon: 'fa-solid fa-home',
      route: '/dashboard'
    },
    {
      number: '2',
      name: 'Products',
      icon: 'fa-solid fa-bowl-food',
      route: '/products'
    },
    {
      number: '3',
      name: 'Menu',
      icon: 'fa-solid fa-utensils',
      route: '/menu'
    },
    {
      number: '4',
      name: 'Order',
      icon: 'fa-solid fa-chart-line',
      route: '/orders'
    }
  ];

  constructor() { }

  ngOnInit(): void {}
}
