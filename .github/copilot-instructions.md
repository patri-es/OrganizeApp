# OrganizeApp
## COPILOTWORKSPACE CONTEXT

### Alcance del repositorio
OrganizeApp es una solución con una API REST en .NET 10 y una SPA React/Vite en JavaScript. 
La solución completa está en `OrganizeApp.sln`; 
el código de producción está bajo `src/` y las pruebas bajo `test/`.

### Instrucciones específicas por capa
- Para cambios en Domain, Application, Infrastructure, API o pruebas .NET, leer `C:\Users\tpmancilla\source\repos\OrganizeApp\.github\backend-instructions.md`.
- Para cambios en `src/OrganizeApp.Frontend`, leer `C:\Users\tpmancilla\source\repos\OrganizeApp\.github\frontend-instructions.md`.
- Si un cambio cruza Backend y Frontend, leer ambos archivos y mantener el contrato HTTP sincronizado.

### Convenciones globales
- Identificadores, nombres de tipos, endpoints, propiedades nuevas y comentarios en archivos de código deben estar en inglés.
- Responder siempre en español.
- Mantener Clean Architecture: `API -> Application + Infrastructure`, `Infrastructure -> Application`, `Application -> Domain`; Domain no depende de otras capas.
- En Application organizar el código por feature (`Product`, `Category`).
- Preferir cambios pequeños, coherentes con los patrones existentes y sin introducir arquitecturas paralelas.
- Los planes se guardan en `.github/plans/` y siguen el formato de `.github/instructions/plans.instructions.md`.
- Implementación de planes (`.github/plans/`): un paso cada vez.
  1. Implementar solo el primer paso sin completar (sin `[x]`).
  2. Marcarlo en el plan (`[ ]` → `[x]`).
  3. Resumir en pocas líneas qué se ha cambiado y por qué; omitir el resumen si el cambio es trivial.
  4. Preguntar si continuar con el siguiente paso y detenerse hasta recibir respuesta. No adelantar trabajo del siguiente paso.
  - Si un paso es demasiado grande para completarlo de una vez, proponer dividirlo en subpasos y esperar confirmación antes de empezar.
  - Si un paso falla o se bloquea, detenerse e informar; no reintentar en bucle.

## Comandos
- Backend: `dotnet restore`, `dotnet build`, `dotnet test`.
- Migración: `dotnet ef migrations add <Name> --project src/OrganizeApp.Infrastructure --startup-project src/OrganizeApp.API`
- Frontend, desde `src/OrganizeApp.Frontend`: `npm ci`, `npm run dev`, `npm run lint`, `npm run build`.

## Prohibido
- No versionar secretos, cadenas de conexión ni `.env.local`.
- No editar `src/OrganizeApp.Infrastructure/Migrations/` a mano.
- No añadir paquetes NuGet/npm sin pedir autorización explícita.
- No hacer commits ni modificar el historial Git sin autorización explícita.
- Validar los cambios con las pruebas, build o lint apropiados antes de finalizar.

## Contexto de configuración local
- API HTTP: `http://localhost:5295`; HTTPS: `https://localhost:7049`.
- Frontend Vite: `http://localhost:5173`.
- La API usa rutas `/api`; el frontend debe configurar `VITE_BASE_API_URL` con el prefijo `/api`.
- La configuración de desarrollo no debe convertirse en secretos ni copiarse a archivos versionados.