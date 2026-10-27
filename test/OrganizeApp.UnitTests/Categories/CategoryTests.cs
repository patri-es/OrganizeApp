using OrganizeApp.Domain.Entities;
using OrganizeApp.Domain.Exceptions;

namespace OrganizeApp.UnitTests.Categories;

public class CategoryTests
{
    #region Constructor

    [Fact]
    public void Constructor_WithValidData_CreatesCategoryWithInitialValues()
    {
        // Arrange & Act
        var category = new Category(
            "Electrónica",
            "ELEC",
            "Productos electrónicos",
            "/images/electronica.jpg");

        // Assert
        Assert.Equal("Electrónica", category.Name);
        Assert.Equal("ELEC", category.Code);
        Assert.Equal("Productos electrónicos", category.Description);
        Assert.Equal("/images/electronica.jpg", category.ImageUrl);
        Assert.Empty(category.Products);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidName_ThrowsCategoryDomainException(string? name)
    {
        // Arrange, Act & Assert
        Assert.Throws<CategoryDomainException>(() =>
            new Category(name!, "ELEC", "Descripción", "imagen.jpg"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidCode_ThrowsCategoryDomainException(string? code)
    {
        // Arrange, Act & Assert
        Assert.Throws<CategoryDomainException>(() =>
            new Category("Electrónica", code!, "Descripción", "imagen.jpg"));
    }

    [Fact]
    public void Constructor_WithCodeAtMaximumLength_CreatesCategory()
    {
        // Arrange
        var code = new string('A', 20);

        // Act
        var category = new Category("Electrónica", code, "Descripción", "imagen.jpg");

        // Assert
        Assert.Equal(code, category.Code);
    }

    [Fact]
    public void Constructor_WithCodeExceedingMaximumLength_ThrowsCategoryDomainException()
    {
        // Arrange
        var code = new string('A', 21);

        // Act & Assert
        Assert.Throws<CategoryDomainException>(() =>
            new Category("Electrónica", code, "Descripción", "imagen.jpg"));
    }

    [Fact]
    public void Constructor_WithNameAtMaximumLength_CreatesCategory()
    {
        // Arrange
        var name = new string('A', 100);

        // Act
        var category = new Category(name, "ELEC", "Descripción", "imagen.jpg");

        // Assert
        Assert.Equal(name, category.Name);
    }

    [Fact]
    public void Constructor_WithNameExceedingMaximumLength_ThrowsCategoryDomainException()
    {
        // Arrange
        var name = new string('A', 101);

        // Act & Assert
        Assert.Throws<CategoryDomainException>(() =>
            new Category(name, "ELEC", "Descripción", "imagen.jpg"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithOptionalDescription_CreatesCategory(string? description)
    {
        // Arrange & Act
        var category = new Category("Electrónica", "ELEC", description!, "imagen.jpg");

        // Assert
        Assert.Equal(description, category.Description);
    }

    [Fact]
    public void Constructor_WithDescriptionAtMaximumLength_CreatesCategory()
    {
        // Arrange
        var description = new string('A', 500);

        // Act
        var category = new Category("Electrónica", "ELEC", description, "imagen.jpg");

        // Assert
        Assert.Equal(description, category.Description);
    }

    [Fact]
    public void Constructor_WithDescriptionExceedingMaximumLength_ThrowsCategoryDomainException()
    {
        // Arrange
        var description = new string('A', 501);

        // Act & Assert
        Assert.Throws<CategoryDomainException>(() =>
            new Category("Electrónica", "ELEC", description, "imagen.jpg"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithOptionalImageUrl_CreatesCategory(string? imageUrl)
    {
        // Arrange & Act
        var category = new Category("Electrónica", "ELEC", "Descripción", imageUrl!);

        // Assert
        Assert.Equal(imageUrl, category.ImageUrl);
    }

    [Fact]
    public void Constructor_WithImageUrlAtMaximumLength_CreatesCategory()
    {
        // Arrange
        var imageUrl = new string('A', 500);

        // Act
        var category = new Category("Electrónica", "ELEC", "Descripción", imageUrl);

        // Assert
        Assert.Equal(imageUrl, category.ImageUrl);
    }

    [Fact]
    public void Constructor_WithImageUrlExceedingMaximumLength_ThrowsCategoryDomainException()
    {
        // Arrange
        var imageUrl = new string('A', 501);

        // Act & Assert
        Assert.Throws<CategoryDomainException>(() =>
            new Category("Electrónica", "ELEC", "Descripción", imageUrl));
    }

    #endregion

    #region Modification

    [Theory]
    [InlineData("Informática")]
    [InlineData("  Informática  ")]
    public void ChangeName_WithValidName_UpdatesCategoryName(string name)
    {
        // Arrange
        var category = new Category("Electrónica", "ELEC", "Descripción", "imagen.jpg");

        // Act
        category.ChangeName(name);

        // Assert
        Assert.Equal(name, category.Name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ChangeName_WithInvalidName_ThrowsCategoryDomainException(string? name)
    {
        // Arrange
        var category = new Category("Electrónica", "ELEC", "Descripción", "imagen.jpg");

        // Act
        var act = () => category.ChangeName(name!);

        // Assert
        Assert.Throws<CategoryDomainException>(act);
        Assert.Equal("Electrónica", category.Name);
    }

    [Fact]
    public void ChangeName_WithNameAtMaximumLength_UpdatesCategoryName()
    {
        // Arrange
        var category = new Category("Electrónica", "ELEC", "Descripción", "imagen.jpg");
        var name = new string('A', 100);

        // Act
        category.ChangeName(name);

        // Assert
        Assert.Equal(name, category.Name);
    }

    [Fact]
    public void ChangeName_WithNameExceedingMaximumLength_ThrowsCategoryDomainException()
    {
        // Arrange
        var category = new Category("Electrónica", "ELEC", "Descripción", "imagen.jpg");
        var name = new string('A', 101);

        // Act
        var act = () => category.ChangeName(name);

        // Assert
        Assert.Throws<CategoryDomainException>(act);
        Assert.Equal("Electrónica", category.Name);
    }

    [Fact]
    public void ChangeCode_WithValidCode_UpdatesCategoryCode()
    {
        // Arrange
        var category = new Category("Electrónica", "ELEC", "Descripción", "imagen.jpg");

        // Act
        category.ChangeCode("INFOR");

        // Assert
        Assert.Equal("INFOR", category.Code);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ChangeCode_WithInvalidCode_ThrowsCategoryDomainException(string? code)
    {
        // Arrange
        var category = new Category("Electrónica", "ELEC", "Descripción", "imagen.jpg");

        // Act
        var act = () => category.ChangeCode(code!);

        // Assert
        Assert.Throws<CategoryDomainException>(act);
        Assert.Equal("ELEC", category.Code);
    }

    [Fact]
    public void ChangeCode_WithCodeAtMaximumLength_UpdatesCategoryCode()
    {
        // Arrange
        var category = new Category("Electrónica", "ELEC", "Descripción", "imagen.jpg");
        var code = new string('A', 20);

        // Act
        category.ChangeCode(code);

        // Assert
        Assert.Equal(code, category.Code);
    }

    [Fact]
    public void ChangeCode_WithCodeExceedingMaximumLength_ThrowsCategoryDomainException()
    {
        // Arrange
        var category = new Category("Electrónica", "ELEC", "Descripción", "imagen.jpg");
        var code = new string('A', 21);

        // Act
        var act = () => category.ChangeCode(code);

        // Assert
        Assert.Throws<CategoryDomainException>(act);
        Assert.Equal("ELEC", category.Code);
    }

    [Theory]
    [InlineData("Nueva descripción")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void ChangeDescription_WithOptionalDescription_UpdatesCategoryDescription(string? description)
    {
        // Arrange
        var category = new Category("Electrónica", "ELEC", "Descripción", "imagen.jpg");

        // Act
        category.ChangeDescription(description!);

        // Assert
        Assert.Equal(description, category.Description);
    }

    [Fact]
    public void ChangeDescription_WithDescriptionAtMaximumLength_UpdatesCategoryDescription()
    {
        // Arrange
        var category = new Category("Electrónica", "ELEC", "Descripción", "imagen.jpg");
        var description = new string('A', 500);

        // Act
        category.ChangeDescription(description);

        // Assert
        Assert.Equal(description, category.Description);
    }

    [Fact]
    public void ChangeDescription_WithDescriptionExceedingMaximumLength_ThrowsCategoryDomainException()
    {
        // Arrange
        var category = new Category("Electrónica", "ELEC", "Descripción", "imagen.jpg");
        var description = new string('A', 501);

        // Act
        var act = () => category.ChangeDescription(description);

        // Assert
        Assert.Throws<CategoryDomainException>(act);
        Assert.Equal("Descripción", category.Description);
    }

    [Theory]
    [InlineData("nueva-imagen.jpg")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void ChangeImageUrl_WithOptionalImageUrl_UpdatesCategoryImageUrl(string? imageUrl)
    {
        // Arrange
        var category = new Category("Electrónica", "ELEC", "Descripción", "imagen.jpg");

        // Act
        category.ChangeImage(imageUrl!);

        // Assert
        Assert.Equal(imageUrl, category.ImageUrl);
    }

    [Fact]
    public void ChangeImageUrl_WithImageUrlAtMaximumLength_UpdatesCategoryImageUrl()
    {
        // Arrange
        var category = new Category("Electrónica", "ELEC", "Descripción", "imagen.jpg");
        var imageUrl = new string('A', 500);

        // Act
        category.ChangeImage(imageUrl);

        // Assert
        Assert.Equal(imageUrl, category.ImageUrl);
    }

    [Fact]
    public void ChangeImageUrl_WithImageUrlExceedingMaximumLength_ThrowsCategoryDomainException()
    {
        // Arrange
        var category = new Category("Electrónica", "ELEC", "Descripción", "imagen.jpg");
        var imageUrl = new string('A', 501);

        // Act
        var act = () => category.ChangeImage(imageUrl);

        // Assert
        Assert.Throws<CategoryDomainException>(act);
        Assert.Equal("imagen.jpg", category.ImageUrl);
    }

    #endregion


}
