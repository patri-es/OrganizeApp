# Backend
Aplica a `src/OrganizeApp.Domain`, `src/OrganizeApp.Application`, `src/OrganizeApp.Infrastructure`, `src/OrganizeApp.API` y `test/OrganizeApp.Tests`.

## Convenciones
- Los controladores solo adaptan HTTP: llaman al caso de uso y devuelven el resultado. Sin lógica de negocio ni try/catch.
- No exponer entidades desde la API, solo DTOs (sufijo `DTO` en mayúsculas: `ProductDTO`, `CreateProductDTO`).
- Los errores de dominio y aplicación los traduce `GlobalExceptionHandler`; mantener respuestas HTTP coherentes con ese mecanismo.
- Entidades: `private set` en todas las propiedades. El estado solo cambia con métodos de dominio (en los parciales `*Behavior`), que validan las invariantes y lanzan `<Entity>DomainException`.
- Casos de uso: métodos sin sufijo `Async`. Las queries devuelven `null` si no existe; los comandos lanzan `NotFoundException`. Flujo de un comando: cargar entidad → método de dominio → `SaveChangesAsync`.
- Mapeo entidad→DTO manual, en un único método privado reutilizado. Sin AutoMapper.
- Repositorios: interfaz en Application, implementación en Infrastructure, métodos con sufijo `Async`. Las consultas de `Product` incluyen siempre `Categories`.
- Una funcionalidad nueva incluye DTOs, interfaz, caso de uso, repositorio, configuración EF y endpoints dentro de su feature. Registrar servicios en el `DependencyInjection.cs` de su capa.
- No duplicar consultas ni reglas entre controladores, casos de uso y repositorios.
- Seguir el patrón async existente salvo que el plan indique lo contrario.
- No asumir que la autorización está activa: `UseAuthorization` está comentado.

## Comandos y validación
- Comandos: `dotnet restore`, `dotnet build`, `dotnet test --solution OrganizeApp.sln`.
- Cobertura: `dotnet test --solution OrganizeApp.sln --coverage --coverage-output-format cobertura --results-directory test/TestResults`
- Migración: `dotnet ef migrations add <Name> --project src/OrganizeApp.Infrastructure --startup-project src/OrganizeApp.API`
- Validar con build y los tests afectados; `dotnet test` completo solo si el cambio cruza capas.
- Si cambia el contrato HTTP, actualizar en el mismo cambio los tests de controlador y de casos de uso, y los servicios del frontend.