<div align="center">
  <h1 align="center">Restaurant Manager</h1>
</div>
<br />
<p align="center">A project for Visma Case Interview Round 2</p>

<div align="center">
  <a href="#getting-started">Getting started</a> •
  <a href="rm-api/doc/video">View demo</a> •
  <a href="#license">License</a>
</div>

## Project Goals
The main goal of this project is to develop a "Restaurant Manager" RESTful Web API using ASP.NET Core. This API is designed to manage various aspects of restaurant operations, including inventory management, menu management, and customer order processing. The application focuses on providing a scalable and maintainable backend solution that adheres to good software development practices.

### Key Objectives

* **Manage Restaurant Stock:** The API provides endpoints to add, update, remove, and retrieve stock information. This includes managing quantities and types of ingredients available in the restaurant.
* **Manage Restaurant Menu:** The API allows for the creation, updating, deletion, and retrieval of menu items. Each menu item can have associated ingredients that are managed through the inventory.
* **Create and Manage Customer Orders:** The API handles customer orders by creating, updating, and retrieving order information. It also ensures inventory levels are adjusted based on orders placed.
## Table of contents

- [Project structure](#project-structure)
- [Architecture](#architecture)
- [Useful resources](#useful-resources)
- [Getting started](#getting-started)
  - [Development](#development)
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
The architecture of the Restaurant Manager application is designed with a focus on scalability, maintainability, and clean code practices. The application follows a layered architecture pattern, which separates concerns and promotes a clean separation between the different components of the application.

### Components
* **Controllers:** These are the entry points for the API and handle HTTP requests. Each controller corresponds to a specific part of the application, such as MenuItems, Orders, and Products.
* **Services:** Business logic is implemented in service classes. Services are responsible for handling the operations defined by the application requirements, such as adding products to stock or processing an order.
* **Repositories:** These provide an abstraction layer for data access. Repositories interact with the database (SQLite or LocalDB) to perform CRUD operations.
* **Models:** Represent the data structure used throughout the application. This includes both domain models and DTOs (Data Transfer Objects) used to transfer data between layers.
* **Database:** A lightweight database (SQLite) is used for storing data related to products, menu items, and orders. The database schema is designed to efficiently support the operations required by the application.

### Design Principles
* **OOP, SOLID Principles:** The application design follows Object-Oriented Programming principles and SOLID principles to ensure that the system is easy to maintain, extend, and scale.
* **TDD (Test-Driven Development):** The application is developed with a focus on TDD, ensuring a high level of code quality and reliability. Unit tests are written for all major components to ensure functionality is as expected.

### Main Features

| Method   | Description                              |
| -------- | ---------------------------------------- |
| `GET`    | Used to retrieve a single item or a collection of items. |
| `POST`   | Used when creating new items e.g. a new user, post, comment etc. |
| `PATCH`  | Used to update one or more fields on an item e.g. update name of product. |
| `PUT`    | Used to replace a whole item (all fields) with new data. |
| `DELETE` | Used to delete an item.                  |

#### API Endpoints for Stock Management

| Method   | URL                                      | Description                              |
| -------- | ---------------------------------------- | ---------------------------------------- |
| `POST`   | `/api/Products`                          | Add new products to stock.               |
| `PUT`    | `/api/Products/{id]`                     | Update existing stock quantities         |
| `DELETE` | `/api/Products/{id}`                     | Remove products from stock               |
| `GET  `  | `/api/Products`                          | Retrieve current stock information.      |

#### API Endpoints for Menu Management

| Method   | URL                                      | Description                              |
| -------- | ---------------------------------------- | ---------------------------------------- |
| `POST`   | `/api/MenuItems`                         | Add new menu item to stock.              |
| `PUT`    | `/api/MenuItems/{id]`                    | Update existing menu items quantities    |
| `DELETE` | `/api/MenuItems/{id}`                    | Remove menu items from stock             |
| `GET  `  | `/api/MenuItems`                         | Retrieve current menu items information. |

#### API Endpoints for Order Management

| Method   | URL                                      | Description                              |
| -------- | ---------------------------------------- | ---------------------------------------- |
| `POST`   | `/api/Orders`                            | Add new orders to stock.                 |
| `GET  `  | `/api/Orders`                            | Retrieve current orders information.     | 
* Ensure that orders are only accepted if sufficient stock is available; otherwise, they are declined.

### Extra Features
* **Swagger UI Integration:** The API is documented using Swagger, providing an interactive UI to test and explore API endpoints.
* **Automated CI/CD with GitHub Actions:** Continuous Integration and Continuous Deployment (CI/CD) pipelines are set up using GitHub Actions to automate testing.
* **Version Control with GitHub:** The project utilizes GitHub for version control, ensuring a robust workflow for managing changes and collaboration.
* **Kanban Board for Agile Project Management:** The project management follows Agile methodologies, utilizing a GitHub Kanban board to track issues, tasks, and milestones. This allows for iterative development and continuous delivery.
* **UI in Angular**: The project has an frontend for enhancing the user experience, written in Angular.
* **Unit Tests**: Unit Tests with a code coverage of 95.55% and branch coverage of 87%. There is a report of code coverage at [CodeCoverageReport](/rm-api/RestaurantManagerAPI/TestResults/coverage-report/index.html)

#### API Endpoints for Order Management

| Method   | URL                                      | Description                              |
| -------- | ---------------------------------------- | ---------------------------------------- |
| `PUT`    | `/api/Orders/{id]`                       | Update existing orders quantities        |
| `DELETE` | `/api/Orders/{id}`                       | Remove orders from stock                 |

## Useful resources

Some internal resources you may find useful:

## Getting started

First, clone the repository to your computer. This can be done by running the following command:

```sh
git clone https://github.com/ejhasler/rm-api.git
```


<br/>

### Backend
For backend launching i.e., normal launch of the backend. One may use the command:
```sh
dotnet run
```


<br/>

### Test
For test launching, one may use the following command in the folder /rm-api/RestaurantManagerAPI:

```sh
dotnet test
```

### Frontend
For production launching i.e., normal launch of the frontend. One may use the command:

```sh
ng serve
```

<br/>


### Development

In order to get started with development, you will need the following:

- .NET 8.0
- Angular 18.0 (LTS)
- BootStrap 5.0 (LTS)
- Node.Js 18 (LTS)
- SQLite Server or any preferred Database

## License

Distributed under the [MIT License](LICENSE).


