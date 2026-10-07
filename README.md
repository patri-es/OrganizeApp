# Organize App

OrganizeApp es una aplicación para gestionar productos y categorías. La solución contiene una API REST en .NET 10 y una SPA en React que consume sus endpoints.

## Arquitectura

- **OrganizeApp.Domain**: entidades y reglas del dominio.
- **OrganizeApp.Application**: casos de uso, DTOs, validaciones e interfaces.
- **OrganizeApp.Infrastructure**: persistencia con Entity Framework Core, configuraciones, repositorios y migraciones.
- **OrganizeApp.API**: aplicación ASP.NET Core, controladores, configuración, excepciones y OpenAPI.
- **OrganizeApp.Frontend**: SPA React servida y compilada con Vite.

La solución es `OrganizeApp.sln` y está en la raíz del repositorio. Los cinco proyectos se encuentran bajo `src/`.

## Requisitos previos

- .NET SDK 10.
- Node.js LTS y npm.
- SQL Server, LocalDB u otra base de datos compatible configurada en la API.
- `dotnet-ef` para aplicar migraciones, si no se utiliza una instalación local o restaurada mediante herramientas .NET.

## Instalación y ejecución

Todos los comandos siguientes se ejecutan desde la raíz del repositorio (`C:\Users\tpmancilla\source\repos\OrganizeApp`).

### Backend

```powershell
dotnet restore
dotnet build
dotnet run --project src/OrganizeApp.API/OrganizeApp.API.csproj
```

La API está disponible en `http://localhost:5295`. El perfil HTTPS también utiliza `https://localhost:7049`.

Para aplicar las migraciones, `OrganizeApp.Infrastructure` es el proyecto de migraciones y `OrganizeApp.API` es el proyecto de inicio:

```powershell
dotnet ef database update --project src/OrganizeApp.Infrastructure/OrganizeApp.Infrastructure.csproj --startup-project src/OrganizeApp.API/OrganizeApp.API.csproj
```

La cadena de conexión y las opciones de la API se configuran en `src/OrganizeApp.API/appsettings.json` y `appsettings.Development.json`. En entornos de despliegue deben utilizarse variables de entorno o secretos.

### Frontend

El frontend forma parte de la solución, pero sus dependencias y scripts se gestionan con npm:

```powershell
cd src/OrganizeApp.Frontend
npm ci
npm run dev
```

Por defecto, Vite sirve la aplicación en `http://localhost:5173`. La URL de la API se configura en `src/OrganizeApp.Frontend/.env.local`:

```env
VITE_BASE_API_URL=http://localhost:5295
```

Esta variable indica el destino de las peticiones; el CORS de la API autoriza por separado el origen `http://localhost:5173`. No se debe versionar `.env.local`.

Scripts disponibles:

- `npm run dev`: inicia el servidor de desarrollo.
- `npm run build`: genera la compilación de producción en `dist`.
- `npm run preview`: sirve la compilación generada.
- `npm run lint`: ejecuta ESLint.

## Estructura del repositorio

```text
OrganizeApp/
├── .github/
│   └── copilot-instructions.md
├── OrganizeApp.sln
├── README.md
└── src/
    ├── OrganizeApp.API/
    │   ├── Controllers/
    │   ├── Exceptions/
    │   ├── Properties/
    │   ├── Program.cs
    │   └── appsettings.json
    ├── OrganizeApp.Application/
    │   ├── Category/
    │   ├── Exceptions/
    │   ├── Product/
    │   └── DependencyInjection.cs
    ├── OrganizeApp.Domain/
    │   ├── Entities/
    │   └── Exceptions/
    ├── OrganizeApp.Infrastructure/
    │   ├── Migrations/
    │   ├── Persistence/
    │   │   ├── Configurations/
    │   │   └── Repositories/
    │   └── DependencyInjection.cs
    └── OrganizeApp.Frontend/
        ├── public/
        ├── src/
        │   ├── assets/
        │   ├── components/
        │   └── pages/
        ├── package.json
        ├── package-lock.json
        └── vite.config.js
```

La documentación específica del frontend está en `src/OrganizeApp.Frontend/README.md`.

## Buenas prácticas

- Abrir `OrganizeApp.sln` para trabajar con la solución completa en Visual Studio.
- Mantener la lógica de negocio en Application y Domain; los controladores deben permanecer delgados.
- Mantener las migraciones de Entity Framework Core en Infrastructure.
- Gestionar cadenas de conexión y secretos mediante configuración segura en los entornos de despliegue.
