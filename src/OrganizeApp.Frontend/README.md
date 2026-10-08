# OrganizeApp.Frontend

Frontend de **OrganizeApp**, una aplicación web de una sola página (SPA) para gestionar los recursos de la aplicación mediante la API de OrganizeApp.

## Requisitos

- Node.js en una versión LTS reciente, que incluye `npm`.
- .NET SDK 10, necesario para ejecutar la API que proporciona los datos al frontend.

## Instalación

Desde la raíz del repositorio:

```bash
cd src/OrganizeApp.Frontend
npm ci
```

`npm ci` utiliza el archivo `package-lock.json` para instalar versiones reproducibles. Si el archivo de bloqueo no está disponible o se han modificado las dependencias, puede utilizarse `npm install`.

### Limpieza de caché y reinstalación
Si es necesario reiniciar la caché por completo, lo mejor es: 
1. Borrar la carpeta `node_modules` 
2. ejecutar los comandos `npm install`, esto volverá a instalar todos los paquetes necesarios
3. a continuación `npm run dev`, ejecutará la aplicación 

## Configuración de la API

El frontend necesita que la API de OrganizeApp esté ejecutándose para cargar y modificar datos. La URL base se obtiene mediante la variable de entorno `VITE_BASE_API_URL`. Esta variable no configura el CORS: indica al frontend dónde debe realizar las peticiones HTTP, mientras que el CORS de la API autoriza el origen del frontend (`http://localhost:5173`).

En el desarrollo local, crea un archivo `.env.local` dentro de esta carpeta con la URL HTTP definida por el perfil de desarrollo de la API:

```env
VITE_BASE_API_URL=http://localhost:5295
```

La URL del ejemplo no es diferente de la utilizada por la API; es la dirección base de la API local. El archivo `.env.local` es necesario con la implementación actual, porque los componentes utilizan directamente `import.meta.env.VITE_BASE_API_URL`.

La API puede iniciarse desde la raíz del repositorio con:

```bash
dotnet run --project src/OrganizeApp.API
```

Si se utiliza otro perfil o puerto, ajusta el valor de `VITE_BASE_API_URL` y reinicia el servidor de Vite. La API también dispone de un perfil HTTPS (`https://localhost:7049`); en ese caso, actualiza la variable y asegúrate de que el certificado de desarrollo de .NET sea de confianza.

## Ejecución

Para iniciar el servidor de desarrollo:

```bash
npm run dev
```

Vite abre el navegador automáticamente. Por defecto, la aplicación queda disponible en `http://localhost:5173`.

## Scripts disponibles

| Comando | Descripción |
| --- | --- |
| `npm run dev` | Inicia el servidor de desarrollo con recarga en caliente. |
| `npm run build` | Genera la compilación de producción en `dist`. |
| `npm run preview` | Sirve localmente la compilación generada. |
| `npm run lint` | Analiza el código con ESLint. |

## Tecnologías principales

- **React 19** y **React DOM** para la interfaz de usuario.
- **Vite** como servidor de desarrollo y herramienta de compilación.
- **React Router DOM** para la navegación de la SPA.
- **Tailwind CSS** para los estilos, integrado mediante el plugin de Vite.
- **Axios** para las peticiones HTTP a la API.
- **React Hook Form** para la gestión de formularios.
- **React Hot Toast** para las notificaciones y **Lucide React** para los iconos.
- **ESLint** para el análisis estático del código.

Las dependencias y sus versiones se encuentran definidas en `package.json`; no es necesario instalarlas individualmente.

## Estructura

El código de la aplicación se encuentra en `src/`. Las páginas, componentes y estilos están organizados por funcionalidad. La aplicación utiliza React Router y actualmente incluye vistas para inicio, información, personas, productos y categorías.

```text
OrganizeApp.Frontend/
├── public/                    # Recursos estáticos públicos
├── src/
│   ├── assets/                # Recursos gráficos
│   ├── components/            # Componentes reutilizables y de gestión
│   │   ├── categories/
│   │   ├── person/
│   │   └── product/Product.jsx, ProductForm.jsx, ProductCard.jsx, ProductList.jsx
│   ├── pages/                 # Vistas principales de la aplicación
│   │   ├── About.jsx
│   │   ├── Home.jsx
│   │   └── NotFound.jsx
│   ├── App.jsx                # Componente raíz y rutas
│   ├── index.css              # Estilos globales y Tailwind CSS
│   └── main.jsx               # Punto de entrada
├── .env.local                 # Configuración local de la API (no versionar)
├── package.json               # Scripts y dependencias
├── package-lock.json          # Versiones bloqueadas de npm
└── vite.config.js             # Configuración de Vite, React y Tailwind
```
