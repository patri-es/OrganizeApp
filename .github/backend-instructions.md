---
applyTo: "**/*.cs"
description: Instrucciones de trabajo: Backend
---
# Instrucciones de trabajo: Backend

## Alcance
Estas instrucciones aplican a `src/OrganizeApp.Domain`, `src/OrganizeApp.Application`, `src/OrganizeApp.Infrastructure`, `src/OrganizeApp.API` y `test/OrganizeApp.Tests`.

## Contexto que debe conocerse antes de modificar código
- La solución usa .NET 10 y está organizada con Clean Architecture.
- Dependencias permitidas: `API -> Application + Infrastructure`, `Infrastructure -> Application`, `Application -> Domain`; `Domain` no depende de otras capas.
- La lógica se organiza por feature en Application, actualmente `Product` y `Category`.
- La persistencia usa Entity Framework Core y SQL Server mediante `Infrastructure/Persistence/AppDbContext.cs`.
- Las migraciones viven exclusivamente en `src/OrganizeApp.Infrastructure/Migrations`.
- Los controladores son adaptadores HTTP delgados; los casos de uso viven en Application y las invariantes de negocio en Domain.
- Los repositorios se abstraen mediante interfaces de Application y se implementan en Infrastructure.
- Los DTOs son contratos de entrada/salida; no exponer entidades de Domain directamente desde la API.
- Los errores de aplicación y dominio se traducen mediante `GlobalExceptionHandler`; mantener respuestas HTTP coherentes con ese mecanismo.

## Reglas de implementación
- Mantener identificadores, nombres de tipos, rutas y propiedades en inglés.
- Preferir métodos asíncronos con `CancellationToken` cuando se añada una nueva superficie, siguiendo el patrón existente si se modifica una ya publicada.
- Validar entradas en el límite apropiado y conservar las reglas de negocio dentro de entidades o servicios de Application, no en los controladores.
- Usar métodos de dominio para cambiar el estado de las entidades; no hacer setters públicos ni mutar directamente propiedades privadas.
- Para nuevas funcionalidades, añadir DTOs, interfaces, casos de uso, repositorio/configuración y endpoints siguiendo la feature correspondiente.
- Registrar nuevas implementaciones mediante los módulos de Dependency Injection de Application o Infrastructure.
- No duplicar consultas ni reglas entre controladores, casos de uso y repositorios.
- No editar manualmente archivos de `Migrations/`. Crear migraciones con:
  `dotnet ef migrations add <Name> --project src/OrganizeApp.Infrastructure --startup-project src/OrganizeApp.API`.
- No añadir paquetes NuGet sin pedir autorización explícita.
- No versionar secretos, cadenas de conexión reales, certificados ni `.env.local`.
- No crear commits ni modificar historial Git sin autorización explícita.

## API actual
- Base local HTTP: `http://localhost:5295`; perfil HTTPS: `https://localhost:7049`.
- La API usa rutas bajo `/api` y CORS autoriza `http://localhost:5173` en desarrollo.
- Productos: `GET/POST /api/products`, `GET/PATCH/DELETE /api/products/{id}`, activación/desactivación, stock y asociación de categorías bajo las rutas definidas en `ProductsController`.
- Categorías: `GET/POST /api/categories`, `GET/PUT/DELETE /api/categories/{id}`.
- Mantener compatibilidad con los verbos y formas de respuesta existentes; si se cambia un contrato, actualizar servicios del frontend y pruebas en el mismo cambio.
- OpenAPI se expone en desarrollo. No asumir que autorización está activa: actualmente `UseAuthorization` está comentado.

## Dominio actual
- `Product` y `Category` tienen setters privados, fechas de creación, relación muchos-a-muchos y comportamiento encapsulado en archivos parciales `*Behavior`.
- Los cambios de disponibilidad, stock, precio, nombre, descripción, imagen y categorías deben respetar las validaciones de Domain/Application existentes.
- Un producto puede requerir categorías para activarse; revisar la regla existente antes de cambiar ese flujo.

## Pruebas y validación
- Hay pruebas separadas por API, Application, Domain e Infrastructure en `test/OrganizeApp.Tests`.
- Para cambios de backend ejecutar, como mínimo, `dotnet build` y las pruebas relacionadas; antes de finalizar, preferir `dotnet test` si el cambio cruza varias capas.
- No ocultar errores con `catch` generales, valores por defecto silenciosos o respuestas exitosas falsas.
- Si una modificación cambia el contrato HTTP, añadir o actualizar pruebas de controlador y casos de uso.

## Archivos que conviene leer antes de implementar
- `README.md` y `.github/copilot-instructions.md`.
- El controlador y los DTOs de la feature afectada.
- La interfaz y el caso de uso de Application.
- La entidad y su comportamiento en Domain.
- La interfaz/repositorio y configuración EF Core en Infrastructure.
- Las pruebas equivalentes en `test/OrganizeApp.Tests`.
