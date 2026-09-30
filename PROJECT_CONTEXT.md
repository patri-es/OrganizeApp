# OrganizeApp — Contexto técnico

## Stack

- Frontend: React + Vite + Tailwind CSS.
- Backend: ASP.NET Core Web API (.NET 10) con Controllers.
- Persistencia: Entity Framework Core + SQL Server.
- Frontend HTTP: Axios.
- Routing: React Router.
- Formularios: React Hook Form.
- Notificaciones: React Hot Toast.
- Iconos: Lucide React.

## Arquitectura

Separación Frontend / Backend:

Frontend SPA
→ HTTP/REST
→ ASP.NET Core API
→ Entity Framework Core
→ SQL Server

Estructura principal:

Frontend/
Backend/

### Frontend

Aplicación SPA React basada en componentes funcionales y Hooks.

Organización principal por funcionalidades/entidades. Los módulos CRUD siguen un patrón similar de componente principal + formulario + listado.

Configuración dependiente del entorno mediante variables de entorno de Vite.

### Backend

API REST basada en Controllers.

Estructura:

Backend/
├── Controllers/
├── Models/
├── Migrations/
├── Program.cs
└── appsettings.json

Dependency Injection para la configuración y consumo de `AppDbContext`.

Los Controllers acceden directamente a `AppDbContext`; actualmente no existen capas Service, Repository ni una arquitectura Clean/Onion.
Esta estructura esta siendo refactorizada para que se organize por capas completamente separadas.

## Modelo

Entidades principales:

- `Person`
- `Product`
- `Category`

Relación:

`Category 1 ── N Product`

`Product.CategoryId` es nullable y `Category` es una navegación opcional.

Persistencia mediante EF Core Code First y migraciones.

## Convenciones / características

- C# con Nullable Reference Types.
- Controllers con `[ApiController]` y attribute routing.
- Acceso a datos asíncrono mediante `async`/`await`.
- Validación mediante Data Annotations donde corresponde.
- React funcional con Hooks.
- Axios para comunicación con la API.
- Tailwind CSS para estilos.
- Código orientado a mantener patrones homogéneos entre los distintos CRUD.

Al modificar el proyecto, mantener la arquitectura y patrones existentes salvo indicación expresa de cambio arquitectónico. 
Priorizar la coherencia con el código existente sobre la introducción de patrones o capas que actualmente no forman parte del proyecto.


# Refactorización:

## Resumen y Objetivos de este proyecto:
Estoy trabajando en un proyecto .NET Core 10 para crear un e-commerce. 

Actualmente está separado por Frontend y Backend. 

- Frontend: Aplicación SPA React basada en componentes funcionales y Hooks. Organización principal por funcionalidades/entidades. Los módulos CRUD siguen un patrón similar de componente principal + formulario + listado.  Configuración dependiente del entorno mediante variables de entorno de Vite.

- Backend: API REST basada en Controllers. Estructura:  

		Backend/ 
			├── Controllers/ 
			├── Models/ 
			├── Migrations/ 
			├── Program.cs 
			└── appsettings.json  
	
Dependency Injection para la configuración y consumo de `AppDbContext`. 


>Los Controllers acceden directamente a `AppDbContext`; actualmente no existen capas Service, Repository ni una arquitectura Clean/Onion. 



--> Te he dado el contexto básico del proyecto. Esta última frase es clave, ya que estoy modificando la estructura del Backend por completo. Quiero que siga la siguiente separación por capas: 

- OrganizeApp.Domain: Entidades puras, Value Objects, Excepciones de dominio e Interfaces de repositorios. Sin dependencias externas ni NuGet de BD.

- OrganizeApp.Application: Casos de uso (Servicios/CQRS con MediatR), DTOs, Validaciones (FluentValidation) e Interfaces.

- OrganizeApp.Infrastructure: Entity Framework Core, DbContext, migraciones, persistencia e integraciones con servicios externos.

- OrganizeApp.API: Controladores, Endpoints, Middleware de excepciones y configuración de autenticación.


