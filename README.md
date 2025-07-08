# Central - Multi-Tenant SaaS Management Platform

A comprehensive .NET 8 application for managing multi-tenant SaaS environments with bundles, tenants, domains, and subscriptions.

## 🏗️ Architecture

This project follows **Clean Architecture** principles with a clear separation of concerns:

```
Central/
├── Central.Domain/          # Domain entities, interfaces, and business rules
├── Central.Application/     # Application services, DTOs, and business logic
├── Central.Persistence/     # Data access layer with Entity Framework Core
└── Central.WebApi/         # API presentation layer with Minimal APIs
```

### Architecture Layers

- **Domain Layer**: Core business entities and domain logic
- **Application Layer**: Use cases, validation, mapping, and events
- **Persistence Layer**: Database context, repositories, and migrations
- **API Layer**: HTTP endpoints, middleware, and API documentation

## 🚀 Features

### Core Functionality
- **Bundle Management**: Create and manage feature bundles
- **Tenant Management**: Multi-tenant support with domain isolation
- **Domain Management**: Tenant-specific domain configuration
- **Subscription Management**: Tenant subscriptions to bundles with lifecycle tracking

### Technical Features
- **Clean Architecture**: Well-structured, testable, and maintainable code
- **Result Pattern**: Consistent error handling without exceptions
- **Event-Driven**: Domain events for decoupled business logic
- **Validation**: FluentValidation with comprehensive rules
- **Mapping**: AutoMapper for entity-DTO transformations
- **API Documentation**: Comprehensive Swagger/OpenAPI documentation
- **Logging**: Structured logging with middleware
- **Performance Monitoring**: Request performance tracking

## 🛠️ Technology Stack

### Backend
- **.NET 8**: Latest .NET framework
- **ASP.NET Core**: Web API framework
- **Entity Framework Core**: ORM with PostgreSQL
- **Minimal APIs**: Lightweight API endpoints
- **FluentValidation**: Input validation
- **AutoMapper**: Object-to-object mapping

### Database
- **PostgreSQL**: Primary database
- **Entity Framework Core**: Database access and migrations

### Documentation & Testing
- **Swagger/OpenAPI**: API documentation
- **Swashbuckle**: Swagger generation for .NET

## 📋 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL 15+](https://www.postgresql.org/download/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [JetBrains Rider](https://www.jetbrains.com/rider/) (recommended)

## 🚀 Getting Started

### 1. Clone the Repository
```bash
git clone https://github.com/yourorg/central.git
cd central
```

### 2. Database Setup
```bash
# Update connection string in appsettings.json
# Create database and run migrations
dotnet ef database update --project Central.Persistence --startup-project Central.WebApi
```

### 3. Run the Application
```bash
cd Central.WebApi
dotnet run
```

The API will be available at:
- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`
- **Swagger UI**: `http://localhost:5000` (development only)

## 📚 API Documentation

### Base URL
```
https://localhost:5001/api
```

### Available Endpoints

#### Bundles
```http
GET    /api/bundles                    # Get paginated bundles
GET    /api/bundles/{id}               # Get bundle by ID
GET    /api/bundles/key/{key}          # Get bundle by key
POST   /api/bundles                    # Create new bundle
PUT    /api/bundles/{id}               # Update bundle
DELETE /api/bundles/{id}               # Delete bundle
GET    /api/bundles/key-exists/{key}   # Check if key exists
```

#### Tenants
```http
GET    /api/tenants                    # Get paginated tenants
GET    /api/tenants/{id}               # Get tenant by ID
GET    /api/tenants/email/{email}      # Get tenant by email
POST   /api/tenants                    # Create new tenant
PUT    /api/tenants/{id}               # Update tenant
DELETE /api/tenants/{id}               # Delete tenant
POST   /api/tenants/{id}/activate      # Activate tenant
POST   /api/tenants/{id}/deactivate    # Deactivate tenant
```

#### Tenant Domains
```http
GET    /api/tenant-domains                     # Get paginated domains
GET    /api/tenant-domains/{id}                # Get domain by ID
GET    /api/tenant-domains/name/{name}         # Get domain by name
GET    /api/tenant-domains/tenant/{tenantId}   # Get domains by tenant
POST   /api/tenant-domains                     # Create new domain
PUT    /api/tenant-domains/{id}                # Update domain
DELETE /api/tenant-domains/{id}                # Delete domain
```

#### Tenant Subscriptions
```http
GET    /api/tenant-subscriptions                              # Get paginated subscriptions
GET    /api/tenant-subscriptions/{id}                         # Get subscription by ID
GET    /api/tenant-subscriptions/tenant/{tenantId}            # Get subscriptions by tenant
GET    /api/tenant-subscriptions/bundle/{bundleId}            # Get subscriptions by bundle
GET    /api/tenant-subscriptions/active                       # Get active subscriptions
POST   /api/tenant-subscriptions                              # Create new subscription
PUT    /api/tenant-subscriptions/{id}                         # Update subscription
DELETE /api/tenant-subscriptions/{id}                         # Delete subscription
GET    /api/tenant-subscriptions/check-active/{tenantId}/{bundleId}  # Check active subscription
```

### Query Parameters

Most list endpoints support these query parameters:
- `page`: Page number (default: 1)
- `pageSize`: Items per page (default: 10, max: 100)
- `search`: Search term
- `sortBy`: Field to sort by
- `sortDescending`: Sort direction (default: false)

**Example:**
```http
GET /api/bundles?page=1&pageSize=20&search=carbon&sortBy=name&sortDescending=false
```

## 📊 Data Models

### Bundle
```json
{
  "id": "guid",
  "name": "Carbon Footprint",
  "key": "carbon_footprint",
  "description": "Track and analyze carbon emissions",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

### Tenant
```json
{
  "id": "guid",
  "name": "Acme Corporation",
  "email": "admin@acme.com",
  "isActive": true,
  "logoUrl": "https://example.com/logo.png",
  "phoneNumber": "+1234567890",
  "primaryColor": "#FF0000",
  "secondaryColor": "#00FF00",
  "domains": [],
  "subscriptions": []
}
```

### Tenant Domain
```json
{
  "id": "guid",
  "tenantId": "guid",
  "name": "acme-main",
  "tenantName": "Acme Corporation"
}
```

### Tenant Subscription
```json
{
  "id": "guid",
  "tenantId": "guid",
  "bundleId": "guid",
  "duration": 12,
  "startDate": "2024-01-01T00:00:00Z",
  "endDate": "2025-01-01T00:00:00Z",
  "isActive": true,
  "tenantName": "Acme Corporation",
  "bundleName": "Carbon Footprint"
}
```

## 🧪 Testing

### Running Tests
```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### API Testing
Use the included Swagger UI at `http://localhost:5000` for interactive API testing, or import the OpenAPI specification into:
- [Postman](https://www.postman.com/)
- [Insomnia](https://insomnia.rest/)
- [Thunder Client](https://www.thunderclient.com/) (VS Code)

## 🔧 Configuration

### Connection Strings
Update `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=CentralDb;Username=postgres;Password=your-password"
  }
}
```

### Environment Variables
- `ASPNETCORE_ENVIRONMENT`: Set to `Development`, `Staging`, or `Production`
- `ConnectionStrings__DefaultConnection`: Database connection string

## 📁 Project Structure

```
Central/
├── Central.Domain/
│   ├── Entities/           # Domain entities (Bundle, Tenant, etc.)
│   ├── Events/             # Domain events
│   └── Interfaces/         # Repository interfaces
│
├── Central.Application/
│   ├── Common/             # Shared application components
│   ├── DTOs/               # Data transfer objects
│   ├── Events/             # Event handlers and dispatcher
│   ├── Exceptions/         # Custom exceptions
│   ├── Extensions/         # Service collection extensions
│   ├── Mappings/           # AutoMapper profiles
│   ├── Services/           # Application services
│   └── Validators/         # FluentValidation validators
│
├── Central.Persistence/
│   ├── Context/            # DbContext
│   ├── Extensions/         # Service registration
│   ├── Fakers/             # Test data generators
│   ├── Repositories/       # Repository implementations
│   └── Seeders/            # Database seeders
│
└── Central.WebApi/
    ├── Endpoints/          # Minimal API endpoints
    ├── Extensions/         # API extensions
    ├── Middleware/         # Custom middleware
    ├── Models/             # API-specific models
    └── Program.cs          # Application entry point
```

## 🎯 Domain Events

The application uses domain events for decoupled business logic:

### TenantCreatedEvent
Triggered when a new tenant is created. Handlers can:
- Send welcome emails
- Create audit logs
- Initialize tenant resources
- Notify external systems

## 🔒 Security Considerations

> **Note**: This version does not include authentication/authorization. For production use, consider adding:
> - JWT authentication
> - Role-based authorization
> - API rate limiting
> - Input sanitization
> - HTTPS enforcement

## 🚀 Deployment

### Docker Support
```dockerfile
# Build and run with Docker
docker build -t central-api .
docker run -p 5000:80 central-api
```

### Production Considerations
- Use production database
- Configure proper logging
- Set up monitoring
- Configure HTTPS
- Enable authentication
- Set up CI/CD pipeline

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 📞 Support

For support and questions:
- Create an [issue](https://github.com/yourorg/central/issues)
- Email: support@central.com
- Documentation: [Wiki](https://github.com/yourorg/central/wiki)

---

**Built with ❤️ using .NET 8 and Clean Architecture principles**
