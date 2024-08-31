import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common'; // Import CommonModule for ngFor

@Component({
  selector: 'app-side-nav',
  standalone: true,
  imports: [CommonModule], // Add CommonModule to imports
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
    },
    {
      number: '2',
      name: 'Products',
      icon: 'fa-solid fa-bowl-food',
    },
    {
      number: '3',
      name: 'Menu',
      icon: 'fa-solid fa-utensils',
    },
    {
      number: '4',
      name: 'Order',
      icon: 'fa-solid fa-chart-line',
    },
  ];

  constructor() { }

  ngOnInit(): void {}
}

