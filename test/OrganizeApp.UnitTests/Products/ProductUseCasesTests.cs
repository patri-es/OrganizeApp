using OrganizeApp.Application.Category.Interfaces;
using OrganizeApp.Application.Product.DTOs;
using OrganizeApp.Application.Product.Interfaces;
using OrganizeApp.Domain.Entities;
using ProductEntity = OrganizeApp.Domain.Entities.Product;
using ProductUseCases = OrganizeApp.Application.Product.ProductUseCases;
using OrganizeApp.Application.Exceptions;
using OrganizeApp.Domain.Exceptions;

namespace OrganizeApp.UnitTests.Products;

public class ProductUseCasesTests
{
    #region Product Queries

    [Fact]
    public async Task GetProducts_WithProducts_ReturnsMappedProductDTOs()
    {
        // Arrange
        var product = CreateProduct(1, "Keyboard", "Mechanical keyboard", 59.99m, "keyboard.jpg");

        var productRepository = new FakeProductRepository
        {
            Products = [product]
        };

        var useCases = CreateUseCases(productRepository);

        // Act
        var result = await useCases.GetProducts(null);

        // Assert
        var productDTO = Assert.Single(result);
        Assert.Equal(1, productDTO.Id);
        Assert.Equal("Keyboard", productDTO.Name);
        Assert.Equal("Mechanical keyboard", productDTO.Description);
        Assert.Equal(59.99m, productDTO.Price);
        Assert.Equal("keyboard.jpg", productDTO.ImageUrl);
        Assert.Equal(product.Stock, productDTO.Stock);
        Assert.Equal(product.IsAvailable, productDTO.IsAvailable);
    }

    [Fact]
    public async Task GetProducts_WithAssociatedCategories_ReturnsMappedCategories()
    {
        // Arrange
        var product = CreateProduct(1, "Keyboard", "Mechanical keyboard", 59.99m, "keyboard.jpg");
        var category = CreateCategory(10, "Peripherals", "PERIPHERALS");

        product.AssignCategory(category);

        var productRepository = new FakeProductRepository
        {
            Products = [product]
        };

        var useCases = CreateUseCases(productRepository);

        // Act
        var result = await useCases.GetProducts(null);

        // Assert
        var productDTO = Assert.Single(result);
        var categoryDTO = Assert.Single(productDTO.Categories);

        Assert.Equal(10, categoryDTO.Id);
        Assert.Equal("PERIPHERALS", categoryDTO.Code);
        Assert.Equal("Peripherals", categoryDTO.Name);
    }

    [Fact]
    public async Task GetProducts_WhenNoProductsExist_ReturnsEmptyList()
    {
        // Arrange
        var productRepository = new FakeProductRepository();
        var useCases = CreateUseCases(productRepository);

        // Act
        var result = await useCases.GetProducts(null);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetProducts_WithFilters_PassesFiltersToRepository()
    {
        // Arrange
        var filters = new FiltersProductDTO();
        var productRepository = new FakeProductRepository();
        var useCases = CreateUseCases(productRepository);

        // Act
        await useCases.GetProducts(filters);

        // Assert
        Assert.Same(filters, productRepository.ReceivedFilters);
    }

    [Fact]
    public async Task GetProductById_WhenProductExists_ReturnsMappedProductDTO()
    {
        // Arrange
        var product = CreateProduct(5, "Mouse", "Wireless mouse", 29.99m, "mouse.jpg");

        var productRepository = new FakeProductRepository
        {
            ProductsById = { [5] = product }
        };

        var useCases = CreateUseCases(productRepository);

        // Act
        var result = await useCases.GetProductById(5);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.Id);
        Assert.Equal("Mouse", result.Name);
        Assert.Equal("Wireless mouse", result.Description);
        Assert.Equal(29.99m, result.Price);
        Assert.Equal("mouse.jpg", result.ImageUrl);
        Assert.Equal(product.Stock, result.Stock);
        Assert.Equal(product.IsAvailable, result.IsAvailable);
    }

    [Fact]
    public async Task GetProductById_WhenProductDoesNotExist_ReturnsNull()
    {
        // Arrange
        var productRepository = new FakeProductRepository();
        var useCases = CreateUseCases(productRepository);

        // Act
        var result = await useCases.GetProductById(99);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetProducts_WithProductWithoutCategories_ReturnsEmptyCategoryList()
    {
        // Arrange
        var product = CreateProduct(1, "Keyboard", "Mechanical keyboard", 59.99m, "keyboard.jpg");

        var productRepository = new FakeProductRepository
        {
            Products = [product]
        };

        var useCases = CreateUseCases(productRepository);

        // Act
        var result = await useCases.GetProducts(null);

        // Assert
        var productDTO = Assert.Single(result);
        Assert.Empty(productDTO.Categories);
    }

    #endregion

    #region Product Creation

    [Fact]
    public async Task CreateProduct_WithValidData_CreatesAndSendsProductToRepository()
    {
        // Arrange
        var dto = new CreateProductDTO
        {
            Name = "Keyboard",
            Description = "Mechanical keyboard",
            Price = 59.99m,
            ImageUrl = "keyboard.jpg"
        };

        var productRepository = new FakeProductRepository();
        var useCases = CreateUseCases(productRepository);

        // Act
        await useCases.CreateProduct(dto);

        // Assert
        var product = Assert.IsType<ProductEntity>(productRepository.ReceivedProduct);

        Assert.Equal(dto.Name, product.Name);
        Assert.Equal(dto.Description, product.Description);
        Assert.Equal(dto.Price, product.Price);
        Assert.Equal(dto.ImageUrl, product.ImageUrl);
    }

    [Fact]
    public async Task CreateProduct_WithValidData_SavesChanges()
    {
        // Arrange
        var dto = new CreateProductDTO
        {
            Name = "Keyboard",
            Description = "Mechanical keyboard",
            Price = 59.99m,
            ImageUrl = "keyboard.jpg"
        };

        var productRepository = new FakeProductRepository();
        var useCases = CreateUseCases(productRepository);

        // Act
        await useCases.CreateProduct(dto);

        // Assert
        Assert.Equal(1, productRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task CreateProduct_WithInvalidData_ThrowsProductDomainException()
    {
        // Arrange
        var dto = new CreateProductDTO
        {
            Name = "",
            Description = "Mechanical keyboard",
            Price = 59.99m,
            ImageUrl = "keyboard.jpg"
        };

        var productRepository = new FakeProductRepository();
        var useCases = CreateUseCases(productRepository);

        // Act
        var exception = await Assert.ThrowsAsync<ProductDomainException>(
            () => useCases.CreateProduct(dto));

        // Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task CreateProduct_WhenCreationFails_DoesNotSaveChanges()
    {
        // Arrange
        var dto = new CreateProductDTO
        {
            Name = "",
            Description = "Mechanical keyboard",
            Price = 59.99m,
            ImageUrl = "keyboard.jpg"
        };

        var productRepository = new FakeProductRepository();
        var useCases = CreateUseCases(productRepository);

        // Act
        await Assert.ThrowsAsync<ProductDomainException>(
            () => useCases.CreateProduct(dto));

        // Assert
        Assert.Null(productRepository.ReceivedProduct);
        Assert.Equal(0, productRepository.SaveChangesCallCount);
    }

    #endregion

    #region Product Deletion

    [Fact]
    public async Task DeleteProduct_WhenProductExists_DeletesProductAndSavesChanges()
    {
        // Arrange
        var product = CreateProduct(1, "Keyboard", "Mechanical keyboard", 59.99m, "keyboard.jpg");

        var productRepository = new FakeProductRepository
        {
            ProductsById = { [1] = product }
        };

        var useCases = CreateUseCases(productRepository);

        // Act
        await useCases.DeleteProduct(1);

        // Assert
        Assert.Same(product, productRepository.DeletedProduct);
        Assert.Equal(1, productRepository.DeleteCallCount);
        Assert.Equal(1, productRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task DeleteProduct_WhenProductDoesNotExist_ThrowsException()
    {
        // Arrange
        var productRepository = new FakeProductRepository();
        var useCases = CreateUseCases(productRepository);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => useCases.DeleteProduct(99));

        // Assert
        Assert.Equal("Product not found", exception.Message);
    }

    [Fact]
    public async Task DeleteProduct_WhenProductDoesNotExist_DoesNotSaveChanges()
    {
        // Arrange
        var productRepository = new FakeProductRepository();
        var useCases = CreateUseCases(productRepository);

        // Act
        await Assert.ThrowsAsync<NotFoundException>(
            () => useCases.DeleteProduct(99));

        // Assert
        Assert.Equal(0, productRepository.DeleteCallCount);
        Assert.Equal(0, productRepository.SaveChangesCallCount);
    }

    #endregion

    #region Product Updates

    [Fact]
    public async Task UpdateProduct_WithValidData_UpdatesAllProductProperties()
    {
        // Arrange
        var product = CreateProduct(1, "Keyboard", "Old description", 59.99m, "old.jpg");

        var dto = new UpdateProductDTO
        {
            Name = "Gaming Keyboard",
            Description = "Updated description",
            Price = 79.99m,
            ImageUrl = "gaming-keyboard.jpg"
        };

        var productRepository = new FakeProductRepository
        {
            ProductsById = { [1] = product }
        };

        var useCases = CreateUseCases(productRepository);

        // Act
        await useCases.UpdateProduct(1, dto);

        // Assert
        Assert.Equal(dto.Name, product.Name);
        Assert.Equal(dto.Description, product.Description);
        Assert.Equal(dto.Price, product.Price);
        Assert.Equal(dto.ImageUrl, product.ImageUrl);
    }

    [Fact]
    public async Task UpdateProduct_WithValidData_SavesChanges()
    {
        // Arrange
        var product = CreateProduct(1, "Keyboard", "Old description", 59.99m, "old.jpg");

        var dto = new UpdateProductDTO
        {
            Name = "Gaming Keyboard",
            Description = "Updated description",
            Price = 79.99m,
            ImageUrl = "gaming-keyboard.jpg"
        };

        var productRepository = new FakeProductRepository
        {
            ProductsById = { [1] = product }
        };

        var useCases = CreateUseCases(productRepository);

        // Act
        await useCases.UpdateProduct(1, dto);

        // Assert
        Assert.Equal(1, productRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task UpdateProduct_WhenProductDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var dto = new UpdateProductDTO
        {
            Name = "Gaming Keyboard",
            Description = "Updated description",
            Price = 79.99m,
            ImageUrl = "gaming-keyboard.jpg"
        };

        var productRepository = new FakeProductRepository();
        var useCases = CreateUseCases(productRepository);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => useCases.UpdateProduct(99, dto));

        // Assert
        Assert.Equal("Product not found", exception.Message);
    }

    [Fact]
    public async Task UpdateProduct_WithInvalidData_ThrowsProductDomainException()
    {
        // Arrange
        var product = CreateProduct(1, "Keyboard", "Old description", 59.99m, "old.jpg");

        var dto = new UpdateProductDTO
        {
            Name = "Gaming Keyboard",
            Description = "Updated description",
            Price = -1m,
            ImageUrl = "gaming-keyboard.jpg"
        };

        var productRepository = new FakeProductRepository
        {
            ProductsById = { [1] = product }
        };

        var useCases = CreateUseCases(productRepository);

        // Act
        var exception = await Assert.ThrowsAsync<ProductDomainException>(
            () => useCases.UpdateProduct(1, dto));

        // Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task UpdateProduct_WhenUpdateFails_DoesNotSaveChanges()
    {
        // Arrange
        var product = CreateProduct(1, "Keyboard", "Old description", 59.99m, "old.jpg");

        var dto = new UpdateProductDTO
        {
            Name = "Gaming Keyboard",
            Description = "Updated description",
            Price = -1m,
            ImageUrl = "gaming-keyboard.jpg"
        };

        var productRepository = new FakeProductRepository
        {
            ProductsById = { [1] = product }
        };

        var useCases = CreateUseCases(productRepository);

        // Act
        await Assert.ThrowsAsync<ProductDomainException>(
            () => useCases.UpdateProduct(1, dto));

        // Assert
        Assert.Equal(0, productRepository.SaveChangesCallCount);
    }

    #endregion

    #region Stock Management

    [Fact]
    public async Task IncreaseProductStock_WithExistingProduct_IncreasesStockAndSavesChanges()
    {
        // Arrange
        var product = CreateProduct(1, "Keyboard", "Mechanical keyboard", 59.99m, "keyboard.jpg");

        var productRepository = new FakeProductRepository
        {
            ProductsById = { [1] = product }
        };

        var useCases = CreateUseCases(productRepository);

        // Act
        await useCases.IncreaseProductStock(1, 5);

        // Assert
        Assert.Equal(5, product.Stock);
        Assert.Equal(1, productRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task DecreaseProductStock_WithExistingProduct_DecreasesStockAndSavesChanges()
    {
        // Arrange
        var product = CreateProduct(1, "Keyboard", "Mechanical keyboard", 59.99m, "keyboard.jpg");
        product.IncreaseStock(10);

        var productRepository = new FakeProductRepository
        {
            ProductsById = { [1] = product }
        };

        var useCases = CreateUseCases(productRepository);

        // Act
        await useCases.DecreaseProductStock(1, 3);

        // Assert
        Assert.Equal(7, product.Stock);
        Assert.Equal(1, productRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task IncreaseProductStock_WhenProductDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var productRepository = new FakeProductRepository();
        var useCases = CreateUseCases(productRepository);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => useCases.IncreaseProductStock(99, 5));

        // Assert
        Assert.Equal("Product not found", exception.Message);
    }

    [Fact]
    public async Task DecreaseProductStock_WhenProductDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var productRepository = new FakeProductRepository();
        var useCases = CreateUseCases(productRepository);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => useCases.DecreaseProductStock(99, 3));

        // Assert
        Assert.Equal("Product not found", exception.Message);
    }

    [Fact]
    public async Task IncreaseProductStock_WithInvalidQuantity_ThrowsProductDomainException()
    {
        // Arrange
        var product = CreateProduct(1, "Keyboard", "Mechanical keyboard", 59.99m, "keyboard.jpg");

        var productRepository = new FakeProductRepository
        {
            ProductsById = { [1] = product }
        };

        var useCases = CreateUseCases(productRepository);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ProductDomainException>(
            () => useCases.IncreaseProductStock(1, -1));
    }

    [Fact]
    public async Task DecreaseProductStock_WithInvalidQuantity_ThrowsProductDomainException()
    {
        // Arrange
        var product = CreateProduct(1, "Keyboard", "Mechanical keyboard", 59.99m, "keyboard.jpg");
        product.IncreaseStock(5);

        var productRepository = new FakeProductRepository
        {
            ProductsById = { [1] = product }
        };

        var useCases = CreateUseCases(productRepository);

        // Act
        var exception = await Assert.ThrowsAsync<ProductDomainException>(
            () => useCases.DecreaseProductStock(1, 6));

        // Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task IncreaseProductStock_WhenOperationFails_DoesNotSaveChanges()
    {
        // Arrange
        var product = CreateProduct(1, "Keyboard", "Mechanical keyboard", 59.99m, "keyboard.jpg");

        var productRepository = new FakeProductRepository
        {
            ProductsById = { [1] = product }
        };

        var useCases = CreateUseCases(productRepository);

        // Act
        await Assert.ThrowsAsync<ProductDomainException>(
            () => useCases.IncreaseProductStock(1, -1));

        // Assert
        Assert.Equal(0, productRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task DecreaseProductStock_WhenOperationFails_DoesNotSaveChanges()
    {
        // Arrange
        var product = CreateProduct(1, "Keyboard", "Mechanical keyboard", 59.99m, "keyboard.jpg");
        product.IncreaseStock(5);

        var productRepository = new FakeProductRepository
        {
            ProductsById = { [1] = product }
        };

        var useCases = CreateUseCases(productRepository);

        // Act
        await Assert.ThrowsAsync<ProductDomainException>(
            () => useCases.DecreaseProductStock(1, 6));

        // Assert
        Assert.Equal(0, productRepository.SaveChangesCallCount);
    }

    #endregion

    #region Product Availability

    [Fact]
    public async Task ActivateProduct_WithStockAndCategories_ActivatesProductAndSavesChanges()
    {
        // Arrange
        var product = CreateProduct(1, "Keyboard", "Mechanical keyboard", 59.99m, "keyboard.jpg");
        product.IncreaseStock(5);

        var category = CreateCategory(10, "Peripherals", "PERIPHERALS");
        product.AssignCategory(category);

        var productRepository = new FakeProductRepository
        {
            ProductsById = { [1] = product }
        };

        var useCases = CreateUseCases(productRepository);

        // Act
        await useCases.ActivateProduct(1);

        // Assert
        Assert.True(product.IsAvailable);
        Assert.Equal(1, productRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task ActivateProduct_WhenProductDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var productRepository = new FakeProductRepository();
        var useCases = CreateUseCases(productRepository);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => useCases.ActivateProduct(99));

        // Assert
        Assert.Equal("Product not found", exception.Message);
    }

    [Fact]
    public async Task DeactivateProduct_WithExistingProduct_DeactivatesProductAndSavesChanges()
    {
        // Arrange
        var product = CreateProduct(1, "Keyboard", "Mechanical keyboard", 59.99m, "keyboard.jpg");
        product.IncreaseStock(5);
        product.AssignCategory(CreateCategory(10, "Peripherals", "PERIPHERALS"));
        product.Activate();

        var productRepository = new FakeProductRepository
        {
            ProductsById = { [1] = product }
        };

        var useCases = CreateUseCases(productRepository);

        // Act
        await useCases.DeactivateProduct(1);

        // Assert
        Assert.False(product.IsAvailable);
        Assert.Equal(1, productRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task DeactivateProduct_WhenProductDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var productRepository = new FakeProductRepository();
        var useCases = CreateUseCases(productRepository);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => useCases.DeactivateProduct(99));

        // Assert
        Assert.Equal("Product not found", exception.Message);
    }

    [Fact]
    public async Task ActivateProduct_WithoutStock_ThrowsProductDomainException()
    {
        // Arrange
        var product = CreateProduct(1, "Keyboard", "Mechanical keyboard", 59.99m, "keyboard.jpg");
        product.AssignCategory(CreateCategory(10, "Peripherals", "PERIPHERALS"));

        var productRepository = new FakeProductRepository
        {
            ProductsById = { [1] = product }
        };

        var useCases = CreateUseCases(productRepository);

        // Act
        var exception = await Assert.ThrowsAsync<ProductDomainException>(
            () => useCases.ActivateProduct(1));

        // Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task ActivateProduct_WithoutCategories_ThrowsProductDomainException()
    {
        // Arrange
        var product = CreateProduct(1, "Keyboard", "Mechanical keyboard", 59.99m, "keyboard.jpg");
        product.IncreaseStock(5);

        var productRepository = new FakeProductRepository
        {
            ProductsById = { [1] = product }
        };

        var useCases = CreateUseCases(productRepository);

        // Act
        var exception = await Assert.ThrowsAsync<ProductDomainException>(
            () => useCases.ActivateProduct(1));

        // Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task ActivateProduct_WhenActivationFails_DoesNotSaveChanges()
    {
        // Arrange
        var product = CreateProduct(1, "Keyboard", "Mechanical keyboard", 59.99m, "keyboard.jpg");

        var productRepository = new FakeProductRepository
        {
            ProductsById = { [1] = product }
        };

        var useCases = CreateUseCases(productRepository);

        // Act
        await Assert.ThrowsAsync<ProductDomainException>(
            () => useCases.ActivateProduct(1));

        // Assert
        Assert.False(product.IsAvailable);
        Assert.Equal(0, productRepository.SaveChangesCallCount);
    }

    #endregion

    #region Product Category Management

    [Fact]
    public async Task AssignProductCategory_WithExistingProductAndCategory_AssignsCategoryAndSavesChanges()
    {
        // Arrange
        var product = CreateProduct(1, "Keyboard", "Mechanical keyboard", 59.99m, "keyboard.jpg");
        var category = CreateCategory(10, "Peripherals", "PERIPHERALS");

        var productRepository = new FakeProductRepository
        {
            ProductsById = { [1] = product }
        };

        var categoryRepository = new FakeCategoryRepository
        {
            CategoriesById = { [10] = category }
        };

        var useCases = new ProductUseCases(productRepository, categoryRepository);

        // Act
        await useCases.AssignProductCategory(1, 10);

        // Assert
        var assignedCategory = Assert.Single(product.Categories);
        Assert.Equal(10, assignedCategory.Id);
        Assert.Equal(1, productRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task AssignProductCategory_WhenProductDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var category = CreateCategory(10, "Peripherals", "PERIPHERALS");

        var productRepository = new FakeProductRepository();
        var categoryRepository = new FakeCategoryRepository
        {
            CategoriesById = { [10] = category }
        };

        var useCases = new ProductUseCases(productRepository, categoryRepository);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => useCases.AssignProductCategory(99, 10));

        // Assert
        Assert.Equal("Product not found", exception.Message);
        Assert.Equal(0, productRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task AssignProductCategory_WhenCategoryDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var product = CreateProduct(1, "Keyboard", "Mechanical keyboard", 59.99m, "keyboard.jpg");

        var productRepository = new FakeProductRepository
        {
            ProductsById = { [1] = product }
        };

        var useCases = CreateUseCases(productRepository);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => useCases.AssignProductCategory(1, 10));

        // Assert
        Assert.Equal("Category not found", exception.Message);
        Assert.Equal(0, productRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task RemoveProductCategory_WithExistingProductAndCategory_RemovesCategoryAndSavesChanges()
    {
        // Arrange
        var product = CreateProduct(1, "Keyboard", "Mechanical keyboard", 59.99m, "keyboard.jpg");
        var category = CreateCategory(10, "Peripherals", "PERIPHERALS");
        product.AssignCategory(category);

        var productRepository = new FakeProductRepository
        {
            ProductsById = { [1] = product }
        };

        var categoryRepository = new FakeCategoryRepository
        {
            CategoriesById = { [10] = category }
        };

        var useCases = new ProductUseCases(productRepository, categoryRepository);

        // Act
        await useCases.RemoveProductCategory(1, 10);

        // Assert
        Assert.Empty(product.Categories);
        Assert.Equal(1, productRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task RemoveProductCategory_WhenProductDoesNotExist_ThrowsException()
    {
        // Arrange
        var category = CreateCategory(10, "Peripherals", "PERIPHERALS");

        var productRepository = new FakeProductRepository();
        var categoryRepository = new FakeCategoryRepository
        {
            CategoriesById = { [10] = category }
        };

        var useCases = new ProductUseCases(productRepository, categoryRepository);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => useCases.RemoveProductCategory(99, 10));

        // Assert
        Assert.Equal("Product not found", exception.Message);
    }

    [Fact]
    public async Task RemoveProductCategory_WhenCategoryDoesNotExist_ThrowsException()
    {
        // Arrange
        var product = CreateProduct(1, "Keyboard", "Mechanical keyboard", 59.99m, "keyboard.jpg");

        var productRepository = new FakeProductRepository
        {
            ProductsById = { [1] = product }
        };

        var useCases = CreateUseCases(productRepository);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => useCases.RemoveProductCategory(1, 10));

        // Assert
        Assert.Equal("Category not found", exception.Message);
    }

    [Fact]
    public async Task AssignProductCategory_WhenCategoryIsAlreadyAssigned_ThrowsProductDomainException()
    {
        // Arrange
        var product = CreateProduct(1, "Keyboard", "Mechanical keyboard", 59.99m, "keyboard.jpg");
        var category = CreateCategory(10, "Peripherals", "PERIPHERALS");
        product.AssignCategory(category);

        var productRepository = new FakeProductRepository
        {
            ProductsById = { [1] = product }
        };

        var categoryRepository = new FakeCategoryRepository
        {
            CategoriesById = { [10] = category }
        };

        var useCases = new ProductUseCases(productRepository, categoryRepository);

        // Act
        var exception = await Assert.ThrowsAsync<ProductDomainException>(
            () => useCases.AssignProductCategory(1, 10));

        // Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task RemoveProductCategory_WhenCategoryIsNotAssigned_ThrowsProductDomainException()
    {
        // Arrange
        var product = CreateProduct(1, "Keyboard", "Mechanical keyboard", 59.99m, "keyboard.jpg");
        var category = CreateCategory(10, "Peripherals", "PERIPHERALS");

        var productRepository = new FakeProductRepository
        {
            ProductsById = { [1] = product }
        };

        var categoryRepository = new FakeCategoryRepository
        {
            CategoriesById = { [10] = category }
        };

        var useCases = new ProductUseCases(productRepository, categoryRepository);

        // Act
        var exception = await Assert.ThrowsAsync<ProductDomainException>(
            () => useCases.RemoveProductCategory(1, 10));

        // Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task AssignProductCategory_WhenAssignmentFails_DoesNotSaveChanges()
    {
        // Arrange
        var product = CreateProduct(1, "Keyboard", "Mechanical keyboard", 59.99m, "keyboard.jpg");
        var category = CreateCategory(10, "Peripherals", "PERIPHERALS");
        product.AssignCategory(category);

        var productRepository = new FakeProductRepository
        {
            ProductsById = { [1] = product }
        };

        var categoryRepository = new FakeCategoryRepository
        {
            CategoriesById = { [10] = category }
        };

        var useCases = new ProductUseCases(productRepository, categoryRepository);

        // Act
        await Assert.ThrowsAsync<ProductDomainException>(
            () => useCases.AssignProductCategory(1, 10));

        // Assert
        Assert.Equal(0, productRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task RemoveProductCategory_WhenRemovalFails_DoesNotSaveChanges()
    {
        // Arrange
        var product = CreateProduct(1, "Keyboard", "Mechanical keyboard", 59.99m, "keyboard.jpg");
        var category = CreateCategory(10, "Peripherals", "PERIPHERALS");

        var productRepository = new FakeProductRepository
        {
            ProductsById = { [1] = product }
        };

        var categoryRepository = new FakeCategoryRepository
        {
            CategoriesById = { [10] = category }
        };

        var useCases = new ProductUseCases(productRepository, categoryRepository);

        // Act
        await Assert.ThrowsAsync<ProductDomainException>(
            () => useCases.RemoveProductCategory(1, 10));

        // Assert
        Assert.Equal(0, productRepository.SaveChangesCallCount);
    }

    #endregion

    #region Test helpers

    private static ProductUseCases CreateUseCases(
        FakeProductRepository productRepository)
    {
        return new ProductUseCases(
            productRepository,
            new FakeCategoryRepository());
    }

    private static ProductEntity CreateProduct(
        int id,
        string name,
        string description,
        decimal price,
        string imageUrl)
    {
        var product = new ProductEntity(name, description, price, imageUrl);
        SetEntityId(product, id);
        return product;
    }

    private static Category CreateCategory(
        int id,
        string name,
        string code)
    {
        var category = new Category(name, code, "Test description", "category.jpg");
        SetEntityId(category, id);
        return category;
    }

    private static void SetEntityId(object entity, int id)
    {
        entity.GetType()
            .GetProperty("Id")!
            .SetValue(entity, id);
    }

    #endregion

    #region Fake repositories
    private sealed class FakeProductRepository : IProductRepository
    {
        public List<ProductEntity> Products { get; set; } = [];

        public Dictionary<int, ProductEntity> ProductsById { get; } = [];

        public FiltersProductDTO? ReceivedFilters { get; private set; }

        public ProductEntity? ReceivedProduct { get; private set; }

        public ProductEntity? DeletedProduct { get; private set; }

        public int DeleteCallCount { get; private set; }

        public int SaveChangesCallCount { get; private set; }

        public Task<List<ProductEntity>> GetProductsAsync(FiltersProductDTO? filters)
        {
            ReceivedFilters = filters;
            return Task.FromResult(Products);
        }

        public Task<ProductEntity?> GetByIdAsync(int productId)
        {
            ProductsById.TryGetValue(productId, out var product);
            return Task.FromResult(product);
        }

        public Task<ProductEntity?> GetByIdWithCategoriesAsync(
            int id,
            CancellationToken cancellationToken)
        {
            return GetByIdAsync(id);
        }

        public Task AddAsync(ProductEntity product)
        {
            ReceivedProduct = product;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(ProductEntity product)
        {
            DeletedProduct = product;
            DeleteCallCount++;
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync()
        {
            SaveChangesCallCount++;
            return Task.CompletedTask;
        }
    }

    private sealed class FakeCategoryRepository : ICategoryRepository
    {
        public Dictionary<int, Category> CategoriesById { get; } = [];

        public Task<List<Category>> GetCategoriesAsync()
        {
            return Task.FromResult(CategoriesById.Values.ToList());
        }

        public Task<Category?> GetByIdAsync(int categoryId)
        {
            CategoriesById.TryGetValue(categoryId, out var category);
            return Task.FromResult(category);
        }

        public Task AddAsync(Category category) => Task.CompletedTask;

        public Task DeleteAsync(Category category) => Task.CompletedTask;

        public Task SaveChangesAsync() => Task.CompletedTask;
    }

    #endregion

}