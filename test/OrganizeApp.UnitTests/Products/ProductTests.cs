
using OrganizeApp.Domain.Entities;
using OrganizeApp.Domain.Exceptions;

namespace OrganizeApp.UnitTests.Products;

public class ProductTests
{

    #region Test of Constructor (by create Product)
    // test para el constructor
    // verifica que el constructor conserva los datos proporcionados y establece correctamente los valores iniciales
    // Crea un producto válido con sus datos correctos. Inicializa Stock a 0 e IsAvailable a false.
    [Fact] // ejecuta la prueba una vez con unos datos concretos
    public void Constructor_WithValidData_CreatesProductWithInitialValues()
    {
        // Arrange (arreglar/acordar)
        var name = "Teclado";
        var description = "Descripción de Teclado";
        var price = 25m;
        var imageUrl = "/images/teclado.jpg";

        // Act (acción)
        var product = new Product(name, description, price, imageUrl);

        // Assert (asegurar, afirmar. comprueba exclusivamente los valores iniciales tras crear el producto)
        Assert.Equal(name, product.Name);
        Assert.Equal(description, product.Description);
        Assert.Equal(price, product.Price);
        Assert.Equal(imageUrl, product.ImageUrl);
        Assert.Equal(0, product.Stock);
        Assert.False(product.IsAvailable);
    }

    // Rechaza un nombre null, vacío o compuesto solo por espacios (3 casos con InlineData)
    [Theory] // ejecuta la misma prueba para diferentes conjuntos de datos indicados en los InlineData
    [InlineData(null)] // InlineData (cambia el valor recibido en el parámetro, en este caso "name")
    [InlineData("")]
    [InlineData("  ")]
    public void Constructor_WithInvalidName_ThrowsProductDomainException(string? name)
    {
        // Act & Assert
        Assert.Throws<ProductDomainException>(
            () => new Product(name!, "Descripción de Teclado", 25m, "/images/teclado.jpg"));
    }

    // Rechaza un precio negativo
    [Theory]
    [InlineData(-1)]
    [InlineData(-25)]
    [InlineData(-0.01)]
    public void Constructor_WithNegativePrice_ThrowsProductDomainException(decimal price)
    {
        // Act & Assert
        Assert.Throws<ProductDomainException>(
            () => new Product("Teclado", "Descripción de Teclado", price, "/images/teclado.jpg"));
    }

    [Fact]
    public void Constructor_WithZeroPrice_CreatesProduct()
    {
        // Act
        var product = new Product("Teclado", "Descripción de Teclado", 0m, "/images/teclado.jpg");

        // Assert
        Assert.Equal(0m, product.Price);
    }


    [Fact]
    public void Constructor_WithNullDescription_CreatesProduct()
    {
        // Arrange
        var name = "Teclado";
        string? description = null;
        var price = 25m;
        var imageUrl = "/images/teclado.jpg";

        // Act
        var product = new Product(name, description, price, imageUrl);

        // Assert
        Assert.Null(product.Description);
    }

    [Fact]
    public void Constructor_WithNullImageUrl_CreatesProduct()
    {
        // Arrange
        var name = "Teclado";
        var description = "Descripción de Teclado";
        var price = 25m;
        string? imageUrl = null;

        // Act
        var product = new Product(name, description, price, imageUrl);

        // Assert
        Assert.Null(product.ImageUrl);
    }

    #endregion

    #region Test of Edit Product

    [Theory]
    [InlineData("Test")]
    [InlineData("Test with spaces")]
    [InlineData("Test@with_symbols")]
    [InlineData("15897")]
    public void ChangeName_WithValidName_UpdatesProductName(string newName)
    {
        // Arrange
        var product = new Product(
            "Teclado",
            "Descripción de Teclado",
            25m,
            "/images/teclado.jpg");

        // Act
        product.ChangeName(newName);

        // Assert
        Assert.Equal(newName, product.Name);
    }


    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void ChangeName_WithInvalidName_ThrowsProductDomainException(string? name)
    {
        // Arrange (creo producto válido para la prueba)
        var originalName = "Teclado";

        var product = new Product(
            originalName,
            "Descripción de Teclado",
            25m,
            "/images/teclado.jpg");

        // Act & Assert (pasamos parámetro name con los valores InlineData, debe ir a la excepción de Dominio)
        Assert.Throws<ProductDomainException>(
            () => product.ChangeName(name!));

        // Assert
        Assert.Equal(originalName, product.Name);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-0.05)]
    [InlineData(-10)]
    public void ChangePrice_WithNegativeValue_ThrowsProductDomainException(decimal price)
    {
        // Arrange
        var originalPrice = 10m;

        var product = new Product(
            "Teclado",
            "Descripción de Teclado",
            originalPrice,
            "/images/teclado.jpg");

        // Act & Assert 
        Assert.Throws<ProductDomainException>(
            () => product.ChangePrice(price));

        Assert.Equal(originalPrice, product.Price);
    }

    #endregion

}
