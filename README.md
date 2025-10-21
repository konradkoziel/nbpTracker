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


🛠️ Prerequisites
Make sure you have the following installed:

.NET SDK 8.0+

Node.js 18+

npm or yarn

⚙️ API Setup
Navigate to the API project folder:

cd ./API
Add the initial migration:

dotnet ef migrations add InitialCreate
Apply the migration to the database:

dotnet ef database update
Run the API:

dotnet run

WEB Setup
Navigate to the WEB project folder:

cd ./WEB
Open the .env file and update the API URL:

env
VITE_API_URL=http://localhost:5000
Replace http://localhost:5000 with the actual URL where your API is running.

Install dependencies:

npm install
Start the development server:

npm run dev
