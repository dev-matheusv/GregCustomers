# ⚙️ GregCustomers

[![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat\&logo=dotnet)](https://dotnet.microsoft.com/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2016+-CC2927?style=flat\&logo=microsoftsqlserver)](https://www.microsoft.com/sql-server)
[![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?style=flat\&logo=docker)](https://www.docker.com/)

> Customer management system built with **.NET 8**, applying **CQRS**, **Dapper**, **Entity Framework Core** and **SQL Server**, focusing on performance and data consistency.

---

## 📌 Overview

GregCustomers is a backend-focused application designed to manage customers and their addresses, with emphasis on **performance**, **data integrity** and **clean architecture principles**.

This project demonstrates a hybrid persistence approach, combining the strengths of ORMs and direct SQL execution.

---

## 🧠 Key Concepts

* CQRS (Command Query Responsibility Segregation)
* Hybrid data access (Dapper + EF Core)
* Stored procedures for write operations
* Separation of concerns
* Performance-oriented design

---

## 🧱 Architecture

```id="arch03"
- Domain
- Application (Commands / Queries)
- Infrastructure (EF Core + Dapper)
- API
- Web MVC (Razor)
```

---

## ⚙️ Tech Stack

* .NET 8 / ASP.NET Core
* ASP.NET Core MVC (Razor)
* SQL Server
* Dapper (writes)
* Entity Framework Core (reads)
* MediatR (CQRS)
* Docker / Docker Compose

---

## 🔑 Features

### Customer Management

* Create, update, delete and query customers
* Unique email validation
* Logo upload and retrieval (stored in database)

### Address Management

* Multiple addresses per customer
* Duplicate address prevention per customer

### Security

* JWT authentication
* Role-based authorization (admin / reader)

---

## 🧠 Business Rules

* Customers cannot have duplicated emails
* Addresses must be unique per customer
* Write operations are executed via stored procedures
* Read operations are optimized using EF Core

---

## ⚡ Performance Considerations

* Hybrid persistence (Dapper for writes, EF Core for reads)
* Stored procedures for critical operations
* Indexed columns (email, addresses)
* Pagination for large datasets
* Use of `AsNoTracking` for read queries

---

## ⚙️ Setup & Running Locally

### Requirements

* Docker
* .NET 8 SDK

---

### 1. Start SQL Server

```bash id="greg01"
docker compose up -d
```

---

### 2. Configure database

* Host: `localhost`
* Port: `1433`
* User: `sa`
* Password: `GregCustomer@123`
* Database: `GregCustomersDb`

---

### 3. Run database scripts

Execute in order:

1. `database/scripts/001_create_tables.sql`
2. `database/scripts/002_stored_procs.sql`

---

### 4. Configure JWT

File:

```
src/GregCustomers.Api/appsettings.Development.json
```

```json id="greg02"
"Jwt": {
  "Issuer": "GregCustomers",
  "Audience": "GregCustomers",
  "Key": "YOUR_SECRET_KEY_WITH_32+_CHARACTERS",
  "ExpiresMinutes": 60
}
```

---

### 5. Run API

```bash id="greg03"
cd src/GregCustomers.Api
dotnet run
```

Swagger:

```
http://localhost:5181/swagger
```

---

### 6. Run MVC application

```bash id="greg04"
cd src/GregCustomers.WebMvc
dotnet run
```

```
http://localhost:5161
```

---

## 📡 API Highlights

### Authentication

```
POST /api/auth/login
```

### Customers

```
GET /api/clients
POST /api/clients
PUT /api/clients/{id}
DELETE /api/clients/{id}
```

---

## 🧠 What this project demonstrates

* Enterprise-level backend architecture
* CQRS implementation in practice
* Hybrid data access strategy
* Performance-oriented design
* Secure API with authentication and authorization

---

## 📄 Additional Documentation

* `docs/api.md`
* `docs/architecture.md`
* `docs/decisions.md`

---

## 📄 License

Project developed for technical evaluation purposes.
