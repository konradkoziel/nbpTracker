# 💱 nbpTracker

**nbpTracker** is a .NET 8 Web API application that periodically fetches currency exchange rates from the National Bank of Poland (NBP) — specifically from Table B — and stores them in a SQLite database. It exposes RESTful endpoints for retrieving the stored rates, which can be consumed by a separate frontend application (React).

---

## 📦 Technologies Used

- ASP.NET Core 8 (Web API)
- Entity Framework Core
- SQLite
- AutoMapper
- HttpClient
- BackgroundService


## 🛠️ Prerequisites
Make sure you have the following installed:

.NET SDK 8.0+

Node.js 18+

## ⚙️ API Setup
Navigate to the API project folder:

```bash
cd ./API
```
Add the initial migration:
```bash
dotnet ef migrations add InitialCreate
```
Apply the migration to the database:
```bash
dotnet ef database update
```
Run the API:
```bash
dotnet run
```
## ⚙️ WEB Setup

Replace VITE_API_URL in .env file with the actual URL where your API is running.

Install dependencies:
```bash
npm install
```
Start the development server:

```bash
npm run dev
```
