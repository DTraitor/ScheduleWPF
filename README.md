# University Schedule Viewer and Editor

## Overview
The **University Schedule Manasger** app is a desktop application built to facilitate the viewing, management, and editing of university schedules for students and faculty members. It provides an intuitive and user-friendly interface, allowing users to easily interact with class schedules, course lists, and faculty assignments.

The application is built using modern technologies, including **.NET Core 8**, **WPF (Windows Presentation Foundation)** for the UI, **ReactiveUI** for responsive interactions, and adheres to the **MVVM (Model-View-ViewModel)** architectural pattern for maintainable and testable code.

## Features
- **View University Schedule**: Browse class schedules by day, week, or month.
- **Edit Schedules**: Modify, add, or remove class entries, subjects, and faculty assignments.
- **Reactive Updates**: Real-time updates to the schedule, providing instant feedback for any changes made.

## Technology Stack
- **.NET Core 8**: Backend logic and API integration.
- **WPF (Windows Presentation Foundation)**: Frontend UI framework, offering rich desktop experience.
- **ReactiveUI**: Reactive extensions for handling complex asynchronous events in a clear and concise manner.
- **MVVM (Model-View-ViewModel)**: A clean architecture ensuring separation of concerns, allowing easy maintenance and testing.

## Installation and Setup

1. **Prerequisites**:
    - Install [.NET Core 8 SDK](https://dotnet.microsoft.com/download/dotnet-core/8.0)
    - Ensure that Visual Studio (or another compatible IDE) is installed with WPF and .NET Core support.

2. **Clone the Repository**:
   ```bash
   git clone https://github.com/your-repo/university-schedule-editor.git
   ```

3. **Build and Run**:
    - Open the solution file (`UniversityScheduleEditor.sln`) in Visual Studio.
    - Restore NuGet packages:
      ```bash
      dotnet restore
      ```
    - Build the solution:
      ```bash
      dotnet build
      ```
    - Run the application:
      ```bash
      dotnet run
      ```

## License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

