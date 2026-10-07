# Organize App
Versión actual del proyecto: API backend en .NET 10 organizada en capas (Domain, Application, Infrastructure) y una capa API que expone los casos de uso. El frontend existe en un directorio separado y se ejecuta con herramientas Node (Vite/React), pero en este README se describe lo mínimo necesario sobre la parte frontend.

## Resumen breve
- Backend dividido en capas: Domain, Application (casos de uso, DTOs, validaciones, interfaces) e Infrastructure (DbContext, configuraciones EF Core, repositorios y registro de dependencias).
- API: proyecto ASP.NET Core que publica los endpoints REST y configura las dependencias de Application e Infrastructure.
- Proyecto orientado a productos y categorías con DTOs, repositorios y casos de uso independientes.

### Requisitos previos
- .NET SDK 10
- (Para la parte opcional del frontend) Node.js >= 18 y npm
- SQL Server / LocalDB o cualquier base de datos configurada en appsettings.json
- dotnet-ef: dotnet tool install --global dotnet-ef
- Node.js (recomendado ≥ 18)
- npm

### Instalación y ejecución
Desde la raíz del repositorio (C:\Users\tpmancilla\source\repos\OrganizeApp):

1) Restaurar y compilar la solución
- dotnet restore
- dotnet build

2) Ejecutar la API (desde la raíz)
- dotnet run --project src/OrganizeApp.API/OrganizeApp.API.csproj
  - El comando iniciará la API y mostrará la URL (Kestrel). Usar esa URL para consumir los endpoints.

3) (Opcional) Aplicar migrations EF Core
- Si el proyecto usa EF Core migrations en el paquete de Infrastructure:
  - dotnet tool restore
  - dotnet ef database update --project src/OrganizeApp.Infrastructure/OrganizeApp.Infrastructure.csproj --startup-project src/OrganizeApp.API/OrganizeApp.API.csproj

### Notas sobre variables de entorno
- La cadena de conexión y otras opciones de entorno se encuentran en src/OrganizeApp.API/appsettings.json y appsettings.Development.json. Para entornos de producción/salida preferible usar variables de entorno o secretos del entorno.

## Frontend (mínimo)
- El frontend está en el directorio OrganizeApp.Frontend (o OrganizeApp.Frontend/). Para el desarrollo típico:
  - cd OrganizeApp.Frontend
  - npm install
  - npm run dev

#### Tecnologías y librerías destacadas
  - React 19, Vite, Tailwind CSS
  - axios (peticiones HTTP), react-router-dom, react-hook-form, react-hot-toast, lucide-react
  - eslint (calidad de código)
 
- El README y documentación específica del frontend se mantienen en su propio directorio y se actualizará por separado.

### Estructura de archivos (visión general actual)

    src/
    ├── OrganizeApp.sln
    ├── OrganizeApp.Domain/
    │   ├── Product/
    │   │   └── (entidades, value objects)
    │   └── Category/
    │       └── (entidades)
    ├── OrganizeApp.Application/
    │   ├── Product/
    │   │   ├── ProductUseCases.cs
    │   │   ├── ProductValidation.cs
    │   │   ├── DTOs/
    │   │   │   ├── ProductDTO.cs
    │   │   │   ├── CreateProductDTO.cs
    │   │   │   └── FiltersProductDTO.cs
    │   │   └── Interfaces/
    │   │       └── IProductRepository.cs
    │   └── Category/
    │       ├── CategoryUseCases.cs
    │       ├── DTOs/
    │       │   ├── CategoryDTO.cs
    │       │   └── CreateCategoryDTO.cs
    │       └── Interfaces/
    │           └── ICategoryRepository.cs
    ├── OrganizeApp.Infrastructure/
    │   ├── AppDbContext.cs
    │   ├── Configurations/
    │   │   ├── ProductConfiguration.cs
    │   │   ├── CategoryConfiguration.cs
    │   │   └── ProductCategoryConfiguration.cs
    │   ├── Repositories/
    │   │   ├── ProductRepository.cs
    │   │   └── CategoryRepository.cs
    │   └── DependencyInjection.cs
    └── OrganizeApp.API/
        ├── Program.cs
        ├── appsettings.json
        ├── Controllers/
        │   ├── ProductsController.cs
        │   └── CategoriesController.cs
        └── DependencyInjection.cs  (registra Application e Infrastructure)


## Buenas prácticas y recomendaciones
- Usar el proyecto solución (src/OrganizeApp.sln) para abrir/compilar en Visual Studio.
- Gestionar la cadena de conexión mediante variables de entorno en despliegues.
- Ejecutar las migraciones EF Core desde los proyectos correctos (Infrastructure como proyecto de migraciones y API como startup-project).
