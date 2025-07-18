# Expense Tracker Application - Detailed Setup and Run Instructions

This document provides detailed steps to set up and run the Expense Tracker application, including the Angular frontend, .NET API backend, and connecting the API to a MySQL database hosted on AWS RDS.

---

## Prerequisites

- Node.js and npm installed (for Angular frontend)
- .NET 6 SDK or later installed (for .NET API)
- AWS account with RDS MySQL instance created
- MySQL client or tools for database management (optional)

---

## 1. Setting up the Angular Frontend

### Navigate to the frontend directory

```bash
cd src
```

### Install dependencies

```bash
npm install
```

### Update API URL

- Open `src/environments/environment.ts` and `src/environments/environment.prod.ts`
- Set the `apiUrl` property to your running API URL, e.g., `http://localhost:5070/api`

### Run the Angular app

```bash
npm start
```

- The app will be available at `http://localhost:4200`

---

## 2. Setting up the .NET API Backend

### Navigate to the API project directory

```bash
cd dotnet-api-temp
```

### Update database connection string

- Open `appsettings.json`
- Update the `DefaultConnection` string with your AWS RDS MySQL details:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=your-rds-endpoint;Port=3306;Database=ExpenseTrackerDb;User=your-username;Password=your-password;"
}
```

### Install required NuGet packages

Run the following commands to install necessary EF Core packages compatible with MySQL:

```bash
dotnet add package Pomelo.EntityFrameworkCore.MySql
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.13
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.13
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.13
dotnet add package Microsoft.EntityFrameworkCore.Relational --version 8.0.13
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.13
```

### Update `Program.cs` to use MySQL

Ensure your `Program.cs` contains:

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 33))));
```

### Run EF Core migrations

- If migrations are not created yet, create them:

```bash
dotnet ef migrations add InitialCreate
```

- Apply migrations to create database schema:

```bash
dotnet ef database update
```

### Run the API

```bash
dotnet run
```

- The API will be available at `http://localhost:5070`

---

## 3. Testing the API

You can test the API endpoints using tools like Postman or curl.

### Example curl commands

- Get all expenses for a user:

```bash
curl -X GET "http://localhost:5070/api/Expenses/GET_ALL_EXPENSE/{userId}"
```

- Create a new expense:

```bash
curl -X POST "http://localhost:5070/api/Expenses/CREATE_EXPENSE" -H "Content-Type: application/json" -d '{"property1":"value1", "property2":"value2"}'
```

- Get single expense by id:

```bash
curl -X GET "http://localhost:5070/api/Expenses/GET_SINGLE_EXPENSE/{userId}/{id}"
```

- Update an expense:

```bash
curl -X PATCH "http://localhost:5070/api/Expenses/UPDATE_EXPENSE/{userId}/{id}" -H "Content-Type: application/json" -d '{"id":1, "Creater":"userId", "property1":"newValue"}'
```

- Delete an expense:

```bash
curl -X DELETE "http://localhost:5070/api/Expenses/DELETE_EXPENSE/{userId}/{id}"
```

Replace `{userId}` and `{id}` with actual values.

---

## 4. Additional Notes

- Ensure your AWS RDS security group allows inbound traffic on port 3306 from your development machine.
- The Angular frontend is configured to allow CORS requests from `http://localhost:4200`.
- Swagger UI is enabled in development mode at `http://localhost:5070/swagger` for API exploration.

---

This completes the setup and running instructions for the Expense Tracker application.

If you encounter any issues or need further assistance, please reach out.
