<div align="center">
  <h1 align="center">Future Work</h1>
</div>
<br />

To enhance the functionality, security, and scalability of the Restaurant Manager application, several improvements are planned:

1. **Implement Authentication and Authorization with OAuth2 and Azure AD:**
  * **Reason:** To ensure secure access to the API, it's important to implement robust authentication and authorization mechanisms. Integrating OAuth2 with Azure Active Directory will provide secure, scalable user management, including features like single sign-on (SSO) and multi-factor authentication (MFA), ensuring that only authorized users can access specific parts of the system.

2. **Migrate to Azure Cosmos DB:**
  * **Reason:** Moving from SQLite to Azure Cosmos DB will enhance the application's scalability and performance. Cosmos DB offers global distribution, low-latency access, and automatic scaling, which are essential for handling larger datasets and increasing traffic as the application grows.

4. **Strengthen Security to Prevent SQL Injections and Protect API Endpoints:**
  * **Reason:** Security is a critical concern for any web application. Preventing SQL injections through parameterized queries and input validation will help protect against common attacks. Additionally, implementing security measures such as HTTPS, rate limiting, and monitoring will safeguard the API from unauthorized access and abuse.

6. **Refactor and Optimize the Frontend:**
  * **Reason:** To improve maintainability and performance, refactoring the Angular frontend using better coding practices and design patterns will make the codebase more scalable and easier to manage. This includes optimizing for faster load times and enhancing responsiveness for a better user experience.

These enhancements will ensure that the Restaurant Manager application is secure, scalable, and well-positioned for future growth and feature development.
