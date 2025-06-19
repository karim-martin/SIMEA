# SIMEA - Strategic Information Management & Enterprise Architecture Platform

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Open Source](https://badges.frapsoft.com/os/v1/open-source.svg?v=103)](https://opensource.org/)
[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-purple)](https://asp.net/)
[![Entity Framework](https://img.shields.io/badge/Entity%20Framework-8.0-green)](https://docs.microsoft.com/en-us/ef/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-red)](https://www.microsoft.com/en-us/sql-server)
[![Contributions Welcome](https://img.shields.io/badge/contributions-welcome-brightgreen.svg?style=flat)](https://github.com/dwyl/est/issues)

## 🏗️ Overview

**SIMEA** (Strategic Information Management & Enterprise Architecture) is a comprehensive, **open source** web-based platform designed to manage and visualize enterprise architecture across four key domains: Business, Application, Data, and Infrastructure. The system provides a centralized repository for architectural artifacts, enables cross-domain traceability, and generates comprehensive enterprise architecture views and reports.

As an open source solution, SIMEA empowers organizations of all sizes to implement enterprise architecture management without licensing costs, while benefiting from community-driven development and transparency.

## 🚀 Key Features

### 📋 **Multi-Domain Architecture Management**

#### Business Architecture
- **Business Strategy Management**: Track business goals, strategic objectives, KPIs, and stakeholder alignment
- **Operational Models**: Document operational processes, owners, inputs/outputs, and performance metrics
- **Organization Views**: Manage department structures, roles, responsibilities, and reporting hierarchies
- **Capability Mapping**: Define business capabilities, maturity levels, dependencies, and gaps
- **Process Modeling**: Document business processes with workflow steps, value streams, and efficiency metrics
- **Product/Service Catalog**: Manage business products and services with value propositions and market analysis
- **Investment Decision Tracking**: Financial modeling, ROI analysis, and investment prioritization
- **Business Outcome Management**: Track enhanced business outcomes with success metrics and value realization
- **Technology Trend Analysis**: Monitor emerging technologies and their business impact

#### Application Architecture
- **Application Portfolio Management**: Comprehensive application inventory with technology stacks and integration points
- **Application Rationalization**: Categorize applications (Tolerate/Invest/Migrate/Eliminate) with business value scoring
- **Service Catalog**: Document IT services with SLAs, costs, and dependencies
- **Cross-Reference Management**: Track application-to-application relationships and data flows
- **Security Management**: Application security requirements, compliance standards, and vulnerability tracking
- **Business Requirements**: Manage application business requirements with priority and status tracking

#### Data Architecture
- **Data Governance**: Define data ownership, stewardship, quality metrics, and compliance requirements
- **Information Hierarchy**: Model data relationships and hierarchical structures
- **Data Flow Management**: Document data movement between systems with security requirements
- **Logical Data Modeling**: Create entity-relationship models with attributes and relationships
- **Data Security**: Manage data security requirements, access controls, and encryption policies
- **Information Requirements**: Track data requirements across the organization

#### Infrastructure Architecture
- **Resource Management**: Track infrastructure resources, costs, and allocation status
- **System Integration**: Document system-to-application and system-to-business relationships
- **Infrastructure Security**: Manage security requirements, compliance, and vulnerability assessments
- **Business Requirements**: Infrastructure-specific business requirements and priorities

### 🔗 **Advanced Linking & Traceability**
- **Artifact Linking System**: Create relationships between artifacts across all domains
- **Cross-Domain Impact Analysis**: Understand how changes in one domain affect others
- **Dependency Tracking**: Visualize dependencies between different architectural components
- **Traceability Matrix**: Full traceability from business strategy to infrastructure implementation

### 📊 **Comprehensive Reporting & Visualization**

The system provides 13 specialized enterprise architecture views:

1. **Strategic Alignment View (View A)**: Visualizes alignment between business strategies, capabilities, and applications
2. **Application Portfolio Dashboard (View B)**: Overview of all applications, statuses, and relationships
3. **Architecture Roadmap (View C)**: Timeline view of planned architecture changes and implementations
4. **Cross-Domain Impact Analysis (View D)**: Analysis of inter-domain dependencies and impacts
5. **Data Flow Visualization (View E)**: Data movement and integration patterns
6. **Technology Stack Analysis (View F)**: Technology inventory and standardization status
7. **Capability Heat Map (View G)**: Capability maturity and investment analysis
8. **Security & Compliance Dashboard (View H)**: Security posture across all domains
9. **Resource Utilization View (View I)**: Infrastructure resource allocation and capacity planning
10. **Integration Architecture (View J)**: System integration patterns and interfaces
11. **Business Value Realization (View K)**: Investment outcomes and value tracking
12. **Risk & Gap Analysis (View L)**: Risk assessment and capability gaps
13. **Rationalization Matrix (View M)**: Application rationalization recommendations and portfolio optimization

### 🔐 **Security & Access Control**
- **Role-Based Access Control**: Granular permissions with roles (Root, EATeam)
- **Identity Management**: Built-in ASP.NET Identity with user registration and authentication
- **Secure Cookie Configuration**: Enhanced security with SameSite and HTTPS enforcement
- **Audit Tracking**: Complete audit trail for all artifacts with creation and modification tracking

### 🔄 **Data Management & Integration**
- **Export Capabilities**: Excel and CSV export functionality using ClosedXML and CsvHelper
- **API Support**: RESTful APIs with Swagger documentation
- **Real-time Updates**: SignalR integration for real-time collaboration
- **Data Validation**: Comprehensive input validation and sanitization

## 🛠️ Technology Stack

### Backend
- **.NET 8.0**: Latest .NET framework for high performance and modern development
- **ASP.NET Core MVC**: Model-View-Controller architecture for web applications
- **Entity Framework Core**: Object-relational mapping for data access
- **SQL Server**: Enterprise-grade database management system
- **ASP.NET Identity**: Authentication and authorization framework

### Frontend
- **HTML5/CSS3**: Modern web standards for responsive UI
- **Bootstrap**: CSS framework for responsive design
- **JavaScript/jQuery**: Client-side interactivity and AJAX
- **Charts.js**: Data visualization and charting
- **DataTables**: Advanced table functionality with sorting and filtering

### Additional Libraries & Tools
- **Serilog**: Structured logging framework
- **AutoMapper**: Object-to-object mapping
- **Quartz.NET**: Job scheduling and background tasks
- **SignalR**: Real-time web functionality
- **HtmlSanitizer**: XSS protection and HTML sanitization
- **ClosedXML**: Excel file generation and manipulation
- **CsvHelper**: CSV file processing
- **EPPlus**: Advanced Excel functionality

## 📁 Project Structure

```
SIMEA/
├── Controllers/                    # MVC Controllers organized by domain
│   ├── Application/               # Application architecture controllers
│   ├── Business/                  # Business architecture controllers
│   ├── Data/                     # Data architecture controllers
│   ├── Infrastructure/           # Infrastructure controllers
│   ├── EAOutputController.cs     # Enterprise Architecture reporting
│   └── ...
├── Models/                        # Data models and ViewModels
│   ├── ApplicationArchitectureModels.cs
│   ├── BusinessArchitectureModels.cs
│   ├── DataArchitectureModels.cs
│   ├── InfrastructureArchitectureModels.cs
│   ├── EnterpriseArchitectureModels.cs
│   ├── ArtifactLink.cs           # Cross-domain linking
│   └── ...
├── Views/                         # Razor views organized by controller
├── Data/                         # Entity Framework DbContext and migrations
├── Areas/Identity/               # ASP.NET Identity area
├── wwwroot/                      # Static files (CSS, JS, images)
└── ...
```

## ⚙️ Installation & Setup

### Prerequisites
- .NET 8.0 SDK or later
- SQL Server 2019 or later (or SQL Server Express)
- Visual Studio 2022 or VS Code
- Node.js (for frontend dependencies, if applicable)

### Installation Steps

1. **Clone the Repository**
   ```bash
   git clone <repository-url>
   cd SIMEA
   ```

2. **Restore Dependencies**
   ```bash
   dotnet restore
   ```

3. **Configure Database Connection**
   - Update `appsettings.json` with your SQL Server connection string:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SIMEA;Trusted_Connection=true;MultipleActiveResultSets=true"
     }
   }
   ```

4. **Apply Database Migrations**
   ```bash
   dotnet ef database update
   ```

5. **Seed Initial Data** (Optional)
   - Run the seed data SQL script located in `Data/seedata EA.sql`

6. **Run the Application**
   ```bash
   dotnet run
   ```

7. **Access the Application**
   - Navigate to `https://localhost:5001` or `http://localhost:5000`
   - Register a new account or use seeded credentials

## 🎯 Usage Guide

### Getting Started
1. **User Registration**: Create an account through the registration system
2. **Role Assignment**: Contact administrators for appropriate role assignment (EATeam, Root)
3. **Artifact Creation**: Start by creating business strategies and work down through the architecture layers
4. **Linking**: Use the artifact linking system to create relationships between components
5. **Reporting**: Generate comprehensive reports through the EAOutput views

### Best Practices
- **Start with Business**: Begin with business architecture before moving to technical domains
- **Maintain Links**: Regularly update artifact relationships to ensure accurate impact analysis
- **Use Standardized Naming**: Follow consistent naming conventions across all artifacts
- **Regular Reviews**: Schedule periodic reviews of architectural artifacts for accuracy
- **Version Control**: Leverage the built-in audit trail for change management

## 🔧 Configuration

### Application Settings
Key configuration options in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your SQL Server connection string"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

### Security Configuration
- Cookie settings are configured for secure production deployment
- HTTPS redirection is enforced
- Identity options can be customized in `Program.cs`

### Logging Configuration
- Serilog is configured for structured logging
- Logs are written to both console and file
- Log levels can be adjusted per namespace

## 🤝 Contributing

We welcome and encourage contributions from the community! SIMEA thrives on collaborative development and diverse perspectives. Here's how you can contribute:

### How to Contribute

1. **Fork the Repository**
2. **Create Feature Branch**: `git checkout -b feature/amazing-feature`
3. **Commit Changes**: `git commit -m 'Add amazing feature'`
4. **Push to Branch**: `git push origin feature/amazing-feature`
5. **Open Pull Request**

### Ways to Contribute
- 🐛 **Bug Reports**: Found an issue? Create a detailed bug report
- 💡 **Feature Requests**: Have an idea? Share it through GitHub issues
- 📝 **Documentation**: Improve docs, tutorials, or examples
- 🧪 **Testing**: Help with testing across different environments
- 💻 **Code**: Implement new features or fix existing issues
- 🎨 **UI/UX**: Improve the user interface and experience
- 🌐 **Translations**: Help make SIMEA available in more languages

### Development Guidelines
- Follow C# coding conventions and .NET best practices
- Add unit tests for new features
- Update documentation as needed
- Ensure all tests pass before submitting
- Keep pull requests focused and atomic
- Provide clear commit messages and PR descriptions

### Community Guidelines
- Be respectful and inclusive
- Provide constructive feedback
- Help others learn and grow
- Follow our Code of Conduct

## 📄 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

The MIT License means you can:
- ✅ Use SIMEA for commercial purposes
- ✅ Modify and distribute the code
- ✅ Include it in proprietary software
- ✅ Use it privately

**Freedom to innovate, obligation to share improvements with the community!**

## 🆘 Support & Community

Join our growing community of enterprise architects and developers!

### Getting Help
- 📋 **GitHub Issues**: For bug reports and feature requests
- 💬 **Discussions**: For questions, ideas, and community support
- 📖 **Wiki**: Documentation, tutorials, and best practices
- 📧 **Email**: For security issues or private inquiries

### Community
- 🌍 **Global Community**: Enterprise architects from around the world
- 🤝 **Collaboration**: Learn from and contribute to shared knowledge
- 📚 **Best Practices**: Share your EA experiences and methodologies
- 🎯 **Use Cases**: Real-world examples and implementations

## 🗺️ Roadmap

### Upcoming Features
- [ ] Advanced visualization with D3.js integration
- [ ] REST API expansion for third-party integrations
- [ ] Enhanced reporting with custom report builder
- [ ] Mobile-responsive design improvements
- [ ] Advanced analytics and KPI dashboards
- [ ] Advanced workflow management
- [ ] Plugin architecture for extensibility
- [ ] Community-driven templates and best practices

## 🌟 Why Open Source?

SIMEA is open source because we believe that:
- **Enterprise Architecture should be accessible** to organizations of all sizes
- **Transparency builds trust** in critical business systems
- **Community collaboration** leads to better, more robust solutions
- **No vendor lock-in** gives organizations freedom and flexibility
- **Shared knowledge** accelerates innovation in the EA field

## 🙏 Acknowledgments

- Built with ASP.NET Core and Entity Framework
- UI components powered by Bootstrap
- Charting capabilities provided by Chart.js
- Excel functionality through ClosedXML and EPPlus

---

**SIMEA** - Empowering organizations to manage and visualize their enterprise architecture with comprehensive cross-domain traceability and insightful reporting capabilities. 