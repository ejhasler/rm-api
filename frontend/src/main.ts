// src/main.ts
import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { appConfig } from './app/app.config'; // Import the configuration

// Bootstrap the application with the provided configuration
bootstrapApplication(AppComponent, appConfig)
  .catch(err => console.error(err));
