# MicroShop

MicroShop is a marketplace platform where users can sell their products and create orders for products from other sellers.

## Architecture

The project uses a **microservices architecture** consisting of the following services:

- **Gateway** – API gateway using Ocelot  
- **AuthService** – handles authentication and authorization with JWT and Refresh/Access tokens  
- **ProductService** – manages products and product-related logic  
- **OrderService** – handles orders and order processing  
- **Orchestrator** – coordinates workflow between services  
- **NotificationService** – sends notifications via Twilio and SendGrid  
- **FileService** – manages file uploads, stored in Azure Blob Storage  

---

## Technologies

### Backend

- ASP.NET Core with Identity  
- CQRS + MediatR  
- SQL Server  
- Redis caching  
- Azure Service Bus  
- SendGrid & Twilio integration  
- Dockerized with docker-compose  

### Frontend

- React + TypeScript  
- Material UI (MUI)  

---

## Running Locally

1. Clone the repository:

```bash
git clone https://github.com/your-username/MicroShop.git
cd MicroShop
