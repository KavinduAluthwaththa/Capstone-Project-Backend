# Plant Management & Disease Detection API

A comprehensive agricultural management system built with ASP.NET Core 8.0 that provides crop recommendations, disease identification, and farming resource management capabilities.

## 🌱 Features

### Core Functionality
- **Crop Recommendation System**: AI-powered crop suggestions based on soil and environmental conditions
- **Disease Detection**: Multi-crop disease identification for Rice, Potato, and Pumpkin using machine learning models
- **Farm Management**: Complete farming operations management including crop tracking and growth monitoring
- **Shop Integration**: Marketplace for agricultural products and resources
- **User Management**: Authentication and authorization with JWT and Azure AD integration

### Machine Learning Models
- **Crop Recommendation Model**: ONNX-based model for intelligent crop suggestions
- **Disease Detection Models**: Specialized models for different crops:
  - Rice disease identification
  - Potato disease identification  
  - Pumpkin disease identification

## 🏗️ Architecture

This project follows a clean architecture pattern with the following layers:

```
├── Capstone/                    # Web API Layer (Controllers, Configuration)
├── Capstone.Application/        # Application Services Layer
├── Capstone.Data/              # Data Access Layer (Entity Framework, Migrations)
├── Capstone.Models/            # Domain Models and Entities
└── Capstone.Source/            # Shared Components and Enums
```

## 🛠️ Technology Stack

- **Framework**: ASP.NET Core 8.0
- **Database**: PostgreSQL
- **ORM**: Entity Framework Core
- **Authentication**: JWT Bearer + Azure AD (OpenID Connect)
- **Machine Learning**: ML.NET with ONNX models
- **API Documentation**: Swagger/OpenAPI
- **Architecture**: Clean Architecture with dependency injection

## 📋 Prerequisites

- .NET 8.0 SDK
- PostgreSQL 12+ 
- Visual Studio 2022 or Visual Studio Code
- Git

## 🚀 Getting Started

### 1. Clone the Repository
```bash
git clone https://github.com/KavinduAluthwaththa/Capstone-Project-Backend.git
cd Capstone-Project-Backend
```

### 2. Database Setup
1. Install PostgreSQL and create a database named `PlantManagementApp`
2. Update the connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=PlantManagementApp;Username=your_username;Password=your_password"
  }
}
```

### 3. Run Database Migrations
```bash
cd Capstone
dotnet ef database update
```

### 4. Build and Run
```bash
dotnet restore
dotnet build
dotnet run
```

The API will be available at:
- HTTPS: `https://localhost:7001`
- HTTP: `https://localhost:5000`
- Swagger UI: `https://localhost:7001/swagger`

## 📚 API Endpoints

### Crop Management
- `GET /api/crop` - Get all crops
- `POST /api/crop` - Create new crop
- `PUT /api/crop/{id}` - Update crop
- `DELETE /api/crop/{id}` - Delete crop

### Disease Detection
- `POST /api/ricedisease/predict` - Rice disease prediction
- `POST /api/potatodisease/predict` - Potato disease prediction  
- `POST /api/pumpkindisease/predict` - Pumpkin disease prediction

### Crop Recommendation
- `POST /api/croprecommendation/predict` - Get crop recommendations

### User Management
- `POST /api/user/register` - User registration
- `POST /api/user/login` - User authentication
- `GET /api/user/profile` - Get user profile

### Shop & Marketplace
- `GET /api/shop` - Get all shops
- `GET /api/item` - Get marketplace items
- `POST /api/request` - Create purchase request

## 🧪 Testing

### Running Tests
```bash
dotnet test
```

### API Testing
Use the included `Capstone.http` file for testing endpoints, or explore the API using Swagger UI at `/swagger`.

## 🔧 Configuration

### Environment Variables
Key configuration options in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your PostgreSQL connection string"
  },
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "Domain": "your-domain.com",
    "TenantId": "your-tenant-id",
    "ClientId": "your-client-id"
  }
}
```

### Azure AD Setup (Optional)
For production deployments with Azure AD authentication:
1. Register your application in Azure Portal
2. Update the Azure AD configuration in `appsettings.json`
3. Configure redirect URIs and API permissions

## 📊 Database Schema

The system includes the following main entities:
- **ApplicationUser**: User management and authentication
- **Crop**: Crop information and types
- **Disease**: Disease definitions and symptoms
- **Farmer**: Farmer profiles and information
- **Shop**: Marketplace vendors
- **Item**: Products and resources
- **GrowingCrop**: Active crop tracking
- **Request**: Purchase and service requests

## 🤖 Machine Learning Models

### Model Files Location
ML models are stored in the `MLModels/` directory:
```
MLModels/
├── CropRecommendation/
│   └── crop_recommendation.onnx
└── DiseaseIdentification/
    ├── Rice/rice_model.onnx
    ├── Potato/potato_model.onnx
    └── Pumpkin/pumpkin_model.onnx
```

### Adding New Models
1. Place ONNX model files in the appropriate directory
2. Update the `.csproj` file to copy models to output directory
3. Create corresponding model helper classes

## 🚀 Deployment

### Local Development
```bash
dotnet run --environment Development
```

### Production Deployment
```bash
dotnet publish -c Release -o ./publish
```

### Docker (Optional)
Create a `Dockerfile` for containerized deployment:
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0
COPY ./publish /app
WORKDIR /app
EXPOSE 80
ENTRYPOINT ["dotnet", "Capstone.dll"]
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👥 Authors

- **Kavindu Aluthwaththa** - [KavinduAluthwaththa](https://github.com/KavinduAluthwaththa)

## 🙏 Acknowledgments

- ML.NET team for machine learning capabilities
- ASP.NET Core team for the excellent framework
- PostgreSQL community for the robust database system

## 📞 Support

For support and questions:
- Create an issue in the GitHub repository
- Email: [kavindu18602@gmail.com]

---

**Note**: This is a capstone project developed for educational purposes. For production use, ensure proper security measures, error handling, and testing are implemented. Also you can access the frontend of the project at [https://github.com/KavinduAluthwaththa/Capstone-Project-Frontend].