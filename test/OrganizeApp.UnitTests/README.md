# Test unitarios

## 1. Pruebas unitarias de Dominio
Comprueban reglas concretas de una clase sin depender de la base de datos ni de la API.

Ejemplos: un producto no admite un precio negativo; reducir el stock a cero lo deja no disponible; no se puede activar sin stock o sin categorías.

- Comenzamos con el test para verificar la creación de un nuevo producto. 

### Definiciones para las pruebas:
Para definir los métodos que realizan las pruebas podemos utilizar una convención habitual como:
Podría ser de la siguiente forma (varios ejemplos):

- **Método que se prueba + situación + resultado esperado**

``` 
CreateProduct_WithValidData_SetsInitialValues()
CreateProduct_WithNegativePrice_ThrowsException()
CreateProduct_WithoutName_ThrowsException()
CreateProduct_WithoutImage_ThrowsException()
CreateProduct_WithValidData_NewProductWithInitialValues()  
```
- En el caso de crear producto, en la entidad se trata del constructor
porque define la construcción de un producto, por lo que, createProduct en nuestra entidad es el constructor
- Por eso, tendría sentido nombrar a los test así: 
``` 
Constructor_WithValidData_CreatesProductWithInitialValues()   
Constructor_WithNegativePrice_ThrowsProductDomainException()
```

### Para ejecutar las pruebas: 
```dotnet test test/OrganizeApp.UnitTests/OrganizeApp.UnitTests.csproj```

- Al usar el paquete XUnit.v3, es necesario especificar el motor de ejecución para los test (si queremos usarlo por consola)

```
{
  "test": {
    "runner": "Microsoft.Testing.Platform"
  }
}
```
### - Posible lista de próximos test

Leyenda: P0 = esenciales · P1 = importantes · P2 = complementarios

#### A. Constructor y valores iniciales
- [x] P0 — Crea un producto válido con sus datos correctos e inicializa Stock a 0 e IsAvailable a false.
- [x] P0 — Rechaza un nombre null, vacío o compuesto únicamente por espacios.
- [x] P0 — Rechaza un precio negativo.
- [x] P0 — Acepta un precio igual a cero.
- [x] P1 — Acepta una descripción null, vacía o compuesta únicamente por espacios.
- [x] P1 — Acepta una ImageUrl null, vacía o compuesta únicamente por espacios.
- [x] P1 — Acepta un nombre con la longitud máxima permitida (100 caracteres).
- [x] P1 — Rechaza un nombre que supera la longitud máxima (101 caracteres).
- [x] P1 — Acepta una descripción con la longitud máxima permitida (500 caracteres).
- [x] P1 — Rechaza una descripción que supera la longitud máxima (501 caracteres).
- [x] P1 — Acepta una ImageUrl con la longitud máxima permitida (500 caracteres).
- [x] P1 — Rechaza una ImageUrl que supera la longitud máxima (501 caracteres).
- [x] P2 — Inicializa Categories como una colección vacía.

#### B. Modificación de datos
- [x] P0 — ChangeName actualiza el nombre con un valor válido.
- [x] P0 — ChangeName rechaza un nombre null, vacío o compuesto únicamente por espacios.
- [x] P0 — ChangeName conserva el nombre anterior si la validación falla.
- [x] P1 — ChangeName acepta un nombre de 100 caracteres.
- [x] P1 — ChangeName rechaza un nombre de 101 caracteres.
- [x] P1 — ChangeDescription actualiza una descripción válida.
- [x] P1 — ChangeDescription permite establecer Description a null.
- [x] P1 — ChangeDescription rechaza una descripción que supera los 500 caracteres.
- [x] P1 — ChangeImage actualiza una ImageUrl válida.
- [x] P1 — ChangeImage permite establecer ImageUrl a null.
- [x] P1 — ChangeImage rechaza una ImageUrl que supera los 500 caracteres.
- [x] P0 — ChangePrice rechaza un precio negativo y conserva el precio anterior.
- [x] P1 — ChangePrice actualiza el precio con un valor válido.

#### C. Gestión del stock
- [x] P0 — IncreaseStock aumenta el stock con una cantidad válida.
- [x] P0 — IncreaseStock rechaza una cantidad igual a cero.
- [x] P0 — IncreaseStock rechaza una cantidad negativa.
- [x] P0 — DecreaseStock reduce el stock con una cantidad válida.
- [x] P0 — DecreaseStock rechaza una cantidad igual a cero.
- [x] P0 — DecreaseStock rechaza una cantidad negativa.
- [x] P0 — DecreaseStock rechaza una cantidad superior al stock actual.
- [x] P0 — DecreaseStock permite reducir el stock exactamente a cero.
- [x] P0 — DecreaseStock desactiva el producto cuando el stock llega a cero.

#### D. Disponibilidad
- [x] P0 — Activate activa un producto con stock y al menos una categoría.
- [x] P0 — Activate rechaza un producto sin stock.
- [x] P0 — Activate rechaza un producto sin categorías.
- [x] P1 — Deactivate desactiva un producto activo.

#### E. Gestión de categorías
- [x] P1 — AssignCategory añade una categoría al producto.
- [x] P1 — AssignCategory evita asignar dos veces la misma categoría.
- [x] P1 — RemoveCategory elimina una categoría asignada.
- [x] P1 — RemoveCategory no modifica el producto si la categoría no está asignada.
- [x] P0 — RemoveCategory desactiva el producto cuando se elimina su última categoría.


## 2. Pruebas unitarias para la capa de Application

#### A. Consultas de productos
- [ ] P0 — Obtiene todos los productos y devuelve sus datos en una lista de `ProductDTO`.
- [ ] P0 — Mapea correctamente las categorías asociadas a cada producto en su `ProductDTO`.
- [ ] P0 — Devuelve una lista vacía cuando no existen productos.
- [ ] P0 — Envía los filtros recibidos al repositorio al consultar productos.
- [ ] P0 — Obtiene un producto por su identificador y devuelve su `ProductDTO`.
- [ ] P0 — Devuelve `null` cuando no existe un producto con el identificador solicitado.
- [ ] P1 — Devuelve correctamente un producto sin categorías asociadas.

#### B. Creación de productos
- [ ] P0 — Crea un producto con los datos del `CreateProductDTO` y lo envía al repositorio.
- [ ] P0 — Guarda los cambios después de crear el producto.
- [ ] P1 — Propaga la excepción de dominio cuando los datos recibidos no son válidos.
- [ ] P1 — No guarda cambios si falla la creación del producto.

#### C. Eliminación de productos
- [ ] P0 — Elimina un producto existente y guarda los cambios.
- [ ] P0 — Lanza una excepción cuando el producto no existe.
- [ ] P1 — No guarda cambios cuando el producto no existe.

#### D. Actualización de productos
- [ ] P0 — Actualiza el nombre, la descripción, el precio y la imagen de un producto existente.
- [ ] P0 — Guarda los cambios después de actualizar el producto.
- [ ] P0 — Lanza `NotFoundException` cuando el producto no existe.
- [ ] P1 — Propaga la excepción de dominio cuando alguno de los datos no es válido.
- [ ] P1 — No guarda cambios cuando la actualización falla por una excepción de dominio.

#### E. Gestión del stock
- [ ] P0 — Aumenta el stock de un producto existente y guarda los cambios.
- [ ] P0 — Reduce el stock de un producto existente y guarda los cambios.
- [ ] P0 — Lanza `NotFoundException` al aumentar el stock de un producto inexistente.
- [ ] P0 — Lanza `NotFoundException` al reducir el stock de un producto inexistente.
- [ ] P1 — Propaga la excepción de dominio cuando la cantidad para aumentar el stock no es válida.
- [ ] P1 — Propaga la excepción de dominio cuando la cantidad para reducir el stock no es válida.
- [ ] P1 — No guarda cambios cuando una operación de stock falla por una excepción de dominio.

#### F. Disponibilidad de productos
- [ ] P0 — Activa un producto que tiene stock y categorías asociadas, y guarda los cambios.
- [ ] P0 — Lanza `NotFoundException` al intentar activar un producto inexistente.
- [ ] P0 — Desactiva un producto existente y guarda los cambios.
- [ ] P0 — Lanza `NotFoundException` al intentar desactivar un producto inexistente.
- [ ] P1 — Propaga la excepción de dominio al activar un producto sin stock.
- [ ] P1 — Propaga la excepción de dominio al activar un producto sin categorías.
- [ ] P1 — No guarda cambios cuando la activación falla por una excepción de dominio.

#### G. Asignación y eliminación de categorías
- [ ] P0 — Asigna una categoría existente a un producto existente y guarda los cambios.
- [ ] P0 — Lanza `NotFoundException` cuando el producto no existe al asignar una categoría.
- [ ] P0 — Lanza `NotFoundException` cuando la categoría no existe al asignarla.
- [ ] P0 — Elimina la asociación entre un producto y una categoría existentes, y guarda los cambios.
- [ ] P0 — Lanza una excepción cuando el producto no existe al eliminar una categoría.
- [ ] P0 — Lanza una excepción cuando la categoría no existe al eliminarla del producto.
- [ ] P1 — Propaga la excepción de dominio si la asignación o eliminación de una categoría incumple una regla de dominio.
- [ ] P1 — No guarda cambios cuando falla la operación de asignación o eliminación.