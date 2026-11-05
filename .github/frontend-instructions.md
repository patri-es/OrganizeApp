# Instrucciones de trabajo: Frontend

## Alcance
Estas instrucciones aplican a `src/OrganizeApp.Frontend` y, especialmente, a `src/OrganizeApp.Frontend/src`.

## Contexto que debe conocerse antes de modificar código
- Es una SPA React 19 con JavaScript, Vite 8 y módulos ES.
- La navegación usa `react-router-dom`; las rutas principales se declaran en `src/App.jsx`.
- La UI se organiza en `pages`, `components` por feature y `services` para acceso HTTP.
- Las features actuales son productos, categorías y una sección de personas en evolución.
- La comunicación con el backend debe pasar por `src/services/api.js` y los servicios de feature (`productService.js`, `categoryService.js`); no llamar Axios directamente desde componentes nuevos.
- Axios usa `VITE_BASE_API_URL`, timeout de 10 segundos, JSON y serialización de parámetros sin índices.
- La API expone sus recursos bajo `/api`; por tanto, el valor normal de desarrollo debe incluir ese prefijo, por ejemplo `VITE_BASE_API_URL=http://localhost:5295/api` o `https://localhost:7049/api`.
- El CORS lo controla el backend y no se resuelve desde Vite. El origen local esperado es `http://localhost:5173`.

## Reglas de implementación
- Mantener identificadores y nombres de componentes, funciones, props y archivos en inglés.
- Usar componentes funcionales y hooks de React siguiendo el estilo existente.
- Mantener la separación entre presentación, estado de pantalla y servicios HTTP.
- Reutilizar `api.js`, los helpers de errores y las notificaciones existentes antes de crear mecanismos alternativos.
- Usar `react-hook-form` para formularios nuevos cuando exista un formulario equivalente; conservar validación y mensajes consistentes.
- Usar `react-hot-toast` para feedback de operaciones y estados de error visibles; no silenciar errores de red.
- Mantener accesibilidad básica: labels asociados, controles con nombres claros, estados de carga y mensajes de error comprensibles.
- No duplicar URLs, nombres de endpoints ni transformaciones de DTO en componentes.
- Respetar el contrato real de la API: productos usan `PATCH` para edición y categorías usan `PUT`; no cambiar verbos o payloads sin actualizar backend, servicios y pruebas/documentación.
- No añadir dependencias npm sin pedir autorización explícita. Si se autoriza una dependencia, actualizar `package.json` y `package-lock.json` mediante npm.
- No versionar secretos ni `.env.local`; las variables `VITE_*` se consideran configuración pública del cliente y no deben contener credenciales.
- No crear commits ni modificar historial Git sin autorización explícita.

## Convenciones visuales y técnicas
- Tailwind CSS se integra mediante `@tailwindcss/vite`; preferir utilidades y patrones ya usados en la pantalla afectada.
- Los iconos proceden de `lucide-react`.
- Mantener la estructura por feature en `src/components/product` y `src/components/categories`.
- Revisar `src/utils/errorHelper.js` antes de introducir una nueva forma de mostrar errores.
- Actualizar rutas, navegación y estado de página de forma consistente cuando se añada una pantalla.
- Evitar introducir TypeScript, Redux u otra arquitectura global sin una decisión explícita del proyecto.

## Contrato de servicios actual
- `productService.js` centraliza consulta, alta, edición, borrado, stock, disponibilidad y relación producto-categoría.
- `categoryService.js` centraliza consulta, alta, edición y borrado de categorías.
- Las respuestas Axios mantienen la forma de Axios; transformar datos en el servicio o en el límite de consumo, siguiendo el patrón existente.
- Las rutas y nombres de propiedades deben contrastarse con los DTOs de `src/OrganizeApp.Application` antes de cambiar formularios o tarjetas.

## Desarrollo y validación
- Desde `src/OrganizeApp.Frontend`: `npm ci`, `npm run dev`, `npm run lint`, `npm run build`.
- Para probar operaciones reales, iniciar también `src/OrganizeApp.API` y verificar la URL de `VITE_BASE_API_URL`.
- Después de modificar un servicio o flujo de formulario, comprobar estados de éxito, error, carga y datos vacíos.
- Corregir errores introducidos por ESLint o por la compilación; no reformatear archivos no relacionados.

## Archivos que conviene leer antes de implementar
- `.github/copilot-instructions.md`, `README.md` y `src/OrganizeApp.Frontend/README.md`.
- `src/App.jsx`, `src/main.jsx` y `src/components/Navbar.jsx` para rutas y composición global.
- El componente de la feature afectada y sus formularios/listas.
- `src/services/api.js` y el servicio correspondiente.
- `src/utils/errorHelper.js` y la configuración de ESLint/Vite.
- El controlador y DTOs de backend si cambia el consumo de la API.
