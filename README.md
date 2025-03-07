# Hotel Management System

A comprehensive .NET Windows Forms application for hotel management, featuring a user-friendly interface where hotel rooms are represented by customized UserControls.

## Project Overview

This Hotel Management System is designed to simplify room management and enhance user experience with the following features:

- Visual representation of rooms with dynamic status updates
- Date-based room availability management
- Context-specific menus for different room statuses
- Reservation management system
- Guest management
- Room status summary and statistics

## Architecture

The application follows a clean architecture with separation of concerns:

### Core Layer

Contains domain models, interfaces, and business logic:

- Domain entities (Room, RoomType, Reservation, Guest, etc.)
- Interfaces defining the contract for repositories and services
- Service implementations containing business logic
- Events and event handlers

### Data Layer

Handles data access and persistence:

- Entity Framework Core with PostgreSQL
- Repository pattern implementation
- Database context and migrations
- Database seeding and initialization

### UI Layer

Provides the user interface:

- Windows Forms implementation
- Custom UserControls for rooms
- MVVM pattern for UI separation
- Factory pattern for creating room controls
- Command pattern for room operations

## Design Patterns

The application implements several design patterns:

- **Repository Pattern**: Abstracts data access logic
- **MVVM Pattern**: Separates UI from business logic
- **Factory Pattern**: Creates room controls
- **Observer Pattern**: Updates room statuses when dates change
- **Command Pattern**: Encapsulates room operations

## Features

### Room Management

- Visually represent rooms with their current status
- Filter rooms by number, status, or type
- View detailed room information
- Update room status (Available, Occupied, Reserved, Under Maintenance, Cleaning)

### Reservation System

- Create, update, and cancel reservations
- Check room availability for specific dates
- Check-in and check-out process
- Extend guest stays
- Track payments and deposits

### Guest Management

- Maintain guest information
- Track guest history and preferences
- Associate guests with reservations

### Dashboard and Statistics

- Summary panel showing room status distribution
- Occupancy rate calculation
- Quick access to daily check-ins and check-outs

## Database Schema

The system uses a PostgreSQL database with the following main tables:

- **Rooms**: Stores room information including number, type, features
- **RoomTypes**: Defines different room categories
- **RoomStatuses**: Tracks the history of room statuses
- **Guests**: Contains guest information
- **Reservations**: Manages bookings and stays

## Setup Instructions

### Prerequisites

- .NET Framework 4.8 or .NET Core 3.1+
- PostgreSQL database
- Visual Studio 2019+

### Installation

1. Clone the repository
2. Open the solution in Visual Studio
3. Update the connection string in `appsettings.json`
4. Run the following commands in Package Manager Console:
   ```
   Add-Migration InitialCreate -Project HotelManagementSystem.Data
   Update-Database -Project HotelManagementSystem.Data
   ```
5. Build and run the application

### Configuration

The application can be configured by modifying the `appsettings.example.json` file to `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=HotelManagementDB;Username=postgres;Password=yourpassword"
  }
}
```

## Project Structure

```
HotelManagementSystem/
├── HotelManagementSystem.Core/               // Core business logic
│   ├── Models/                               // Domain entities
│   ├── Interfaces/                           // Interfaces for services and repositories
│   ├── Services/                             // Business logic services
│   └── Events/                               // Event handling
├── HotelManagementSystem.Data/               // Data access layer
│   ├── Context/                              // EF Core context
│   ├── Repositories/                         // Repository implementations
│   ├── Migrations/                           // EF Core migrations
│   └── DatabaseInitializer.cs                // Database seeding
├── HotelManagementSystem.UI/                 // User interface layer
│   ├── Forms/                                // Windows Forms
│   ├── Controls/                             // Custom UserControls
│   ├── ViewModels/                           // MVVM implementation
│   ├── Factories/                            // Creation of UI elements
│   ├── Commands/                             // Command pattern implementations
│   └── Utilities/                            // Helper classes
```

## Future Enhancements

The application is designed with scalability in mind, allowing for future enhancements such as:

- Integration with payment systems
- Support for multiple user roles (admin, receptionist)
- Reporting features for occupancy and financials
- Mobile application for remote management
- API for third-party integrations

## Technical Details

### Technologies Used

- C# / .NET
- Windows Forms
- Entity Framework Core
- PostgreSQL
- Microsoft Dependency Injection

### Patterns and Principles

- Clean Architecture
- SOLID principles
- Repository Pattern
- MVVM Pattern
- Command Pattern
- Factory Pattern
- Observer Pattern

## License

This project is licensed under the MIT License.