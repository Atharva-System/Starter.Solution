# Starter.Solution

Starter.Solution is a starter project designed to kickstart your development process with a robust backend API and multiple frontend options. It leverages various technologies and design patterns to provide a scalable and maintainable foundation for your application.

## Backend (.Net API 8)

### Technologies Used

- **.Net API 8:** The backend is built using .Net API 8, taking advantage of its features and performance improvements.
- **Clean Architecture:** The project follows the clean architecture principles, promoting separation of concerns and maintainability.
- **Repository Pattern:** Utilizes the repository pattern for data access, enhancing data abstraction and testability.
- **CQRS Pattern with Mediator:** Implements the CQRS pattern using MediatR library, providing a clear separation of command and query responsibilities.
- **Caching:** Incorporates caching mechanisms to optimize performance and reduce redundant data fetches.
- **JWT Authorization:** Ensures secure authentication and authorization using JSON Web Tokens (JWT).

### Getting Started

1. Clone the repository: `git clone https://github.com/Atharva-System/Starter.Solution.git`
2. Navigate to the backend folder: `cd Starter.Solution/src/API/Starter.API`
3. Run the backend API: `dotnet run`

## Frontend

Starter.Solution provides multiple frontend options to cater to different preferences.

### 1. Blazor WebAssembly

#### Technologies Used

- **Blazor WebAssembly:** The Blazor framework for building interactive web applications.
- **Consumes .Net API 8:** Interacts seamlessly with the backend API for data retrieval and manipulation.

#### Getting Started

1. Navigate to the Blazor WebAssembly folder: `cd Starter.Solution/src/UI/Blazor`
2. Run the application: `dotnet run`

### 2. Angular

#### Technologies Used

- **Angular:** A popular frontend framework for building dynamic web applications.

#### Getting Started

1. Navigate to the Angular folder: `cd Starter.Solution/src/UI/Angular`
2. Install dependencies: `npm install`
3. Run the application: `ng serve`

### 3. Vue

#### Technologies Used

- **Vue:** A progressive JavaScript framework for building user interfaces.

#### Getting Started

1. Navigate to the Vue folder: `cd Starter.Solution/src/UI/Vue`
2. Install dependencies: `npm install`
3. Run the application: `npm run dev`

## Contributing

Feel free to contribute to this project by opening issues or pull requests. Your feedback and enhancements are welcomed!

## License

This project is licensed under the [MIT License](LICENSE).
