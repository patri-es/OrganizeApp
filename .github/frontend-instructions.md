# Frontend
Aplica a `src/OrganizeApp.Frontend`.

## Convenciones
- Todo el acceso HTTP pasa por `src/services` (`api.js`, `productService.js`, `categoryService.js`). No llamar a Axios desde componentes.
- Estructura: `pages` (estado de pantalla), `components/<feature>` (presentación; datos y callbacks por props), `services` (HTTP).
- Formularios nuevos con `react-hook-form`; feedback de éxito y error con `react-hot-toast`. Los errores de API se muestran con `src/utils/errorHelper.js`; no silenciar errores de red.
- Estilos con Tailwind (siguiendo la pantalla afectada) e iconos de `lucide-react`.
- Contrato real: los productos se editan con `PATCH` y las categorías con `PUT`. No cambiar verbos ni payloads sin actualizar backend y servicios.
- Contrastar rutas y nombres de propiedades con los DTOs de `src/OrganizeApp.Application` antes de tocar formularios o tarjetas.
- No duplicar URLs, endpoints ni transformaciones de DTO en componentes.
- Al añadir una pantalla, actualizar las rutas en `src/App.jsx` y la navegación en `src/components/Navbar.jsx`.
- No introducir TypeScript, Redux ni otra arquitectura global sin una decisión explícita.

## Comandos y validación
- Desde `src/OrganizeApp.Frontend`: `npm ci`, `npm run dev`, `npm run lint`, `npm run build`.
- Al terminar, validar con `npm run lint` y `npm run build`.
- Tras cambiar un servicio o formulario, comprobar los estados de éxito, error, carga y datos vacíos.
- No reformatear archivos no relacionados.