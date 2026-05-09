# 🏥 MediConnect Mobile App

Welcome to the **Frontend** repository of **MediConnect**. This mobile application is built using **Flutter** to provide a seamless and premium user experience on mobile devices, bridging the gap between patients, doctors, receptionists, and hospital administration in one unified healthcare platform.

---

## ✨ Features

The application is designed to support a robust **Role-Based Access Control (RBAC)** system. The user interface and available features adapt dynamically based on the logged-in account type:

### 1. 🩺 Patient Portal
* **Advanced Registration:** Create a comprehensive medical profile including physical indices (height, weight) and blood type.
* **Smart Search:** Find and filter doctors by specialization, average reviews, or consultation fees.
* **Appointment Booking:** Easily select available time slots through an interactive calendar interface.
* **Medical Records:** Track appointments (upcoming and past) and review previous medical diagnoses.

### 2. 🥼 Doctor Panel
* **Appointment Management:** View today's schedule and accept, reject, or reschedule booking requests.
* **Clinic Management:** Update personal profile, consultation fees, and available working hours.

### 3. 💁 Receptionist Dashboard
* **QR Scanner:** Instantly confirm patient attendance at the clinic by scanning their booking QR code.
* **Payment Management:** Process and log payments (Cash/Card) upon patient arrival to finalize appointments.

### 4. 🛡️ Admin Suite
* **Advanced Analytics:** A comprehensive dashboard displaying live statistics and charts for patients, doctors, and revenue.
* **System Management:** Add and update accounts for doctors and receptionists, and manage medical specializations across the platform.

---

## 🛠️ Tech Stack & Libraries

* **Framework:** Flutter
* **Programming Language:** Dart
* **Security & Session Management:** 
  * `flutter_secure_storage` for securely storing JWT credentials.
  * Custom network Interceptor to automatically handle token expiration and refresh tokens in the background without interrupting the user.
* **Networking:** `http` package for communicating with the remote RESTful API.
* **UI/UX Design:** 
  * Advanced theme management system supporting Light and Dark Modes.
  * Premium skeleton loading animations (`shimmer`).
* **Utilities:** `mobile_scanner` for QR code processing, `connectivity_plus` for live network monitoring.

---

## 🚀 Getting Started

Follow these steps to run the application on your local machine:

### Prerequisites
* Install the [Flutter SDK](https://docs.flutter.dev/get-started/install) (version 3.10.0 or higher recommended).
* Install Android Studio or Xcode to run an emulator/simulator.

### Running the App
1. Open your terminal and navigate to the project directory: `mediconnect_flutter`.
2. Fetch the required dependencies:
   ```bash
   flutter pub get
   ```
3. Run the application on an emulator or a connected physical device:
   ```bash
   flutter run
   ```

---

## 👥 Frontend Team

The MediConnect Mobile Client was developed by:
* **Moaaz Ali**
* **Mustafa Amr**
* **Ahmed Gohar**

> *Note: This repository is dedicated to the Flutter mobile client. The Backend server is maintained in its own separate directory.*
