# DriveOps

A comprehensive vehicle service management system built with .NET 9 and Entity Framework Core.

## Overview

DriveOps is a modern automotive service management application designed to streamline vehicle inspections, maintenance tracking, and customer service operations. The system provides a complete workflow from vehicle registration to inspection findings and service recommendations.

## Features

### 🚗 Vehicle Management
- Vehicle registration with detailed information (plate number, make, model, year, VIN)
- Vehicle ownership tracking and transfer management
- Complete vehicle history and service records

### 👥 Customer Management
- Support for both individual and company customers
- Customer contact information and service history
- Vehicle ownership relationships

### 🔧 Technician Management
- Technician registration with specialization tracking
- Service assignment and workload management
- Performance and availability tracking

### 📋 Job Order System
- Comprehensive job order creation and management
- Status tracking (Pending, InProgress, Completed, Cancelled)
- Customer, vehicle, and technician assignment
- Complete audit trail with timestamps

### 🔍 Vehicle Inspections
- Detailed inspection findings documentation
- Severity classification (Critical, Moderate, Minor)
- **Required recommendations** for every finding
- Issue resolution tracking
- Integration with job order workflow

### 📊 Service Management
- Reported issue tracking and resolution
- Inspection finding management
- Service recommendation system
- Complete service history

## Technology Stack

- **Backend**: .NET 9, ASP.NET Core Web API
- **Database**: PostgreSQL with Entity Framework Core
- **Architecture**: Clean Architecture with separated concerns
- **Documentation**: Swagger/OpenAPI integration

## Project Structure

```
DriveOps/
├── DriveOps.Api/           # Main API project
│   ├── Controllers/        # API endpoints
│   ├── Services/          # Business logic
│   ├── Models/            # Entity models
│   ├── Data/              # Database context and migrations
│   ├── Helpers/           # Validation and utility classes
│   ├── Mappers/           # Entity-DTO mapping
│   └── Interfaces/        # Service contracts
├── DriveOps.Shared/       # Shared DTOs and enums
│   ├── Dtos/              # Data transfer objects
│   └── Enums/             # Shared enumerations
└── README.md
```

## Key Business Workflow

1. **Customer & Vehicle Registration**
   - Register customers (individual or company)
   - Add vehicles and establish ownership

2. **Service Scheduling**
   - Create job orders for vehicle services
   - Assign technicians based on specialization

3. **Vehicle Inspection**
   - Document inspection findings with descriptions
   - Provide mandatory recommendations for each finding
   - Classify severity levels for prioritization

4. **Issue Resolution**
   - Track reported issues and inspection findings
   - Monitor resolution status
   - Maintain complete service history

## API Endpoints

### Core Resources
- `/api/customers` - Customer management
- `/api/vehicles` - Vehicle information and tracking
- `/api/vehicle-ownerships` - Ownership management
- `/api/technicians` - Technician administration
- `/api/joborders` - Service job management

### Service Operations
- `/api/joborders/{jobOrderNumber}/findings` - Inspection findings
- `/api/joborders/{jobOrderNumber}/issues` - Reported issues

## Database Schema

The system uses PostgreSQL with Entity Framework Core migrations for schema management. Key entities include:

- **Customers** (Individual/Company support)
- **Vehicles** (Complete vehicle information)
- **VehicleOwnerships** (Ownership tracking)
- **Technicians** (Service staff management)
- **JobOrders** (Service requests)
- **InspectionFindings** (Inspection results with mandatory recommendations)
- **ReportedIssues** (Customer-reported problems)

## Features in Development

- 📝 Quotation and estimate system
- 💰 Parts pricing and sourcing options
- ✅ Customer approval workflow
- 📅 Service scheduling system
- 📊 Reporting and analytics
