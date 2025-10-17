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
