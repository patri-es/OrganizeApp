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
### - lista de test

### - Lista de pruebas unitarias

Leyenda: P0 = esenciales · P1 = importantes · P2 = complementarios

## 1. Pruebas unitarias para la capa de Domain

### A. Product — Constructor y valores iniciales

- [x] P0 — `Constructor_WithValidData_CreatesProductWithInitialValues`: crea un producto válido e inicializa Stock a 0 e IsAvailable a false.
- [x] P0 — `Constructor_WithInvalidName_ThrowsProductDomainException`: rechaza un nombre null, vacío o compuesto únicamente por espacios.
- [x] P0 — `Constructor_WithNegativePrice_ThrowsProductDomainException`: rechaza un precio negativo.
- [x] P0 — `Constructor_WithZeroPrice_CreatesProduct`: acepta un precio igual a cero.
- [x] P1 — `Constructor_WithOptionalDescription_CreatesProduct`: acepta una descripción null, vacía o compuesta únicamente por espacios.
- [x] P1 — `Constructor_WithOptionalImageUrl_CreatesProduct`: acepta una ImageUrl null, vacía o compuesta únicamente por espacios.
- [x] P1 — `Constructor_WithNameAtMaximumLength_CreatesProduct`: acepta un nombre de 100 caracteres.
- [x] P1 — `Constructor_WithNameExceedingMaximumLength_ThrowsProductDomainException`: rechaza un nombre de 101 caracteres.
- [x] P1 — `Constructor_WithDescriptionAtMaximumLength_CreatesProduct`: acepta una descripción de 500 caracteres.
- [x] P1 — `Constructor_WithDescriptionExceedingMaximumLength_ThrowsProductDomainException`: rechaza una descripción de 501 caracteres.
- [x] P1 — `Constructor_WithImageUrlAtMaximumLength_CreatesProduct`: acepta una ImageUrl de 500 caracteres.
- [x] P1 — `Constructor_WithImageUrlExceedingMaximumLength_ThrowsProductDomainException`: rechaza una ImageUrl de 501 caracteres.
- [x] P2 — `Constructor_WithValidData_InitializesCategoriesAsEmptyCollection`: inicializa Categories como una colección vacía.

### B. Product — Modificación de datos

- [x] P0 — `ChangeName_WithValidName_UpdatesProductName`: actualiza el nombre con un valor válido.
- [x] P0 — `ChangeName_WithInvalidName_ThrowsProductDomainException`: rechaza un nombre null, vacío o compuesto únicamente por espacios y conserva el anterior.
- [x] P1 — `ChangeName_WithNameAtMaximumLength_UpdatesProductName`: acepta un nombre de 100 caracteres.
- [x] P1 — `ChangeName_WithNameExceedingMaximumLength_ThrowsProductDomainException`: rechaza un nombre de 101 caracteres y conserva el anterior.
- [x] P1 — `ChangeDescription_WithOptionalDescription_UpdatesProductDescription`: permite establecer una descripción válida, vacía, con espacios o null.
- [x] P1 — `ChangeDescription_WithDescriptionAtMaximumLength_UpdatesProductDescription`: acepta una descripción de 500 caracteres.
- [x] P1 — `ChangeDescription_WithDescriptionExceedingMaximumLength_ThrowsProductDomainException`: rechaza una descripción de 501 caracteres y conserva la anterior.
- [x] P1 — `ChangeImageUrl_WithOptionalImageUrl_UpdatesProductImageUrl`: permite establecer una ImageUrl válida, vacía, con espacios o null.
- [x] P1 — `ChangeImageUrl_WithImageUrlAtMaximumLength_UpdatesProductImageUrl`: acepta una ImageUrl de 500 caracteres.
- [x] P1 — `ChangeImageUrl_WithImageUrlExceedingMaximumLength_ThrowsProductDomainException`: rechaza una ImageUrl de 501 caracteres y conserva la anterior.
- [x] P0 — `ChangePrice_WithNegativeValue_ThrowsProductDomainException`: rechaza un precio negativo y conserva el anterior.
- [x] P1 — `ChangePrice_WithValidPrice_UpdatesProductPrice`: actualiza el precio con un valor válido, incluido cero.

### C. Product — Gestión del stock

- [x] P0 — `IncreaseStock_WithValidQuantity_IncreasesStock`: aumenta el stock con una cantidad válida.
- [x] P0 — `IncreaseStock_WithInvalidQuantity_ThrowsProductDomainException`: rechaza una cantidad igual a cero o negativa.
- [x] P0 — `DecreaseStock_WithValidQuantity_DecreasesStock`: reduce el stock con una cantidad válida.
- [x] P0 — `DecreaseStock_WithInvalidQuantity_ThrowsProductDomainException`: rechaza una cantidad igual a cero o negativa.

- [ ] P0 — Pendiente: comprobar que `DecreaseStock` rechaza una cantidad superior al stock actual.
- [ ] P0 — Pendiente: comprobar que `DecreaseStock` permite reducir el stock exactamente a cero.
- [ ] P0 — Pendiente: comprobar que `DecreaseStock` desactiva el producto cuando el stock llega a cero.

### D. Product — Disponibilidad

- [x] P0 — `Activate_WithStockAndCategories_SetsIsAvailableToTrue`: activa un producto con stock y al menos una categoría.
- [x] P0 — `Activate_WithoutStock_ThrowsProductDomainException`: rechaza la activación de un producto sin stock.
- [x] P0 — `Activate_WithoutCategories_ThrowsProductDomainException`: rechaza la activación de un producto sin categorías.
- [x] P1 — `Deactivate_WhenProductIsActive_SetsIsAvailableToFalse`: desactiva un producto activo.

### E. Product — Gestión de categorías

- [x] P1 — `AssignCategory_WithValidCategory_AddsCategoryToProduct`: asigna una categoría al producto.
- [x] P1 — `AssignCategory_WithSameCategoryTwice_ThrowsProductDomainException`: lanza una excepción de dominio al asignar una categoría duplicada y no modifica la colección.
- [x] P1 — `AssignCategory_WithDifferentCategories_AddsBothCategories`: permite asignar categorías diferentes al mismo producto.
- [x] P1 — `RemoveCategory_WithExistingCategory_RemovesCategoryFromProduct`: elimina una categoría asignada.
- [x] P1 — `RemoveCategory_WithCategoryNotAssigned_ThrowsProductDomainException`: lanza una excepción de dominio si la categoría no está asignada y conserva las asociaciones existentes.
- [x] P0 — `RemoveCategory_WhenLastCategoryIsRemoved_SetsIsAvailableToFalse`: desactiva el producto al eliminar su última categoría.
- [x] P1 — `RemoveCategory_WhenOtherCategoriesRemain_KeepsProductAvailable`: mantiene el producto disponible cuando conserva otras categorías.

### F. Category — Constructor

- [x] P0 — `Constructor_WithValidData_CreatesCategoryWithInitialValues`: crea una categoría válida con sus datos iniciales y una colección Products vacía.
- [x] P0 — `Constructor_WithInvalidName_ThrowsCategoryDomainException`: rechaza un nombre null, vacío o compuesto únicamente por espacios.
- [x] P0 — `Constructor_WithInvalidCode_ThrowsCategoryDomainException`: rechaza un código null, vacío o compuesto únicamente por espacios.
- [x] P0 — `Constructor_WithValidCodeFormat_CreatesCategory`: acepta códigos con formato válido, incluidas letras mayúsculas, números, guiones y guiones bajos.
- [x] P0 — `Constructor_WithInvalidCodeFormat_ThrowsCategoryDomainException`: rechaza códigos con minúsculas, espacios, puntos, símbolos no permitidos o caracteres acentuados.
- [x] P1 — `Constructor_WithCodeAtMaximumLength_CreatesCategory`: acepta un código de 20 caracteres.
- [x] P1 — `Constructor_WithCodeExceedingMaximumLength_ThrowsCategoryDomainException`: rechaza un código de 21 caracteres.
- [x] P1 — `Constructor_WithNameAtMaximumLength_CreatesCategory`: acepta un nombre de 100 caracteres.
- [x] P1 — `Constructor_WithNameExceedingMaximumLength_ThrowsCategoryDomainException`: rechaza un nombre de 101 caracteres.
- [x] P1 — `Constructor_WithOptionalDescription_CreatesCategory`: acepta una descripción null, vacía o compuesta únicamente por espacios.
- [x] P1 — `Constructor_WithDescriptionAtMaximumLength_CreatesCategory`: acepta una descripción de 500 caracteres.
- [x] P1 — `Constructor_WithDescriptionExceedingMaximumLength_ThrowsCategoryDomainException`: rechaza una descripción de 501 caracteres.
- [x] P1 — `Constructor_WithOptionalImageUrl_CreatesCategory`: acepta una ImageUrl null, vacía o compuesta únicamente por espacios.
- [x] P1 — `Constructor_WithImageUrlAtMaximumLength_CreatesCategory`: acepta una ImageUrl de 500 caracteres.
- [x] P1 — `Constructor_WithImageUrlExceedingMaximumLength_ThrowsCategoryDomainException`: rechaza una ImageUrl de 501 caracteres.

### G. Category — Modificación de datos

- [x] P0 — `ChangeName_WithValidName_UpdatesCategoryName`: actualiza el nombre con un valor válido.
- [x] P0 — `ChangeName_WithInvalidName_ThrowsCategoryDomainException`: rechaza un nombre null, vacío o compuesto únicamente por espacios y conserva el anterior.
- [x] P1 — `ChangeName_WithNameAtMaximumLength_UpdatesCategoryName`: acepta un nombre de 100 caracteres.
- [x] P1 — `ChangeName_WithNameExceedingMaximumLength_ThrowsCategoryDomainException`: rechaza un nombre de 101 caracteres y conserva el anterior.
- [x] P0 — `ChangeCode_WithValidCode_UpdatesCategoryCode`: actualiza el código con un valor válido.
- [x] P0 — `ChangeCode_WithInvalidCode_ThrowsCategoryDomainException`: rechaza un código null, vacío o compuesto únicamente por espacios y conserva el anterior.
- [x] P1 — `ChangeCode_WithCodeAtMaximumLength_UpdatesCategoryCode`: acepta un código de 20 caracteres.
- [x] P1 — `ChangeCode_WithCodeExceedingMaximumLength_ThrowsCategoryDomainException`: rechaza un código de 21 caracteres y conserva el anterior.
- [x] P0 — `ChangeCode_WithValidCodeFormat_UpdatesCategoryCode`: acepta los formatos de código válidos.
- [x] P0 — `ChangeCode_WithInvalidCodeFormat_ThrowsCategoryDomainException`: rechaza los formatos de código no permitidos y conserva el anterior.
- [x] P1 — `ChangeDescription_WithOptionalDescription_UpdatesCategoryDescription`: permite establecer una descripción válida, vacía, con espacios o null.
- [x] P1 — `ChangeDescription_WithDescriptionAtMaximumLength_UpdatesCategoryDescription`: acepta una descripción de 500 caracteres.
- [x] P1 — `ChangeDescription_WithDescriptionExceedingMaximumLength_ThrowsCategoryDomainException`: rechaza una descripción de 501 caracteres y conserva la anterior.
- [x] P1 — `ChangeImageUrl_WithOptionalImageUrl_UpdatesCategoryImageUrl`: permite establecer una ImageUrl válida, vacía, con espacios o null.
- [x] P1 — `ChangeImageUrl_WithImageUrlAtMaximumLength_UpdatesCategoryImageUrl`: acepta una ImageUrl de 500 caracteres.
- [x] P1 — `ChangeImageUrl_WithImageUrlExceedingMaximumLength_ThrowsCategoryDomainException`: rechaza una ImageUrl de 501 caracteres y conserva la anterior.

## 2. Pruebas unitarias para la capa de Application

### A. Consultas de productos

- [x] P0 — `GetProducts_WithProducts_ReturnsMappedProductDTOs`: obtiene los productos y devuelve sus datos en una lista de `ProductDTO`.
- [x] P0 — `GetProducts_WithAssociatedCategories_ReturnsMappedCategories`: mapea las categorías asociadas a cada producto en su DTO.
- [x] P0 — `GetProducts_WhenNoProductsExist_ReturnsEmptyList`: devuelve una lista vacía cuando no existen productos.
- [x] P0 — `GetProducts_WithFilters_PassesFiltersToRepository`: envía los filtros recibidos al repositorio.
- [x] P0 — `GetProductById_WhenProductExists_ReturnsMappedProductDTO`: obtiene un producto por su identificador y devuelve su DTO.
- [x] P0 — `GetProductById_WhenProductDoesNotExist_ReturnsNull`: devuelve null cuando no existe el producto solicitado.
- [x] P1 — `GetProducts_WithProductWithoutCategories_ReturnsEmptyCategoryList`: devuelve un producto con una lista de categorías vacía cuando no tiene asociaciones.

### B. Creación de productos

- [x] P0 — `CreateProduct_WithValidData_CreatesAndSendsProductToRepository`: crea un producto a partir de `CreateProductDTO` y lo envía al repositorio.
- [x] P0 — `CreateProduct_WithValidData_SavesChanges`: guarda los cambios después de crear el producto.
- [x] P1 — `CreateProduct_WithInvalidData_ThrowsProductDomainException`: propaga la excepción de dominio si los datos no son válidos.
- [x] P1 — `CreateProduct_WhenCreationFails_DoesNotSaveChanges`: no guarda cambios si falla la creación.

### C. Eliminación de productos

- [x] P0 — `DeleteProduct_WhenProductExists_DeletesProductAndSavesChanges`: elimina un producto existente y guarda los cambios.
- [x] P0 — `DeleteProduct_WhenProductDoesNotExist_ThrowsException`: lanza `NotFoundException` cuando el producto no existe.
- [x] P1 — `DeleteProduct_WhenProductDoesNotExist_DoesNotSaveChanges`: no elimina ni guarda cambios cuando el producto no existe.

### D. Actualización de productos

- [x] P0 — `UpdateProduct_WithValidData_UpdatesAllProductProperties`: actualiza nombre, descripción, precio e imagen.
- [x] P0 — `UpdateProduct_WithValidData_SavesChanges`: guarda los cambios después de actualizar el producto.
- [x] P0 — `UpdateProduct_WhenProductDoesNotExist_ThrowsNotFoundException`: lanza `NotFoundException` cuando el producto no existe.
- [x] P1 — `UpdateProduct_WithInvalidData_ThrowsProductDomainException`: propaga la excepción de dominio cuando los datos no son válidos.
- [x] P1 — `UpdateProduct_WhenUpdateFails_DoesNotSaveChanges`: no guarda cambios cuando la actualización falla por una excepción de dominio.

### E. Gestión del stock

- [x] P0 — `IncreaseProductStock_WithExistingProduct_IncreasesStockAndSavesChanges`: aumenta el stock y guarda los cambios.
- [x] P0 — `DecreaseProductStock_WithExistingProduct_DecreasesStockAndSavesChanges`: reduce el stock y guarda los cambios.
- [x] P0 — `IncreaseProductStock_WhenProductDoesNotExist_ThrowsNotFoundException`: lanza `NotFoundException` al aumentar el stock de un producto inexistente.
- [x] P0 — `DecreaseProductStock_WhenProductDoesNotExist_ThrowsNotFoundException`: lanza `NotFoundException` al reducir el stock de un producto inexistente.
- [x] P1 — `IncreaseProductStock_WithInvalidQuantity_ThrowsProductDomainException`: lanza `ProductDomainException` si la cantidad para aumentar el stock es inválida.
- [x] P1 — `DecreaseProductStock_WithInvalidQuantity_ThrowsProductDomainException`: lanza `ProductDomainException` si la cantidad para reducir el stock es inválida.
- [x] P1 — `IncreaseProductStock_WhenOperationFails_DoesNotSaveChanges`: no guarda cambios si falla el aumento del stock.
- [x] P1 — `DecreaseProductStock_WhenOperationFails_DoesNotSaveChanges`: no guarda cambios si falla la reducción del stock.

### F. Disponibilidad de productos

- [x] P0 — `ActivateProduct_WithStockAndCategories_ActivatesProductAndSavesChanges`: activa un producto con stock y categorías, y guarda los cambios.
- [x] P0 — `ActivateProduct_WhenProductDoesNotExist_ThrowsNotFoundException`: lanza `NotFoundException` al activar un producto inexistente.
- [x] P0 — `DeactivateProduct_WithExistingProduct_DeactivatesProductAndSavesChanges`: desactiva un producto existente y guarda los cambios.
- [x] P0 — `DeactivateProduct_WhenProductDoesNotExist_ThrowsNotFoundException`: lanza `NotFoundException` al desactivar un producto inexistente.
- [x] P1 — `ActivateProduct_WithoutStock_ThrowsProductDomainException`: lanza `ProductDomainException` al activar un producto sin stock.
- [x] P1 — `ActivateProduct_WithoutCategories_ThrowsProductDomainException`: lanza `ProductDomainException` al activar un producto sin categorías.
- [x] P1 — `ActivateProduct_WhenActivationFails_DoesNotSaveChanges`: no guarda cambios cuando falla la activación por una excepción de dominio.

### G. Asignación y eliminación de categorías

- [x] P0 — `AssignProductCategory_WithExistingProductAndCategory_AssignsCategoryAndSavesChanges`: asigna una categoría existente a un producto existente y guarda los cambios.
- [x] P0 — `AssignProductCategory_WhenProductDoesNotExist_ThrowsNotFoundException`: lanza `NotFoundException` si el producto no existe al asignar una categoría y comprueba que no se guardan cambios.
- [x] P0 — `AssignProductCategory_WhenCategoryDoesNotExist_ThrowsNotFoundException`: lanza `NotFoundException` si la categoría no existe al asignarla y comprueba que no se guardan cambios.
- [x] P0 — `RemoveProductCategory_WithExistingProductAndCategory_RemovesCategoryAndSavesChanges`: elimina la asociación entre un producto y una categoría y guarda los cambios.
- [x] P0 — `RemoveProductCategory_WhenProductDoesNotExist_ThrowsException`: lanza `NotFoundException` si el producto no existe al eliminar una categoría.
- [x] P0 — `RemoveProductCategory_WhenCategoryDoesNotExist_ThrowsException`: lanza `NotFoundException` si la categoría no existe al eliminarla del producto.
- [x] P1 — `AssignProductCategory_WhenCategoryIsAlreadyAssigned_ThrowsProductDomainException`: lanza `ProductDomainException` si la categoría ya está asignada.
- [x] P1 — `RemoveProductCategory_WhenCategoryIsNotAssigned_ThrowsProductDomainException`: lanza `ProductDomainException` si la categoría no está asociada al producto.
- [x] P1 — `AssignProductCategory_WhenAssignmentFails_DoesNotSaveChanges`: no guarda cambios si falla la asignación por una regla de dominio.
- [x] P1 — `RemoveProductCategory_WhenRemovalFails_DoesNotSaveChanges`: no guarda cambios si falla la eliminación por una regla de dominio.

## 3. Pruebas unitarias pendientes para la capa de Application

### A. Gestión de categorías (`CategoryUseCasesTests.cs`)

- [ ] P0 — Pendiente: comprobar que se obtienen las categorías y se mapean correctamente a sus DTO.
- [ ] P0 — Pendiente: comprobar que se devuelve una lista vacía cuando no existen categorías.
- [ ] P0 — Pendiente: comprobar que se obtiene una categoría por identificador y se devuelve su DTO.
- [ ] P0 — Pendiente: comprobar el comportamiento cuando la categoría solicitada no existe.
- [ ] P0 — Pendiente: comprobar la creación de una categoría válida y que se guarda.
- [ ] P1 — Pendiente: comprobar que la creación propaga `CategoryDomainException` cuando los datos no son válidos y no guarda cambios.
- [ ] P0 — Pendiente: comprobar la actualización de una categoría existente y que se guarda.
- [ ] P0 — Pendiente: comprobar que la actualización lanza `NotFoundException` cuando la categoría no existe.
- [ ] P1 — Pendiente: comprobar que la actualización propaga `CategoryDomainException` cuando los datos no son válidos y no guarda cambios.
- [ ] P0 — Pendiente: comprobar la eliminación de una categoría existente y que se guarda.
- [ ] P0 — Pendiente: comprobar que la eliminación lanza `NotFoundException` cuando la categoría no existe.
