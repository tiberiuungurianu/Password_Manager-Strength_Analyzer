# 🔐 Fortress Password Manager & Strength Analyzer

Enterprise-level password management web application built with ASP.NET Core MVC, featuring comprehensive security framework and real-time threat detection.

## 📋 Table of Contents
- [Overview](#overview)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Installation](#installation)
- [Usage](#usage)
- [Testing](#testing)
- [Contributing](#contributing)

## 🎯 Overview

Fortress is a sophisticated password manager and strength analyzer designed to help users securely store credentials while providing real-time security insights. Developed as academic team project for CS6004 Application Development, achieving First Class standard through comprehensive security implementation.

## ✨ Features

### 🔒 Security Framework
- ASP.NET Core Identity with two-factor authentication
- Role-based access control (Admin/User roles)
- Server-side password encryption with secure storage
- Anti-forgery protection throughout application

### 📊 Password Management
- Automated strength evaluation (Weak/Medium/Strong/Very Strong)
- Real-time security dashboard with analytics
- Automated alerts for weak/reused passwords
- Advanced tagging system for password categorization

### 🔍 Advanced Features
- Vulnerability detection systems
- Duplicate password prevention
- Complex many-to-many database relationships
- Optimized caching strategies for performance

## 🛠️ Tech Stack

**Backend:**
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- C#

**Testing:**
- xUnit Framework
- Moq for mocking
- High code coverage across ViewModels and Controllers

**Architecture:**
- Model-View-Controller (MVC) pattern
- Service layer separation
- Repository pattern implementation

## 🚀 Installation

### Prerequisites
- Visual Studio 2022 (or later) with ASP.NET and web development workload
- .NET SDK
- SQL Server LocalDB

### Steps
1. Clone the repository
```bash
git clone https://github.com/yourusername/fortress-password-manager.git
cd fortress-password-manager
