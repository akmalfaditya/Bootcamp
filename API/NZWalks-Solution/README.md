# NZWalks Solution

## Overview
Welcome to the **NZWalks Solution** - a comprehensive full-stack application designed to manage and showcase walking trails across New Zealand's beautiful regions. This solution demonstrates modern .NET 8 development practices, implementing both a robust Web API and an interactive web application.

## 🏗️ Solution Architecture

This solution consists of two main projects:

### 🔧 **NZWalks.API** - RESTful Web API
A comprehensive ASP.NET Core Web API that serves as the backend for managing New Zealand walking trails, regions, and difficulties.

### 🌐 **NZWalks.UI** - Web Application
An ASP.NET Core MVC web application providing a user-friendly interface for managing regions and interacting with the API.

## 🚀 Key Features

- **Complete CRUD Operations** for Regions, Walks, and Difficulties
- **JWT-based Authentication** with role-based authorization
- **Entity Framework Core** with SQL Server integration
- **AutoMapper** for seamless object mapping
- **Image Upload** functionality with local storage
- **Comprehensive Logging** using Serilog
- **RESTful API Design** with proper HTTP status codes
- **Model Validation** and custom action filters
- **Responsive UI** with Bootstrap styling

## 🛠️ Technology Stack

- **.NET 8.0** - Latest framework features
- **ASP.NET Core** - Web API and MVC
- **Entity Framework Core** - Data access and migrations
- **SQL Server** - Primary database
- **AutoMapper** - Object-to-object mapping
- **JWT Authentication** - Secure API access
- **Serilog** - Structured logging
- **Bootstrap** - Responsive UI framework

## 📊 Domain Model

The application manages three core entities:

1. **Regions** - New Zealand geographical regions (Auckland, Wellington, etc.)
2. **Walks** - Individual walking trails with difficulty levels
3. **Difficulties** - Trail difficulty ratings (Easy, Medium, Hard)
4. **Images** - File uploads for walks and regions

## 🎯 Learning Objectives

This solution serves as an excellent learning resource for:

- **RESTful API Development** best practices
- **Clean Architecture** principles
- **Entity Framework Core** migrations and relationships
- **Authentication & Authorization** implementation
- **Full-stack development** with .NET
- **Modern web development** patterns

## 🚦 Getting Started

1. **Prerequisites**: .NET 8 SDK, SQL Server, Visual Studio 2022
2. **Database Setup**: Update connection strings in `appsettings.json`
3. **Run Migrations**: Use Entity Framework tools to create the database
4. **Start Projects**: Run both API and UI projects simultaneously

## 📁 Project Structure

```
NZWalks-Solution/
├── NZWalks.API/          # Backend Web API
├── NZWalks.UI/           # Frontend Web Application
└── README.md             # This file
```

Each project contains its own detailed README with specific setup instructions and implementation details.


