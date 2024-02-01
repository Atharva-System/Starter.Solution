# Starter.Solution

Starter.Solution is a starter project designed to kickstart your development process with a powerful solution template that exemplifies the principles of [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) and incorporates the robustness of CQRS implementation using ASP.NET Core. It leverages various technologies and design patterns to provide a scalable and maintainable foundation for your application with **multiple frontend options**

![CleanArchitecture](https://user-images.githubusercontent.com/42376112/110762993-a61b1580-8266-11eb-9ac1-438072319971.jpg)

## Backend (.Net API 8)

## Technologies at Play:

* ASP.NET Core
* Entity Framework Core
* MediatR
* Swagger
* Redis (for distributed caching)
* Jwt Token Authentication
* Asp.Net Identity
* Api Versioning
* FluentValidation
* Serilog
* Mapper
* Docker
* xUnit

## Championing Best Practices and Design Principles:

* Clean Architecture
* Clean Code
* CQRS
* Authentication and Authorization
* Distributed caching
* SOLID Principles
* DbContext and Repository (with Generic Repository)
* REST API Naming Conventions
* Multi-environment Utilization in ASP.NET Core (Development, Docker, etc.)
* Custom Exceptions
* Unit Tests
* PipelineBehavior for Validation and Performance Tracking.

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

### 4. React

#### Technologies Used

- **React:** A progressive JavaScript library for building user interfaces.

#### Getting Started

1. Navigate to the React folder: `cd Starter.Solution/src/UI/React`
2. Install dependencies: `npm install`
3. Run the application: `npm run dev`

## Contributing

Feel free to contribute to this project by opening issues or pull requests. Your feedback and enhancements are welcomed!

## License

This project is licensed under the [MIT License](LICENSE).
