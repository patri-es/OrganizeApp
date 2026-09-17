# Backend
Para crear este proyecto, hemos creado la carpeta con el nombre del proyecto

## Requisitos:
- DotNet 10


## Paquetes
- Instalado: Microsoft.EntityFrameworkCore.Sqlite

## Carpetas y archivos
La estructura de archivos es la siguiente:

	- ProductCrudApp 
		- .gitIgnore
		- .git/
		- / Backend /
				- Backend.slnx
				- / Backend / 
							- appsettings.Development.json
							- appsettings.json
							- Backend.csproj
							- Backend.csproj.user
							- Program.cs
							- README.md (este)
									
--> Y dentro de la carpeta Backend:

	- / bin /
	- / Controllers / controladores para las APIs
	- / Models / clases para entidades + --> AppDbContext 
	- / obj /
	- / Properties /


## EntityFramework
Instalados los paquetes:
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Design
 
Tras crear el archivo y clase AppDbContext, este implementa las DbContext, y tiene un constructor para agregar las DbContextOptions.
A continuación define cuales serán los nombres de las tablas que creará para las entidades añadidas: 

    public DbSet<Person> People { get; set; }
    public DbSet<Product> Products { get; set; }

Una vez definida la clase AppDbContext y añadida a Program.cs:

	// Database connection
	string connectionString =
		builder.Configuration.GetConnectionString("Default")
		?? throw new ArgumentNullException(
			"Connection String is not available"
		);

	builder.Services.AddDbContext<AppDbContext>(options => 
		options.UseSqlServer(connectionString));

- Se añade en appsettings.json la conexión a la BBDD:
	
```
	"AllowedHosts": "*",
    "ConnectionStrings": {
      "Default": "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=OrganizeApp; Integrated Security= True; Connect Timeout= 30; Encrypt=False; TrustServerCertificate=False; ApplicationIntent= ReadWrite; MultiSubnetFailover= False"
    }
```

- Finalmente, ejecutamos el siguiente comando que generará las operaciones necesarias para crear las tablas según el modelo actual, siempre que no existan configuraciones adicionales que cambien ese comportamiento.

```
	...\ProductCrudApp\Backend\Backend> dotnet ef migrations add InitialCreate
	Build started...
	Build succeeded.
	Done. To undo this action, use 'ef migrations remove'
```
*Tener en cuenta el comentario que devuelve: Si quieres quitar las migraciones, ejecutar: ```ef migrations remove```
- Este comando ha creado el directorio: *Migrations* con dos archivos: AppDbContextModelSnapshot.cs y 20260917090919_InitialCreate.cs
- Pensaba que crearía las tablas, pero no, ha definido la clase ```InitialCreate.``` con los métodos para generar las tablas para las dos entidades que tenemos actualmente.
- Para generar estas tablas con las propiedades y los tipos definidos en nuestras clases, tenemos que ejecutar el siguiente comando
```dotnet ef database update```
- Este comando creará tantas tablas como entidades hayamos definido, y que se añadieron a los métodos autogenerados con la clase InitialCreate
- Además creará una tabla llamada "__EFMugrationsHistory" con dos columnas: "MigrationId" (PK) y "ProductVersion" que no permite valores nulos.


## Versión de nuestra App