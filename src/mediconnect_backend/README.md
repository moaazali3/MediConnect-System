# MediConnect System Backend

MediConnect is a comprehensive Hospital Appointment & Patient Management System backend built with **ASP.NET Core (net10.0)**. It follows the principles of **Clean Architecture** to ensure maintainability, scalability, and loose coupling of components.

## 🚀 Technologies Used
*   **Framework:** .NET 10.0 (ASP.NET Core Web API)
*   **Database ORM:** Entity Framework Core 10.0.6
*   **Database:** Microsoft SQL Server (LocalDB for development)
*   **Authentication:** JWT (JSON Web Tokens)
*   **Validation:** FluentValidation
*   **API Documentation:** Swagger / OpenAPI
*   **Email Service:** SendGrid

## 🏗 Architecture Layers

The solution is divided into four main layers following Clean Architecture principles:

1.  **`Hospital.Domain`** (Core Layer)
    *   Contains the enterprise logic and core business models (Entities).
    *   Holds the interfaces for Repositories and other external contracts.
    *   No external dependencies.
    *   *Key Entities:* `AppUser`, `Patient`, `Doctor`, `Appointment`, `MedicalRecord`, `Payment`, `Specialization`, etc.

2.  **`Domain.Application`** (Use Cases / Services Layer)
    *   Contains the business logic and application-specific use cases.
    *   Implements Data Transfer Objects (DTOs) and Validation logic using `FluentValidation`.
    *   Depends only on the `Domain` layer.

3.  **`Hospital.Infrastructure`** (External Connections Layer)
    *   Implements the interfaces defined in the `Domain` layer.
    *   Contains the Entity Framework Core `DbContext` and Repository implementations.
    *   Handles database migrations.
    *   Integrates with external services like SendGrid for sending emails.

4.  **`Hospital.API`** (Presentation / API Layer)
    *   The entry point of the application.
    *   Contains RESTful API Controllers handling HTTP requests and responses.
    *   Configures Dependency Injection, Middleware, JWT Authentication, and Swagger.
    *   *Controllers Include:* `AuthController`, `AppointmentController`, `DoctorController`, `AdminController`, `MedicalRecordController`, etc.

## 💡 Key Features
*   **Authentication & Authorization:** Secure user registration and login with JWT tokens and Refresh Tokens. Role-based access control (Admin, Doctor, Patient, Receptionist).
*   **Doctor & Specialization Management:** Admins can manage doctors, their specializations, and schedules.
*   **Appointment Booking:** Patients can book appointments based on doctor availability.
*   **Medical Records:** Doctors can manage and update patient medical records.
*   **Payments:** Integration for handling appointment payments.
*   **Email Notifications:** Sending alerts and notifications via SendGrid.

## ⚙️ Setup and Installation

### Prerequisites
*   [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
*   Microsoft SQL Server (or SQL Server Express / LocalDB)
*   Visual Studio 2022 or Visual Studio Code

### Getting Started

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/YousefX04/mediconnect-system.git
    cd mediconnect-system/src/mediconnect_backend
    ```

2.  **Configuration:**
    *   Navigate to `Hospital.API/appsettings.json` (or `appsettings.Development.json`).
    *   Update the `ConnectionStrings:DefaultConnection` to match your SQL Server instance.
    *   Update the `JWT` configurations if necessary.
    *   Set up your `SendGridSettings` with a valid API Key and Sender Email to enable email functionalities.

3.  **Apply Database Migrations:**
    Using the .NET CLI:
    ```bash
    dotnet ef database update --project Hospital.Infrastructure --startup-project Hospital.API
    ```

4.  **Run the Application:**
    ```bash
    cd Hospital.API
    dotnet run
    ```

5.  **Access Swagger UI:**
    Open your browser and navigate to the local HTTPS URL (e.g., `https://localhost:<port>/swagger`) to view and interact with the API documentation.
