# Expense Tracker / MoneyBuddy Application

This repository contains the frontend Angular application, backend .NET API, and SQL database integration for the Expense Tracker (renamed MoneyBuddy) application.

---

## Directory Structure

- `src/` - Angular frontend application source code
- `dotnet-api/` - Backend .NET API source code
- `dotnet-api-temp/` - Temporary backend API folder (if applicable)
- `dotnet-api-temp/appsettings.json` - Backend configuration including database connection string
- `src/environments/` - Frontend environment configurations

---

## Running the Application

### 1. Running as Legacy App (Local Development)

#### Backend API

- Navigate to `dotnet-api` or `dotnet-api-temp` folder.
- Update the connection string in `appsettings.json` to point to your MySQL database.
- Run database migrations to create schema:
  ```bash
  dotnet ef migrations add InitialCreate
  dotnet ef database update
  ```
- Run the backend API:
  ```bash
  dotnet run
  ```
- The backend API will be available at `http://localhost:5000` (default port).

#### Frontend Angular App

- Navigate to `src/` folder.
- Update `src/environments/environment.ts` to set `apiUrl` to your backend API URL, e.g. `http://localhost:5000/api/`.
- Install dependencies and run the app:
  ```bash
  npm install
  npm start
  ```
- The frontend will be available at `http://localhost:4200`.

---

### 2. Running as Docker Containers (Without Docker Compose)

#### Backend API

- Build the Docker image:
  ```bash
  docker build -t moneybuddy-backend -f dotnet-api/Dockerfile .
  ```
- Run the container:
  ```bash
  docker run -d -p 5000:80 --name moneybuddy-backend moneybuddy-backend
  ```

#### Frontend Angular App

- Build the Docker image:
  ```bash
  docker build -t moneybuddy-frontend -f src/Dockerfile .
  ```
- Run the container:
  ```bash
  docker run -d -p 4200:80 --name moneybuddy-frontend moneybuddy-frontend
  ```

#### Database

- Use your existing MySQL database or run a MySQL container:
  ```bash
  docker run -d -p 3306:3306 --name moneybuddy-db -e MYSQL_ROOT_PASSWORD=yourpassword mysql:latest
  ```
- Update backend connection string to point to this database.

---

### 3. Running as Kubernetes Deployment

- Create Docker images for backend and frontend as above and push to a container registry.
- Create Kubernetes deployment and service YAML files for backend, frontend, and MySQL database.
- Apply the manifests using:
  ```bash
  kubectl apply -f backend-deployment.yaml
  kubectl apply -f frontend-deployment.yaml
  kubectl apply -f mysql-deployment.yaml
  ```
- Ensure services expose the correct ports and backend API URL is updated in frontend environment config.

---

## Key Files for DevOps / Pipeline

- `dotnet-api/Dockerfile` - Backend Dockerfile
- `src/Dockerfile` - Frontend Dockerfile
- `dotnet-api-temp/appsettings.json` - Backend configuration (update connection strings here)
- `src/environments/environment.ts` - Frontend environment config (update API URLs here)
- `dotnet-api/Program.cs` - Backend API startup and middleware configuration
- `src/app/services/business-data.service.ts` - Frontend service calling backend API

---

## Ports and Endpoints

- Frontend Angular app: `http://localhost:4200`
- Backend API: `http://localhost:5000/api/`
- Database: MySQL default port `3306`

### Backend API Endpoints (ExpensesController)

- `GET /api/Expenses/GET_ALL_EXPENSE/{userId}` - Get all expenses for a user
- `POST /api/Expenses/CREATE_EXPENSE` - Create a new expense
- `GET /api/Expenses/GET_SINGLE_EXPENSE/{userId}/{id}` - Get a single expense
- `PATCH /api/Expenses/UPDATE_EXPENSE/{userId}/{id}` - Update an expense
- `DELETE /api/Expenses/DELETE_EXPENSE/{userId}/{id}` - Delete an expense

---

## Important Commands

### Backend

- Run migrations:
  ```bash
  dotnet ef migrations add InitialCreate
  dotnet ef database update
  ```
- Run API:
  ```bash
  dotnet run
  ```
- Build Docker image:
  ```bash
  docker build -t moneybuddy-backend -f dotnet-api/Dockerfile .
  ```

### Frontend

- Install dependencies and run:
  ```bash
  npm install
  npm start
  ```
- Build Docker image:
  ```bash
  docker build -t moneybuddy-frontend -f src/Dockerfile .
  ```

### Database

- Run MySQL container:
  ```bash
  docker run -d -p 3306:3306 --name moneybuddy-db -e MYSQL_ROOT_PASSWORD=yourpassword mysql:latest
  ```

---

## Integration Notes

- Ensure the backend API connection string points to the correct MySQL database.
- Ensure the frontend environment `apiUrl` points to the backend API URL.
- CORS is enabled in backend for frontend origin `http://localhost:4200`.
- When deploying with Docker or Kubernetes, update environment variables and configs accordingly.

---

This README provides a comprehensive overview for developers and DevOps engineers to build, run, and deploy the MoneyBuddy application.
