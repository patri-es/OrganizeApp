# OrganizeApp
API REST .NET 10 + SPA React/Vite (JavaScript). Código en `src/`, pruebas en `test/`.

## Instrucciones por capa
- Backend (Domain, Application, Infrastructure, API, pruebas .NET): leer `.github/backend-instructions.md` antes de modificar código.
- Frontend (`src/OrganizeApp.Frontend`): leer `.github/frontend-instructions.md` antes de modificar código.
- Si un cambio afecta a ambos, leer los dos y mantener sincronizado el contrato HTTP.

## Convenciones
- Identificadores, tipos, endpoints y comentarios de código en inglés; documentación `.md` en español.
- Clean Architecture: `API -> Application + Infrastructure`, `Infrastructure -> Application`, `Application -> Domain`. Domain no depende de otras capas.
- En Application, organizar por feature (`Product`, `Category`).

## Planes
- Para crear o editar un plan, leer `.github/plans-instructions.md`. Los planes se guardan en `.github/plans/`.
- Implementar un paso cada vez:
  1. Implementar solo el primer paso sin completar (`[ ]`).
  2. Validar con el build o los tests afectados, no con la batería completa.
  3. Marcarlo `[x]` en el plan.
  4. Resumir en pocas líneas qué cambió y por qué (omitir si es trivial).
  5. Preguntar si continuar y detenerse hasta recibir respuesta. No adelantar trabajo del siguiente paso.
- Si un paso es demasiado grande, proponer dividirlo en subpasos y esperar confirmación.
- Si un paso falla o se bloquea, detenerse e informar; no reintentar en bucle.

## Prohibido
- No versionar secretos, cadenas de conexión ni `.env.local`.
- No editar `src/OrganizeApp.Infrastructure/Migrations/` a mano.
- No añadir paquetes NuGet/npm sin autorización explícita.
- No hacer commits ni modificar el historial Git sin autorización explícita.

## Configuración local
- `VITE_BASE_API_URL` debe incluir el prefijo `/api`, por ejemplo `https://localhost:7049/api`.