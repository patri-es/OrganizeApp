# Organize App

OrganizeApp es una aplicación para gestionar productos y categorías. La solución contiene una API REST en .NET 10 con Clean Architecture, una SPA en React/Vite, y un conjunto completo de pruebas unitarias e integración usando xUnit.

## Arquitectura

La solución sigue el patrón **Clean Architecture** con dependencias correctamente direccionadas:

- **OrganizeApp.Domain**: entidades, reglas de negocio y excepciones de dominio. Sin dependencias externas.
- **OrganizeApp.Application**: casos de uso, DTOs, servicios, validaciones e interfaces de repositorios. Organizado por features (`Product`, `Category`).
- **OrganizeApp.Infrastructure**: persistencia con Entity Framework Core, configuraciones, repositorios, migraciones y acceso a datos.
- **OrganizeApp.API**: aplicación ASP.NET Core, controladores RESTful, gestión de excepciones, CORS y OpenAPI.
- **OrganizeApp.Frontend**: SPA React/Vite compilada en tiempo de ejecución, componentes, páginas y configuración de rutas.
- **OrganizeApp.Tests**: pruebas unitarias, de integración e integración con API usando xUnit y Microsoft Testing Framework.

La solución es `OrganizeApp.sln` y está en la raíz del repositorio. Los cinco proyectos se encuentran bajo `src/`, y las pruebas bajo `test/`.

## Requisitos previos

- .NET SDK 10.
- Node.js LTS y npm.
- SQL Server, LocalDB u otra base de datos compatible.
- `dotnet-ef` para aplicar migraciones (incluido en .NET SDK 10).
- `ReportGenerator` global (opcional, para generar reportes de cobertura).

## Instalación y ejecución

Todos los comandos se ejecutan desde la raíz del repositorio.

### Backend

Compilar la solución:

```powershell
dotnet restore
dotnet build
```

Ejecutar la API:

```powershell
dotnet run --project src/OrganizeApp.API/OrganizeApp.API.csproj
```

La API está disponible en:
- HTTP: `http://localhost:5295`
- HTTPS: `https://localhost:7049`

Aplicar migraciones:

```powershell
dotnet ef database update --project src/OrganizeApp.Infrastructure --startup-project src/OrganizeApp.API
```

La cadena de conexión se configura en `src/OrganizeApp.API/appsettings.json` y `appsettings.Development.json`. En producción, usar variables de entorno o secretos.

### Frontend

Instalar dependencias:

```powershell
cd src/OrganizeApp.Frontend
npm ci
```

Iniciar servidor de desarrollo (Vite):

```powershell
npm run dev
```

Disponible en `http://localhost:5173`.

Compilar para producción:

```powershell
npm run build
```

Scripts disponibles:

- `npm run dev`: inicia el servidor de desarrollo.
- `npm run build`: genera compilación de producción en `dist/`.
- `npm run preview`: sirve la compilación generada localmente.
- `npm run lint`: ejecuta ESLint en el código.

Configurar la URL de la API en `src/OrganizeApp.Frontend/.env.local`:

```env
VITE_BASE_API_URL=https://localhost:7049/api
```

> **Nota:** No versionar `.env.local` (incluido en `.gitignore`). El CORS de la API autoriza `http://localhost:5173` en desarrollo.

## Pruebas

El proyecto `OrganizeApp.Tests` contiene:
- **Pruebas unitarias de Domain**: validación de entidades y reglas de negocio.
- **Pruebas unitarias de Application**: casos de uso y servicios.
- **Pruebas de integración de Infrastructure**: repositorios y persistencia.
- **Pruebas de integración de API**: controladores y endpoints HTTP.

### Ejecutar pruebas

Ejecutar todas las pruebas:

```powershell
dotnet test test/OrganizeApp.Tests/OrganizeApp.Tests.csproj
```

Ejecutar pruebas de un ensamblado específico:

```powershell
dotnet test test/OrganizeApp.Tests/OrganizeApp.Tests.csproj --filter "Category=Domain"
```

Ejecutar con salida detallada:

```powershell
dotnet test test/OrganizeApp.Tests/OrganizeApp.Tests.csproj --verbosity detailed
```

### Generar reporte de cobertura

1. **Ejecutar tests con cobertura:**

   ```powershell
   dotnet test test/OrganizeApp.Tests/OrganizeApp.Tests.csproj --results-directory test/TestResults --coverage --coverage-output-format xml
   dotnet test test/OrganizeApp.Tests/OrganizeApp.Tests.csproj --coverage --coverage-output-format xml

   ```

2. **Instalar ReportGenerator (una sola vez):**

   ```powershell
   dotnet tool install --global dotnet-reportgenerator-globaltool
   ```

3. **Generar informe HTML:**

   ```powershell
   reportgenerator "-reports:test/TestResults/*.xml" "-targetdir:test/CoverageReport" "-reporttypes:Html"
   ```

4. **Ver el informe:**

   Abrir `test/CoverageReport/index.html` en el navegador.

El informe muestra:
- Cobertura de líneas, ramas y métodos.
- Desglose por ensamblado, clase y método.
- Líneas no cubiertas por tests.

### Convenciones de nombres para pruebas

Se usa el patrón **Método_Situación_ResultadoEsperado**:

```csharp
Constructor_WithValidData_CreatesProductWithInitialValues()
Constructor_WithNegativePrice_ThrowsProductDomainException()
ChangeName_WithInvalidName_ThrowsProductDomainException()
GetProducts_WithFilters_ReturnsMatchingProducts()
```

### Prioridades de tests

- **P0**: Esenciales (reglas de negocio críticas).
- **P1**: Importantes (funcionalidad principal).
- **P2**: Complementarios (casos borde).

Ver `test/OrganizeApp.Tests/README.md` para la lista completa de tests.

## Estructura del repositorio

```text
OrganizeApp/
├── OrganizeApp.sln
├── README.md
├── global.json
├── .gitignore
└── src/
    ├── OrganizeApp.API/
    │   ├── Controllers/
    │   ├── Exceptions/
    │   ├── Properties/
    │   ├── Program.cs
    │   ├── appsettings.json
    │   ├── appsettings.Development.json
    │   └── OrganizeApp.API.csproj
    ├── OrganizeApp.Application/
    │   ├── Category/
    │   │   ├── Commands/
    │   │   ├── Queries/
    │   │   └── DTOs/
    │   ├── Product/
    │   │   ├── Commands/
    │   │   ├── Queries/
    │   │   └── DTOs/
    │   ├── Exceptions/
    │   ├── Interfaces/
    │   ├── DependencyInjection.cs
    │   └── OrganizeApp.Application.csproj
    ├── OrganizeApp.Domain/
    │   ├── Entities/
    │   │   ├── Product.cs
    │   │   └── Category.cs
    │   ├── Exceptions/
    │   └── OrganizeApp.Domain.csproj
    ├── OrganizeApp.Infrastructure/
    │   ├── Migrations/
    │   ├── Persistence/
    │   │   ├── Configurations/
    │   │   ├── Repositories/
    │   │   └── OrganizeAppDbContext.cs
    │   ├── DependencyInjection.cs
    │   └── OrganizeApp.Infrastructure.csproj
    └── OrganizeApp.Frontend/
        ├── public/
        ├── src/
        │   ├── assets/
        │   ├── components/
        │   ├── pages/
        │   ├── App.jsx
        │   └── main.jsx
        ├── package.json
        ├── package-lock.json
        ├── vite.config.js
        ├── .env.local
        └── OrganizeApp.Frontend.csproj
└── test/
    ├── OrganizeApp.Tests/
    │   ├── Domain/
    │   ├── Application/
    │   ├── Infrastructure/
    │   ├── API/
    │   ├── README.md
    │   └── OrganizeApp.Tests.csproj
    └── TestResults/
```

## Librerías y dependencias principales

### Backend (.NET 10)

**Test & Quality:**
- `xunit.v3` (4.0.2): Framework de pruebas unitarias.
- `Microsoft.NET.Test.Sdk` (18.11.0): SDK de tests.
- `Microsoft.Testing.Extensions.CodeCoverage` (18.12.0): Cobertura de código.
- `xunit.runner.visualstudio` (4.0.1): Ejecutor de xUnit en Visual Studio.
- `Microsoft.AspNetCore.Mvc.Testing` (10.0.14): Testing de API e integración.

**Datos & Persistencia:**
- `Entity Framework Core` (10.0.x): ORM.
- Migraciones incluidas.

**API:**
- `ASP.NET Core` (10.0.x): Framework web.
- CORS configurado para desarrollo.
- OpenAPI (Swagger) disponible.

### Frontend (React/Vite)

**Dependencias:**
- `react` (19.2.8)
- `react-dom` (19.2.8)
- `react-router-dom` (7.18.4): Enrutamiento.
- `axios` (1.20.0): Cliente HTTP.
- `react-hook-form` (7.88.0): Gestión de formularios.
- `react-hot-toast` (2.6.1): Notificaciones.
- `tailwindcss` (4.3.3) + `@tailwindcss/vite` (4.3.3): Estilos.
- `lucide-react` (1.47.0): Iconos.

**DevDependencies:**
- `vite` (8.3.0): Bundler y dev server.
- `eslint` (10.10.0): Linter.
- `@vitejs/plugin-react` (6.1.1): Plugin React para Vite.

## Buenas prácticas

- **Arquitectura**: Mantener dependencias direccionadas. La lógica de negocio en Domain y Application; controladores delgados.
- **Tests**: Escribir pruebas antes o junto con el código. Mantener P0 en 100%.
- **Migraciones**: No editar manualmente `Migrations/`. Usar `dotnet ef migrations add`.
- **Secretos**: Nunca versionar `.env.local`, `appsettings.Development.json` con secretos o credenciales.
- **Código**: Usar identificadores en inglés. Seguir convenciones de nombres del proyecto.

## Futuras implementaciones

1. **Autenticación** (JWT/OAuth)
2. **Carrito de compras** (Cart)
3. **Gestión de pedidos** (Order)
4. **Configuración de usuario** (ClientSettings)
5. **Panel de gestión de pedidos** (OrderSettings)

## Referencias

- Documentación del frontend: `src/OrganizeApp.Frontend/README.md`
- Documentación de tests: `test/OrganizeApp.Tests/README.md`

