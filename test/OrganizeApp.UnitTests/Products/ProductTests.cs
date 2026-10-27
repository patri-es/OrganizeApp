
using OrganizeApp.Domain.Entities;
using OrganizeApp.Domain.Exceptions;

namespace OrganizeApp.UnitTests.Products;

public class ProductTests
{

    #region Test of Constructor (by create Product)
    // Crea un producto válido con sus datos correctos. Inicializa Stock a 0 e IsAvailable a false.
    // verifica que el constructor conserva los datos proporcionados y establece correctamente los valores iniciales
    [Fact] 
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

    // NAME
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

    // comprueba que se aceptan valores de máxima longitud
    [Fact]
    public void Constructor_WithNameAtMaximumLength_CreatesProduct()
    {
        // Arrange
        var name = new string('A', 100);
        // Act
        var product = new Product(name, null, 25m, null);
        // Assert
        Assert.Equal(100, product.Name.Length);
    }

    // comprueba que se rechazan valores superiores a la máxima longitud
    [Fact]
    public void Constructor_WithNameExceedingMaximumLength_ThrowsProductDomainException()
    {
        // Arrange
        var name = new string('A', 101);
        // Act & Assert
        Assert.Throws<ProductDomainException>(
            () => new Product(name, null, 25m, null));
    }

    // PRICE
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

    // Comprueba que permite valor de precio a 0
    [Fact]
    public void Constructor_WithZeroPrice_CreatesProduct()
    {
        // Act
        var product = new Product("Teclado", "Descripción de Teclado", 0m, "/images/teclado.jpg");

        // Assert
        Assert.Equal(0m, product.Price);
    }

    // DESCRIPTION
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Constructor_WithOptionalDescription_CreatesProduct(string? description)
    {
        // Act
        var product = new Product("Teclado", description, 25m, "/images/teclado.jpg");

        // Assert
        Assert.Equal(description, product.Description);
    }

    // 
    [Fact]
    public void Constructor_WithDescriptionAtMaximumLength_CreatesProduct()
    {
        // Arrange
        var description = new string('A', 500);

        // Act
        var product = new Product("Teclado", description, 25m, null);

        // Assert
        Assert.Equal(500, product.Description!.Length);
    }

    // 
    [Fact]
    public void Constructor_WithDescriptionExceedingMaximumLength_ThrowsProductDomainException()
    {
        // Arrange
        var description = new string('A', 501);

        // Act & Assert
        Assert.Throws<ProductDomainException>(
            () => new Product("Teclado", description, 25m, null));
    }

    // IMAGE URL
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Constructor_WithOptionalImageUrl_CreatesProduct(string? url)
    {
        // Act
        var product = new Product("Teclado", "Descripción de Teclado", 25m, url);

        // Assert
        Assert.Equal(url, product.ImageUrl);
    }

    // 
    [Fact]
    public void Constructor_WithImageUrlAtMaximumLength_CreatesProduct()
    {
        // Arrange
        var imageUrl = new string('A', 500);

        // Act
        var product = new Product("Teclado", "Descripción de Teclado", 25m, imageUrl);

        // Assert
        Assert.Equal(500, product.ImageUrl!.Length);
    }

    // 
    [Fact]
    public void Constructor_WithImageUrlExceedingMaximumLength_ThrowsProductDomainException()
    {
        // Arrange
        var imageUrl = new string('A', 501);

        // Act & Assert
        Assert.Throws<ProductDomainException>(
            () => new Product("Teclado", "Descripción de Teclado", 25m, imageUrl));
    }

    // CATEGORIES
    // Se declara con: public ICollection<Category> Categories { get; private set; } = [];
    [Fact]
    public void Constructor_WithValidData_InitializesCategoriesAsEmptyCollection()
    {
        // Arrange & Act
        var product = new Product("Teclado", null, 25m, null);

        // Assert
        Assert.NotNull(product.Categories); // comprueba que exista la categoría (no es necesaria esta comprobación en este caso)
        Assert.Empty(product.Categories); // comprueba que inicia sin ninguna categoría
    }


    #endregion

    #region Test of Edit Product

    // NAME
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

    [Fact]
    public void ChangeName_WithNameAtMaximumLength_UpdatesProductName()
    {
        // Arrange
        var product = new Product("Teclado", null, 25m, null);
        var newName = new string('A', 100);
        // Act
        product.ChangeName(newName);
        // Assert
        Assert.Equal(newName, product.Name);
    }
    [Fact]
    public void ChangeName_WithNameExceedingMaximumLength_ThrowsProductDomainException()
    {
        // Arrange
        var product = new Product("Teclado", null, 25m, null);
        var originalName = product.Name;
        var newName = new string('A', 101);

        // Act & Assert
        Assert.Throws<ProductDomainException>(
            () => product.ChangeName(newName));

        Assert.Equal(originalName, product.Name);
    }

    // DESCRIPTION
    [Theory]
    [InlineData("Test")]
    [InlineData("  ")]
    [InlineData(null)]
    public void ChangeDescription_WithOptionalDescription_UpdatesProductDescription(string? newDescription)
    {
        // Arrange
        var product = new Product(
            "Teclado",
            "Descripción de Teclado",
            25m,
            "/images/teclado.jpg");
        // Act
        product.ChangeDescription(newDescription);
        // Assert
        Assert.Equal(newDescription, product.Description);
    }

    [Fact]
    public void ChangeDescription_WithDescriptionAtMaximumLength_UpdatesProductDescription()
    {
        // Arrange
        var product = new Product("Teclado", null, 25m, null);
        var newDescription = new string('A', 500);
        // Act
        product.ChangeDescription(newDescription);
        // Assert
        Assert.Equal(newDescription, product.Description);
    }
    [Fact]
    public void ChangeDescription_WithDescriptionExceedingMaximumLength_ThrowsProductDomainException()
    {
        // Arrange
        var product = new Product("Teclado", null, 25m, null);
        var originalDescription = product.Description;
        var newDescription = new string('A', 501);

        // Act & Assert
        Assert.Throws<ProductDomainException>(
            () => product.ChangeDescription(newDescription));

        Assert.Equal(originalDescription, product.Description);
    }

    // IMAGE URL
    [Theory]
    [InlineData("Test")]
    [InlineData("  ")]
    [InlineData(null)]
    public void ChangeImageUrl_WithOptionalImageUrl_UpdatesProductImageUrl(string? newImageUrl)
    {
        // Arrange
        var product = new Product(
            "Teclado",
            "Descripción de Teclado",
            25m,
            "/images/teclado.jpg");
        // Act
        product.ChangeImage(newImageUrl);
        // Assert
        Assert.Equal(newImageUrl, product.ImageUrl);
    }

    [Fact]
    public void ChangeImageUrl_WithImageUrlAtMaximumLength_UpdatesProductImageUrl()
    {
        // Arrange
        var product = new Product("Teclado", null, 25m, null);
        var newImageUrl = new string('A', 500);
        // Act
        product.ChangeImage(newImageUrl);
        // Assert
        Assert.Equal(newImageUrl, product.ImageUrl);
    }

    [Fact]
    public void ChangeImageUrl_WithImageUrlExceedingMaximumLength_ThrowsProductDomainException()
    {
        // Arrange
        var product = new Product("Teclado", null, 25m, null);
        var originalImageUrl = product.ImageUrl;
        var newImageUrl = new string('A', 501);

        // Act & Assert
        Assert.Throws<ProductDomainException>(
            () => product.ChangeImage(newImageUrl));

        Assert.Equal(originalImageUrl, product.ImageUrl);
    }

    // PRICE
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

    [Theory]
    [InlineData(0)]
    [InlineData(0.05)]
    [InlineData(100)]
    public void ChangePrice_WithValidPrice_UpdatesProductPrice(decimal newPrice)
    {
        // Arrange
        var product = new Product(
            "Teclado",
            "Descripción de Teclado",
            25m,
            "/images/teclado.jpg");
        // Act
        product.ChangePrice(newPrice);
        // Assert
        Assert.Equal(newPrice, product.Price);
    }

    #endregion

    #region Manage Stock of Products
    
    // Aumentar stock
    [Fact]
    public void IncreaseStock_WithValidQuantity_IncreasesStock()
    {
        // Arrange
        var product = new Product("Teclado", "Descripción de Teclado", 25m, null);
        var quantity = 10;
        // Act
        product.IncreaseStock(quantity);
        // Assert
        Assert.Equal(quantity, product.Stock);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void IncreaseStock_WithInvalidQuantity_ThrowsProductDomainException(int quantity)
    {
        // Arrange
        var product = new Product("Teclado", "Descripción de Teclado", 25m, null);

        // Act
        void act() => product.IncreaseStock(quantity);

        // Assert
        Assert.Throws<ProductDomainException>(act);
    }

    // reducir stock
    [Fact]
    public void DecreaseStock_WithValidQuantity_DecreasesStock()
    {
        // Arrange
        var product = new Product("Teclado", "Descripción de Teclado", 25m, null);
        product.IncreaseStock(10);
        var quantity = 5;

        // Act
        product.DecreaseStock(quantity);

        // Assert
        Assert.Equal(5, product.Stock);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void DecreaseStock_WithInvalidQuantity_ThrowsProductDomainException(int quantity)
    {
        // Arrange
        var product = new Product("Teclado", "Descripción de Teclado", 25m, null);
        product.IncreaseStock(10);

        // Act
        void act() => product.DecreaseStock(quantity);

        // Assert
        Assert.Throws<ProductDomainException>(act);
    }


    #endregion

    #region Manage Availability of Products

    [Fact]
    public void Activate_WithStockAndCategories_SetsIsAvailableToTrue()
    {
        // Arrange
        var product = new Product("Teclado", "Descripción de Teclado", 25m, null);
        product.IncreaseStock(10);

        var category = new Category("Electrónica", "ELEC", "Descripción", "imagen.jpg");
        product.AssignCategory(category);

        // Act
        product.Activate();

        // Assert
        Assert.True(product.IsAvailable);
    }

    [Fact]
    public void Activate_WithoutStock_ThrowsProductDomainException()
    {
        // Arrange
        var product = new Product("Teclado", "Descripción de Teclado", 25m, null);

        var category = new Category("Electrónica", "ELEC", "Descripción", "imagen.jpg");
        product.AssignCategory(category);

        // Act
        void act() => product.Activate();

        // Assert
        Assert.Throws<ProductDomainException>(act);
        Assert.False(product.IsAvailable);
    }

    [Fact]
    public void Activate_WithoutCategories_ThrowsProductDomainException()
    {
        // Arrange
        var product = new Product("Teclado", "Descripción de Teclado", 25m, null);
        product.IncreaseStock(10);

        // Act
        void act() => product.Activate();

        // Assert
        Assert.Throws<ProductDomainException>(act);
        Assert.False(product.IsAvailable);
    }

    [Fact]
    public void Deactivate_WhenProductIsActive_SetsIsAvailableToFalse()
    {
        // Arrange
        var product = new Product("Teclado", "Descripción de Teclado", 25m, null);
        product.IncreaseStock(10);

        var category = new Category("nombre Category", "CAT", "Descripción de Cat", "imagen.jpg");
        product.AssignCategory(category);
        product.Activate();

        // Act
        product.Deactivate();

        // Assert
        Assert.False(product.IsAvailable);
    }

    #endregion
    #region Manage Categories of Products

    [Fact]
    public void AssignCategory_WithValidCategory_AddsCategoryToProduct()
    {
        // Arrange
        var product = new Product("Teclado", null, 25m, null);
        var category = CreateCategoryWithId(1);

        // Act
        product.AssignCategory(category);

        // Assert
        Assert.Single(product.Categories);
        Assert.Contains(category, product.Categories);
    }

    [Fact]
    public void AssignCategory_WithSameCategoryTwice_DoesNotAddDuplicate()
    {
        // Arrange
        var product = new Product("Teclado", null, 25m, null);
        var category = CreateCategoryWithId(1);

        // Act
        product.AssignCategory(category);
        product.AssignCategory(category);

        // Assert
        Assert.Single(product.Categories);
        Assert.Contains(category, product.Categories);
    }

    [Fact]
    public void AssignCategory_WithDifferentCategories_AddsBothCategories()
    {
        // Arrange
        var product = new Product("Teclado", null, 25m, null);
        var firstCategory = CreateCategoryWithId(1);
        var secondCategory = CreateCategoryWithId(2);

        // Act
        product.AssignCategory(firstCategory);
        product.AssignCategory(secondCategory);

        // Assert
        Assert.Equal(2, product.Categories.Count);
        Assert.Contains(firstCategory, product.Categories);
        Assert.Contains(secondCategory, product.Categories);
    }

    [Fact]
    public void RemoveCategory_WithExistingCategory_RemovesCategoryFromProduct()
    {
        // Arrange
        var product = new Product("Teclado", null, 25m, null);
        var category = CreateCategoryWithId(1);
        product.AssignCategory(category);

        // Act
        product.RemoveCategory(category);

        // Assert
        Assert.Empty(product.Categories);
    }

    [Fact]
    public void RemoveCategory_WithCategoryNotAssigned_DoesNotModifyCategories()
    {
        // Arrange
        var product = new Product("Teclado", null, 25m, null);
        var assignedCategory = CreateCategoryWithId(1);
        var otherCategory = CreateCategoryWithId(2);

        product.AssignCategory(assignedCategory);

        // Act
        product.RemoveCategory(otherCategory);

        // Assert
        Assert.Single(product.Categories);
        Assert.Contains(assignedCategory, product.Categories);
    }

    [Fact]
    public void RemoveCategory_WhenLastCategoryIsRemoved_SetsIsAvailableToFalse()
    {
        // Arrange
        var product = new Product("Teclado", null, 25m, null);
        product.IncreaseStock(10);

        var category = CreateCategoryWithId(1);
        product.AssignCategory(category);
        product.Activate();

        // Act
        product.RemoveCategory(category);

        // Assert
        Assert.Empty(product.Categories);
        Assert.False(product.IsAvailable);
    }

    [Fact]
    public void RemoveCategory_WhenOtherCategoriesRemain_KeepsProductAvailable()
    {
        // Arrange
        var product = new Product("Teclado", null, 25m, null);
        product.IncreaseStock(10);

        var firstCategory = CreateCategoryWithId(1);
        var secondCategory = CreateCategoryWithId(2);

        product.AssignCategory(firstCategory);
        product.AssignCategory(secondCategory);
        product.Activate();

        // Act
        product.RemoveCategory(firstCategory);

        // Assert
        Assert.Single(product.Categories);
        Assert.Contains(secondCategory, product.Categories);
        Assert.True(product.IsAvailable);
    }

    private static Category CreateCategoryWithId(int id)
    {
        var category = new Category(
            $"Categoría {id}",
            $"CAT{id}",
            "Descripción",
            "imagen.jpg");

        typeof(Category)
            .GetProperty(nameof(Category.Id))!
            .SetValue(category, id);

        return category;
    }

    #endregion

}
