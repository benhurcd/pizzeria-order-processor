# Pizzeria Order Processor

This is a modular, extensible .NET console application for processing pizzeria orders, designed with SOLID principles and dependency injection.

## Features
- Parse orders from JSON files (CSV support can be added via new providers)
- Validate orders (including product existence)
- Calculate total price per order
- Calculate total raw ingredients required
- Display a summary of valid orders and required ingredients
- Easily extensible for new file formats, validation rules, or calculation logic

## Project Structure
- **Core/**: Domain models and interfaces
- **Infrastructure/**: File providers and data access
- **Services/**: Business logic, validation, and calculation
- **ConsoleApp/**: Application entry point and DI setup

## Usage

### Build
```sh
dotnet build
```

### Run
By default, the app looks for `orders.json`, `products.json`, and `ingredients.json` in the application directory.

```sh
dotnet run --project ./ConsoleApp/ConsoleApp.csproj
```

You can also specify custom file paths:
```sh
dotnet run --project ./ConsoleApp/ConsoleApp.csproj -- --orderFile path/to/orders.json --productFile path/to/products.json --ingredientsFile path/to/ingredients.json
```

Short options are also supported:
```sh
dotnet run --project ./ConsoleApp/ConsoleApp.csproj -- -o path/to/orders.json -p path/to/products.json -i path/to/ingredients.json
```

## Running the Project
To run the application with the default sample data:
```sh
dotnet run --project ./ConsoleApp/src/ConsoleApp.csproj
```

To specify custom data files:
```sh
dotnet run --project ./ConsoleApp/src/ConsoleApp.csproj -- --orderFile path/to/orders.json --productFile path/to/products.json --ingredientsFile path/to/ingredients.json
```
Or using short options:
```sh
dotnet run --project ./ConsoleApp/src/ConsoleApp.csproj -- -o path/to/orders.json -p path/to/products.json -i path/to/ingredients.json
```

## Testing
To run all unit and functional tests for every project:
```sh
dotnet test
```
This will build the solution and execute all tests in the `Core.Tests`, `Infrastructure.Tests`, `Services.Tests`, and `ConsoleApp.Tests` projects. All tests should pass if the solution is set up correctly.

## Extensibility
- Add new file formats by implementing `IOrderProvider`, `IProductProvider`, or `IIngredientProvider`.
- Add new validation rules by implementing `IOrderValidator`.
- Add new calculation logic by extending or replacing the calculation services.

## Sample Data
Sample JSON files are provided in the root directory:
- `orders.json`
- `products.json`
- `ingredients.json`

## Requirements
- .NET 6.0 or later

## Design Decisions
- **Layered Architecture:** The solution is split into Core, Infrastructure, Services, and ConsoleApp to enforce separation of concerns and enable independent testing and extensibility.
- **Dependency Injection:** All dependencies are injected, allowing for easy swapping of implementations and facilitating unit testing.
- **SOLID Principles:** Interfaces are used throughout for extensibility and testability. Each class has a single responsibility, and open/closed and dependency inversion principles are followed.
- **Testability:** Each main project has a corresponding test project, and all business logic is covered by unit and functional tests.
- **Extensibility:** New file formats, validation rules, or calculation logic can be added by implementing the relevant interfaces and registering them in DI.

## Architecture Diagram
```
+-------------------+      +---------------------+      +-------------------+      +-------------------+
|   ConsoleApp      | ---> |     Services        | ---> |   Infrastructure  | ---> |      Core         |
| (Entry Point, DI) |      | (Business Logic)    |      | (Data Providers)  |      | (Domain/Contracts)|
+-------------------+      +---------------------+      +-------------------+      +-------------------+
```
- **ConsoleApp**: Handles application startup, argument parsing, and dependency injection setup.
- **Services**: Contains business logic, validation, and calculation services.
- **Infrastructure**: Provides data access and file parsing.
- **Core**: Defines domain models and interfaces.

---
This project is designed for extensibility and clean architecture. Contributions and improvements are welcome!
