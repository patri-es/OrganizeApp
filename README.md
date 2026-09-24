# Organize App
Creating WebApp with React and .NET for the backend, usign API with controllers and EF to manage the SQL Server Data Base.

## Resumen
- Aplicación tipo SPA (Frontend) + API REST (Backend) para gestión de productos/personas.
- Frontend: React + Vite + Tailwind. Backend: ASP.NET Core (target .NET 10) con Entity Framework Core y SQL Server (LocalDB).

## Arquitectura
- Cliente SPA (React) <-> API REST (ASP.NET Core)  
- El frontend consume la API del backend (CORS configurado para http://localhost:5173).
- Persistencia: EF Core con migrations (carpeta Migrations). Cadena por defecto apunta a LocalDB (appsettings.json).

### Estructura principal de carpetas
- /Frontend
  - package.json — scripts: dev, build, preview, lint
  - src/ — código React (main.jsx, App.jsx, componentes, páginas)
  - vite.config.js, index.html, src/index.css (Tailwind)
  - README.md (documentación del frontend)
- /Backend
  - Backend.csproj
  - Program.cs — arranque de la API y configuración (CORS, DbContext)
  - Controllers/ — ProductsController.cs, PeopleController.cs
  - Models/ — AppDbContext.cs, Product.cs, Person.cs
  - Migrations/ — migrations EF Core (InitialCreate ...)
  - appsettings.json / appsettings.Development.json
  - README.md (documentación del backend)
- OrganizeApp.sln — solución .NET en la raíz

### Tecnologías y librerías destacadas
- Frontend:
  - React 19, Vite, Tailwind CSS
  - axios (peticiones HTTP), react-router-dom, react-hook-form, react-hot-toast, lucide-react
  - eslint (calidad de código)
- Backend:
  - .NET 10, ASP.NET Core Web API
  - Entity Framework Core (migrations)
  - SQL Server LocalDB (cadena por defecto en appsettings.json)
- Herramientas:
  - Node.js + npm, .NET SDK (10.x), dotnet-ef (CLI para EF Core)

### Requisitos previos
- Node.js (recomendado ≥ 18)
- npm
- .NET SDK 10
- SQL Server Express / LocalDB (si usas la cadena por defecto)
- (Opcional) dotnet-ef: dotnet tool install --global dotnet-ef

## Instalación y ejecución (desde la raíz del repositorio)

### 1) Frontend
- Instalar dependencias:
  - cd Frontend
  - npm install
- Desarrollo (servidor con hot-reload):
  - npm run dev
  - Vite por defecto usa el puerto 5173 (el backend permite CORS desde http://localhost:5173).
- Build para producción:
  - npm run build
- Probar build localmente (preview):
  - npm run preview
- Linter:
  - npm run lint

### 2) Backend
- Restaurar y compilar:
  - cd Backend/Backend
  - dotnet restore
  - dotnet build
- Ejecutar API:
  - dotnet run
  - El comando mostrará la URL asignada por Kestrel (puedes usar la que aparezca en consola).
- Inicializar / aplicar migrations (desde Backend/Backend):
  - Asegúrate de tener dotnet-ef instalado globalmente si no está disponible:
    - dotnet tool install --global dotnet-ef
  - Aplicar migrations a la base de datos:
    - dotnet ef database update
  - Para crear nuevas migrations:
    - dotnet ef migrations add NombreMigracion
- Si prefieres ejecutar desde la raíz con ruta de proyecto:
  - dotnet run --project Backend/Backend

Ejecutar ambos en desarrollo
- Abre dos terminales:
  - Terminal A: cd Frontend && npm run dev
  - Terminal B: cd Backend/Backend && dotnet run

Actualizar dependencias y análisis
- Frontend:
  - npm update
  - npm audit fix
  - Re-ejecutar npm install si hace falta
- Backend:
  - dotnet restore
  - Para actualizar paquetes NuGet usa:
    - dotnet list package --outdated
    - dotnet add package <Paquete> --version <Versión>
  - Ejecuta pruebas estáticas / linters si están configurados

Configuración importante
- Conexión a base de datos en Backend/Backend/appsettings.json:
  - Actualmente usa LocalDB: "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=OrganizeApp; Integrated Security=True; ..."
  - Cambiar según entorno (usar variables de entorno en producción).
- CORS: Program.cs permite orígenes desde http://localhost:5173. Ajustar si cambias el puerto o dominio del frontend.

Resolución de problemas comunes
- Error EF: si dotnet ef no está disponible, instala dotnet-ef globalmente.
- Si la API no responde desde el frontend: comprobar URL y puerto del backend (consola dotnet run) y revisar CORS.
- Si el frontend no arranca: asegúrate de la versión de Node y de haber ejecutado npm install.

Notas finales
- Hay READMEs específicos en /Frontend y /Backend con detalles locales; revisarlos para instrucciones específicas del subproyecto.
- Este README es una guía de inicio rápido; adapta cadenas de conexión y variables de entorno para entornos de staging/producción.

Si quieres, puedo:
- Generar/actualizar el README.md en la raíz con este contenido (lo pego directamente en el archivo).
- Extraer y añadir comandos exactos de launchSettings.json o el README de Backend/Frontend si quieres mayor precisión.