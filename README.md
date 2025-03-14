# Task-Management

## Overview

This is a **RESTful API** for managing projects and tasks. The API allows users to perform CRUD operations on projects and tasks, with authentication and authorization handled by **AWS Cognito**.

## Features

- **Project Management**
  - Create, update, delete, and retrieve projects.
  - Pagination support for retrieving projects.
- **Task Management**
  - Create, update, delete, and retrieve tasks within a project.
  - Pagination support for retrieving tasks.
- **Authentication & Authorization**
  - Secured with **AWS Cognito**.
  - Role-based access control.
- **Logging & Error Handling**
  - Basic logging using `ILogger`.
  - Error handling for invalid input and missing resources.

## Technologies Used

- **C#** *(ASP.NET Core)*
- **.NET Core**
- **AWS Cognito** *(for authentication)*
- **Logging** (`ILogger`)
- **Unit Testing** (`NUnit`)

## Endpoints

### **Projects**

- `GET /api/projects?page={page}&size={size}` - Retrieve a paginated list of projects.
- `GET /api/projects/{projectId}` - Retrieve a single project by ID.
- `POST /api/projects` - Create a new project.
- `PUT /api/projects/{projectId}` - Update an existing project.
- `DELETE /api/projects/{projectId}` - Delete a project.

### **Tasks**

- `GET /api/projects/{projectId}/tasks?page={page}&size={size}` - Retrieve a paginated list of tasks within a project.
- `GET /api/projects/{projectId}/tasks/{taskId}` - Retrieve a single task by ID.
- `POST /api/projects/{projectId}/tasks` - Create a new task within a project.
- `PUT /api/projects/{projectId}/tasks/{taskId}` - Update an existing task.
- `DELETE /api/projects/{projectId}/tasks/{taskId}` - Delete a task.

## Authentication

The API is secured with **AWS Cognito**. Users must include a valid **JWT token** in the `Authorization` header for all requests.

