<div align="center">
  <h1 align="center">Technical Documentation</h1>
</div>
<br />

## Overview
The backend is implemented as a RESTful API using ASP.NET Core, with SQLite as the database for data persistence. The frontend is developed using Angular, styled with Bootstrap, and managed with npm, and it interacts with the backend API through HTTP requests.

This document provides an in-depth look at the technical strategies, architecture, and design choices made during the development of the project.

## Backend API
### Architecture and Design
The backend of the Restaurant Manager application follows a layered architecture pattern, which promotes separation of concerns and enhances maintainability and scalability. The key components of the backend architecture include Controllers, Services, Repositories, Models, DTOs, and Data.

#### Key Components
1. **Controllers**
Controllers are the entry points for HTTP requests to the API. Each controller is responsible for handling requests related to a specific domain or feature of the application, such as managing menu items, processing orders, or handling product inventory.
* **Responsibilities**: Controllers handle incoming HTTP requests, validate input data, invoke the appropriate service methods, and return responses to the client. They ensure that the correct HTTP status codes are returned based on the success or failure of the requested operations.

2. **DTOs (Data Transfer Object)**
DTOs are used to transfer data between the client and the server while decoupling the internal domain models from the exposed API models. They help to enforce strict validation rules using data annotations, ensuring that only valid data is processed by the API.

* **Purpose:** DTOs provide a structured format for data exchange and prevent over-posting attacks by exposing only the necessary fields required for each operation.
* **Validation:** Data annotations are used within DTOs to enforce rules such as required fields, string length, and value ranges. This ensures data integrity and reduces the risk of errors when processing client requests.

3. **Services**
Service classes implement the business logic of the application and act as an intermediary between the Controllers and Repositories. Services are responsible for performing operations such as adding a new menu item, calculating inventory levels, or processing an order.

** **Responsibilities:**Services encapsulate business rules and logic, ensuring that all operations adhere to the application’s requirements. They handle complex logic and coordinate actions between different components (e.g., updating inventory when an order is placed).
 * **Interface-based Design:** Services are designed using interfaces, allowing for easier testing and mocking. This approach also promotes the Dependency Inversion Principle, one of the SOLID principles, enhancing the flexibility and maintainability of the code.

4. **Repositories**
Repositories provide a data access layer, abstracting the database operations required to interact with SQLite. They perform CRUD operations on the database and are responsible for constructing queries and handling database connections.

* **Responsibilities:** Repositories interact directly with the database context (DataDbContext) to perform data access operations. This includes creating, reading, updating, and deleting records from the database.
* **Abstraction:** The repository pattern abstracts data access logic, making the codebase easier to maintain and test. By isolating data access code, changes to the database schema or access methods have minimal impact on other parts of the application.

5. **DataDbContext**
DataDbContext is the primary class responsible for interacting with the SQLite database using Entity Framework Core. It represents a session with the database, allowing for querying and saving data.

* **Configuration:** DataDbContext is configured to represent the application’s data model. It includes DbSet properties for each entity in the application, such as MenuItems, Orders, and Products.
* **Migration and Updates:** DataDbContext handles database migrations and schema updates, ensuring the database structure remains in sync with the application models.

6. **Models**
Models represent the core data structures used throughout the application. These include domain models that represent entities like MenuItems, Orders, and Products.

* **Purpose:** Models are used to define the structure of data stored in the database and manipulated by the application. They form the foundation of the application’s data layer and are mapped to database tables via Entity Framework Core.

### Database Communication
The backend communicates with a SQLite database to persist and retrieve data. SQLite is chosen for its lightweight nature and ease of integration with ASP.NET Core, making it an ideal choice for this project.

* **Data Access:** The application uses Entity Framework Core to handle all interactions with the SQLite database. This includes performing CRUD operations, managing transactions, and enforcing data integrity.
* **Database Schema:** The schema is designed to efficiently support the operations required by the application. It includes tables for Products, MenuItems, and Orders, with relationships defined by foreign keys where necessary.

### Code Documentation and Best Practices
The project follows best practices in coding and design, adhering to principles such as SOLID, DRY (Don't Repeat Yourself), and KISS (Keep It Simple, Stupid).

* **Code Documentation:** The codebase is thoroughly documented using XML comments and inline comments to describe the purpose and functionality of classes, methods, and properties. This enhances readability and maintainability, making it easier for developers to understand and extend the code.
* **Validation with Data Annotations:** Validation is implemented using data annotations within DTOs, ensuring that incoming data is validated against specific rules before being processed by the application

## Frontend
The frontend of the Restaurant Manager application is built using Angular, with Bootstrap for styling and npm for package management. It serves as the administrative interface for managing restaurant operations, interacting with the backend API through HTTP requests.

![image](https://github.com/user-attachments/assets/d5598fec-7a84-43a8-9b1e-7ceb677a8d1b)


### Key Features
* **Angular Framework:** The frontend is developed using Angular, a popular framework for building single-page applications. Angular provides a robust structure for building dynamic and interactive user interfaces.
* **HTTP Client Module:** Angular's HTTP Client module is used to interact with the backend API, making HTTP requests to perform operations such as retrieving menu items, placing orders, and managing inventory.
* **Bootstrap and SCSS:** The frontend uses Bootstrap for responsive design and SCSS for styling, providing a clean and modern user interface that is easy to navigate.
* **Component-Based Architecture:** The frontend is structured using Angular’s component-based architecture, promoting reusability and modularity. Each feature of the application is encapsulated within its own component, such as MenuItemsComponent, OrdersComponent, and ProductsComponent.


### Communication with Backend API
The frontend communicates with the backend API through a series of HTTP requests. These requests are managed using Angular’s HTTP Client, allowing for seamless integration between the frontend and backend.

* **GET Requests:** Retrieve data from the backend, such as fetching all menu items or retrieving a specific order.
* **POST Requests:** Send data to the backend to create new records, such as adding a new menu item or placing a new order.
* **PUT Requests:** Update existing data on the backend, such as modifying an existing product’s details.
* **DELETE Requests:** Remove records from the backend, such as deleting a menu item or removing a product from inventory.

## Conclusion
The Restaurant Manager application is a well-architected, full-stack solution that demonstrates the effective use of modern development practices. The backend RESTful API, built with ASP.NET Core and SQLite, provides a robust and scalable foundation for managing restaurant operations. The Angular frontend complements this by offering a user-friendly interface for administrators.

