<div align="center">
  <h1 align="center">Restaurant Manager</h1>
</div>
<br />
<p align="center">A project for Visma Case Interview Round 2</p>

<div align="center">
  <a href="#getting-started">Getting started</a> •
  <a href="https://cgg.datasnok.cool/">View demo</a> •
  <a href="#license">License</a>
</div>

## Table of contents

- [Project structure](#project-structure)
- [Architecture](#architecture)
- [Useful resources](#useful-resources)
- [Getting started](#getting-started)
  - [Environment variables](#environment-variables)
  - [Development](#development)
  - [Running tests](#running-tests)
  - [Deployment](#deployment)
- [License](#license)

- ## Project structure

```
├───RestaurantManagerAPI
│   ├───src (ASP.NET backend)
│    │   ├───Controllers
│    │   ├───Data
│    │        ├───Repositories
│    │   ├───Models
│    │         ├───DTOs
│    │   ├───Services
│   ├───test (Unit Tests)
│
├───frontend (admin frontend)
```

This repository contains the full-stack implementation of the Restaurant Manager application, which consists of a REST API backend and an admin frontend. The backend is developed using C# with the ASP.NET framework, and the frontend is built using Angular with TypeScript and Bootstrap.

### Backend (RestaurantManagerAPI)
The backend (RestaurantManagerAPI) is an ASP.NET-based RESTful API designed to manage the core functionalities of the restaurant management system. The backend structure includes:

* Controllers: Define the API endpoints and handle incoming HTTP requests.
* Data: Contains data access logic, including repositories that interact with the database.
* Models: Represents the application's data models, including DTOs (Data Transfer Objects) for structured data exchange.
* Services: Implements business logic and facilitates communication between controllers and data repositories.

Unit tests for the backend are located in the test directory, utilizing JUnit testing framework with mocking to ensure robust and reliable API functionality.

### Frontend
The frontend directory contains the Angular application that serves as the admin interface for managing restaurant operations. It is built using Angular with TypeScript and styled using Bootstrap and SCSS, providing a user-friendly UI for administrators.

A diagram of the project structure can be found [here](Fillin).

## Architecture

## Useful resources

Some internal resources you may find useful:

## Getting started

### Development

### Running tests

## License

Distributed under the [MIT License](LICENSE).


