# 🏥 MediConnect: Comprehensive Healthcare Management Platform

![Flutter](https://img.shields.io/badge/Flutter-%5E3.10.7-02569B?style=for-the-badge&logo=flutter&logoColor=white)
![.NET Core](https://img.shields.io/badge/.NET_Core-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-Latest-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![Platform](https://img.shields.io/badge/Platform-Android%20%7C%20iOS-lightgrey?style=for-the-badge)

Welcome to the **MediConnect** ecosystem! This repository houses the complete source code for a modern, highly scalable, and full-featured healthcare platform designed to streamline hospital operations, patient-doctor interactions, and clinic administration. 

The MediConnect system is built with a clear separation of concerns, divided into a **Flutter Mobile Application (Frontend)** and an **ASP.NET Core RESTful API (Backend)**.

---

## 📑 Table of Contents
- [Overview](#-overview)
- [Platform Modules & Features](#-platform-modules--features)
  - [1. Patient Portal](#1--patient-portal)
  - [2. Doctor Panel](#2--doctor-panel)
  - [3. Receptionist Dashboard](#3--receptionist-dashboard)
  - [4. Admin Command Center](#4--admin-command-center)
- [System Architecture](#-system-architecture)
  - [Backend Architecture](#backend-architecture)
  - [Frontend Architecture](#frontend-architecture)
- [Technology Stack](#️-technology-stack)
- [Security & Authentication Flow](#-security--authentication-flow)
- [Installation & Setup Guide](#-installation--setup-guide)
  - [Running the Backend API](#1-running-the-backend-api)
  - [Running the Mobile Application](#2-running-the-mobile-application)
- [Development Team](#-development-team)

---

## 🌟 Overview

MediConnect aims to digitize the traditional hospital appointment system. It provides distinct, tailor-made interfaces for different actors (Patients, Doctors, Receptionists, Admins) using **Role-Based Access Control (RBAC)**. The backend handles complex business logic, database migrations, and email notifications, while the frontend delivers a premium, fast, and secure mobile experience.

The project was initially developed using two separate repositories — one dedicated to the ASP.NET Core back-end API and another for the Flutter mobile application. After completing development and stabilization, both repositories were unified into a single integrated system for streamlined deployment and maintenance.
Back-End Repository: (https://github.com/YousefX04/mediconnect-system)
Flutter Repository: (https://github.com/moaazali3/mediconnect)

---

## ✨ Platform Modules & Features

### 1. 🩺 Patient Portal
The patient module provides a smooth and user-friendly healthcare experience.

**Features:**
- Patient registration and secure authentication
- Browse doctors and filter by specialization
- View doctor profiles and consultation details
- Browse available doctor schedules
- Book appointments dynamically based on available slots
- Choose preferred payment method:
  - Pay at hospital
  - Visa
  - Wallet
- View appointment history and upcoming appointments
- Manage and update personal profile

---

### 2. 🥼 Doctor Panel
Doctors can efficiently manage appointments and patient records.

**Features:**
- View patients who booked appointments
- Manage daily appointments and schedules
- Create and manage patient medical records
- Upload and update profile picture
- Manage and update professional profile

---

### 3. 💁 Receptionist Dashboard
Receptionists assist doctors in organizing appointments and patient flow.

**Features:**
- Assigned to a specific doctor
- View all appointments related to the assigned doctor
- Update appointment status:
  - Complete appointment
  - Cancel appointment
- Manage and update personal profile

---

### 4. 🛡️ Admin Command Center
The admin panel provides full control over the entire system.

**Features:**
- Create doctor accounts
- Create receptionist accounts
- Manage medical specializations
- View analytics and dashboard statistics
- Monitor system activity and operations

---

## 🏛️ System Architecture

### Backend Architecture
The backend is built following **Clean Architecture** principles, enforcing loose coupling and high testability across four main layers:
1. **`Hospital.Domain` (Core):** Contains enterprise logic, entities (`AppUser`, `Patient`, `Appointment`, etc.), and repository interfaces. No external dependencies.
2. **`Domain.Application` (Use Cases):** Contains business logic, DTOs, and input validation using `FluentValidation`.
3. **`Hospital.Infrastructure`:** Implements data access using Entity Framework Core, database migrations, and external integrations like SendGrid.
4. **`Hospital.API` (Presentation):** RESTful controllers, Dependency Injection, Swagger configuration, and JWT Middleware.

### Frontend Architecture
The mobile client focuses on responsiveness, security, and offline awareness:
* **Dynamic Routing:** Host page dynamically renders the correct dashboard based on the logged-in user's role.
* **Persistent Theming:** A globally accessible state-driven service that switches between customized Light and Dark modes.
* **Modular APIs:** Network requests are separated into specific API service classes (`auth_api.dart`, `appointment_api.dart`).

---

## 🛠️ Technology Stack

### 📱 Frontend (Mobile)
* **Framework:** Flutter (Dart)
* **Networking:** `http` with custom interceptors & `jwt_decoder` for token management.
* **Storage:** `flutter_secure_storage` (Encrypted JWTs) & `shared_preferences` (Theme states).
* **Hardware Integrations:** `mobile_scanner` & `qr_flutter` for robust QR code validation & generation.
* **UI/UX & Media:** Custom theme extensions, `shimmer` & `skeletonizer` for beautiful loading states, and `image_cropper` for profile picture adjustments.

### 💻 Backend (Server)
* **Framework:** ASP.NET Core Web API (.NET 10.0)
* **Database:** Microsoft SQL Server (LocalDB / Express)
* **ORM:** Entity Framework Core 10.0.6
* **Validation:** FluentValidation
* **Documentation:** Swagger / OpenAPI
* **Third-Party Services:** SendGrid (Email Notifications)

---

## 🔐 Security & Authentication Flow

MediConnect utilizes a highly secure, background-managed authentication system:
1. **JWT & Refresh Tokens:** Upon login, the API issues a short-lived access token and a long-lived refresh token.
2. **Auto-Refresh Interceptor:** The Flutter frontend uses an `AuthenticatedClient` that wraps standard HTTP requests. If the API returns a `401 Unauthorized`, the client intercepts it, automatically requests a new token using the refresh token, and replays the failed request transparently.
3. **Role-Based Authorization:** Endpoints are protected via .NET authorization policies, ensuring only valid roles (e.g., `[Authorize(Roles = "Admin")]`) can execute specific commands.

---

## 🚀 Installation & Setup Guide

### Prerequisites
* [Flutter SDK](https://docs.flutter.dev/get-started/install) (v3.10.0+)
* [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
* SQL Server (LocalDB or full instance)
* IDE: Visual Studio 2022, VS Code, or Android Studio.

---

### 1. Running the Backend API

1. **Navigate to the Backend Directory:**
   ```bash
   cd src/mediconnect_backend
   ```
2. **Configure Database & Services:**
   * Open `Hospital.API/appsettings.json`.
   * Update `ConnectionStrings:DefaultConnection` to match your SQL Server instance.
   * Add a valid API Key to `SendGridSettings` if you want to test email notifications.
3. **Apply Entity Framework Migrations:**
   ```bash
   dotnet ef database update --project Hospital.Infrastructure --startup-project Hospital.API
   ```
4. **Run the Server:**
   ```bash
   cd Hospital.API
   dotnet run
   ```
5. **Test via Swagger:** Open your browser and navigate to the provided HTTPS local URL (e.g., `https://localhost:<port>/swagger`).

---

### 2. Running the Mobile Application

1. **Navigate to the Frontend Directory:**
   ```bash
   cd src/mediconnect_flutter
   ```
2. **Install Flutter Dependencies:**
   ```bash
   flutter pub get
   ```
3. **Configure API Endpoints:** Ensure that `lib/constants/api_constants.dart` points to your backend's local IP address (e.g., `10.0.2.2` for Android Emulator or your machine's IPv4 address for physical devices).
4. **Run the App:**
   Launch your emulator or connect a device, then run:
   ```bash
   flutter run
   ```

---

## 👥 Development Team

MediConnect is proudly built by a multi-disciplinary team:

**Mobile Client (Flutter) & Systems Integration:**
* **Moaaz Ali** *(API Integration & Core Logic)*
* **Mustafa Amr** *(UI/UX & Flutter Implementation)*
* **Ahmed Gohar** *(UI/UX & Flutter Implementation)*

**Backend Architecture & API (.NET Core):**
* **Youssef Ahmed**
