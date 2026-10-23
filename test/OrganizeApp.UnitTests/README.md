# Test unitarios

## Pruebas unitarias de Dominio
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

### Posible lista de próximos test
Leyenda: P0 = esenciales · P1 = importantes · P2 = complementarios

A. Constructor y valores iniciales
- [x] P0 — Crea un producto válido con sus datos correctos.
- [x] P0 — Inicializa Stock a 0 e IsAvailable a false.
- [x] P0 — Rechaza un nombre null, vacío o compuesto solo por espacios.
- [x] P0 — Rechaza una URL de imagen null, vacía o compuesta solo por espacios.
- [x] P0 — Rechaza un precio negativo.
- [x] P1 — Permite una descripción null.
- [x] P1 — Permite un precio igual a 0.
B. Cambios de nombre, descripción, precio e imagen
- [ ] P0 — ChangeName actualiza un nombre válido.
- [ ] P0 — ChangeName rechaza un nombre vacío o en blanco.
- [ ] P0 — ChangePrice actualiza un precio válido.
- [ ] P0 — ChangePrice rechaza un precio negativo.
- [ ] P1 — ChangeDescription permite establecer o cambiar una descripción, incluida null.
- [ ] P1 — ChangeImage actualiza la imagen.
- [ ] P1 — Comprobar si ChangeImage debe aceptar null o una cadena vacía según las reglas que quieras para el dominio actual.
C. Gestión del stock
- [ ] P0 — IncreaseStock aumenta el stock con una cantidad válida.
- [ ] P0 — IncreaseStock rechaza cantidades iguales o inferiores a 0.
- [ ] P0 — DecreaseStock reduce el stock correctamente.
- [ ] P0 — DecreaseStock rechaza cantidades iguales o inferiores a 0.
- [ ] P0 — DecreaseStock rechaza una cantidad superior al stock disponible.
- [ ] P0 — Reducir el stock exactamente hasta 0 es válido.
- [ ] P0 — Reducir el stock hasta 0 establece IsAvailable a false.
- [ ] P1 — Aumentar el stock no cambia automáticamente la disponibilidad, salvo que esa sea una regla explícita del dominio.
D. Disponibilidad
- [ ] P0 — Activate activa un producto con stock y al menos una categoría.
- [ ] P0 — Activate rechaza un producto sin stock.
- [ ] P0 — Activate rechaza un producto sin categorías.
- [ ] P0 — Deactivate establece IsAvailable a false.
- [ ] P1 — Eliminar la última categoría de un producto disponible lo deja no disponible.
E. Asignación y eliminación de categorías
- [ ] P0 — AssignCategory añade una categoría.
- [ ] P0 — AssignCategory no duplica una categoría ya asignada.
- [ ] P0 — RemoveCategory elimina una categoría existente.
- [ ] P0 — RemoveCategory no produce errores si la categoría no está asignada.
- [ ] P0 — Eliminar la última categoría establece IsAvailable a false.
- [ ] P1 — Eliminar una categoría no desactiva el producto si todavía conserva otras.
F. Casos límite adicionales
- [ ] P1 — Verificar el comportamiento cuando el stock está cerca del límite de un int.
- [ ] P1 — Verificar qué ocurre si se pasa una categoría null a AssignCategory o RemoveCategory, si esos argumentos deben estar protegidos.
- [ ] P2 — Verificar operaciones repetidas, como desactivar dos veces o quitar dos veces la misma categoría.