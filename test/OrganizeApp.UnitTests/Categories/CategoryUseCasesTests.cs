
using OrganizeApp.Application.Category.DTOs;
using OrganizeApp.Application.Category.Interfaces;
using OrganizeApp.Application.Exceptions;
using OrganizeApp.Domain.Exceptions;
using CategoryEntity = OrganizeApp.Domain.Entities.Category;
using CategoryUseCases = OrganizeApp.Application.Category.CategoryUseCases;

namespace OrganizeApp.UnitTests.Categories;

public class CategoryUseCasesTests
{
    #region Category Queries

    [Fact]
    public async Task GetCategories_WithCategories_ReturnsMappedSummaryDTOs()
    {
        // Arrange
        var firstCategory = CreateCategory(1, "Electrónica", "ELECTRONICS");
        var secondCategory = CreateCategory(2, "Hogar", "HOME");

        var categoryRepository = new FakeCategoryRepository
        {
            Categories = [firstCategory, secondCategory]
        };

        var useCases = CreateUseCases(categoryRepository);

        // Act
        var result = await useCases.GetCategories();

        // Assert
        Assert.Equal(2, result.Count);

        Assert.Collection(
            result,
            category =>
            {
                Assert.Equal(1, category.Id);
                Assert.Equal("Electrónica", category.Name);
                Assert.Equal("ELECTRONICS", category.Code);
            },
            category =>
            {
                Assert.Equal(2, category.Id);
                Assert.Equal("Hogar", category.Name);
                Assert.Equal("HOME", category.Code);
            });
    }

    [Fact]
    public async Task GetCategories_WhenNoCategoriesExist_ReturnsEmptyList()
    {
        // Arrange
        var categoryRepository = new FakeCategoryRepository();
        var useCases = CreateUseCases(categoryRepository);

        // Act
        var result = await useCases.GetCategories();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetCategoryById_WhenCategoryExists_ReturnsMappedCategoryDTO()
    {
        // Arrange
        var category = CreateCategory(
            5,
            "Electrónica",
            "ELECTRONICS",
            "Productos electrónicos",
            "electronics.jpg");

        var categoryRepository = new FakeCategoryRepository
        {
            CategoriesById = { [5] = category }
        };

        var useCases = CreateUseCases(categoryRepository);

        // Act
        var result = await useCases.GetCategoryById(5);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.Id);
        Assert.Equal("Electrónica", result.Name);
        Assert.Equal("ELECTRONICS", result.Code);
        Assert.Equal("Productos electrónicos", result.Description);
        Assert.Equal("electronics.jpg", result.ImageUrl);
    }

    [Fact]
    public async Task GetCategoryById_WhenCategoryDoesNotExist_ReturnsNull()
    {
        // Arrange
        var categoryRepository = new FakeCategoryRepository();
        var useCases = CreateUseCases(categoryRepository);

        // Act
        var result = await useCases.GetCategoryById(99);

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region Category Creation

    [Fact]
    public async Task CreateCategory_WithValidData_CreatesAndSendsCategoryToRepository()
    {
        // Arrange
        var dto = new CreateCategoryDTO
        {
            Name = "Electrónica",
            Code = "ELECTRONICS",
            Description = "Productos electrónicos",
            ImageUrl = "electronics.jpg"
        };

        var categoryRepository = new FakeCategoryRepository();
        var useCases = CreateUseCases(categoryRepository);

        // Act
        await useCases.CreateCategory(dto);

        // Assert
        var category = Assert.IsType<CategoryEntity>(
            categoryRepository.ReceivedCategory);

        Assert.Equal(dto.Name, category.Name);
        Assert.Equal(dto.Code, category.Code);
        Assert.Equal(dto.Description, category.Description);
        Assert.Equal(dto.ImageUrl, category.ImageUrl);
        Assert.Equal(1, categoryRepository.AddCallCount);
    }

    [Fact]
    public async Task CreateCategory_WithValidData_SavesChanges()
    {
        // Arrange
        var dto = new CreateCategoryDTO
        {
            Name = "Electrónica",
            Code = "ELECTRONICS",
            Description = "Productos electrónicos",
            ImageUrl = "electronics.jpg"
        };

        var categoryRepository = new FakeCategoryRepository();
        var useCases = CreateUseCases(categoryRepository);

        // Act
        await useCases.CreateCategory(dto);

        // Assert
        Assert.Equal(1, categoryRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task CreateCategory_WithInvalidData_ThrowsCategoryDomainException()
    {
        // Arrange
        var dto = new CreateCategoryDTO
        {
            Name = "Electrónica",
            Code = "electronics",
            Description = "Productos electrónicos",
            ImageUrl = "electronics.jpg"
        };

        var categoryRepository = new FakeCategoryRepository();
        var useCases = CreateUseCases(categoryRepository);

        // Act
        var exception = await Assert.ThrowsAsync<CategoryDomainException>(
            () => useCases.CreateCategory(dto));

        // Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task CreateCategory_WhenCreationFails_DoesNotSaveChanges()
    {
        // Arrange
        var dto = new CreateCategoryDTO
        {
            Name = "Electrónica",
            Code = "electronics",
            Description = "Productos electrónicos",
            ImageUrl = "electronics.jpg"
        };

        var categoryRepository = new FakeCategoryRepository();
        var useCases = CreateUseCases(categoryRepository);

        // Act
        await Assert.ThrowsAsync<CategoryDomainException>(
            () => useCases.CreateCategory(dto));

        // Assert
        Assert.Null(categoryRepository.ReceivedCategory);
        Assert.Equal(0, categoryRepository.AddCallCount);
        Assert.Equal(0, categoryRepository.SaveChangesCallCount);
    }

    #endregion

    #region Category Deletion

    [Fact]
    public async Task DeleteCategory_WhenCategoryExists_DeletesCategoryAndSavesChanges()
    {
        // Arrange
        var category = CreateCategory(1, "Electrónica", "ELECTRONICS");

        var categoryRepository = new FakeCategoryRepository
        {
            CategoriesById = { [1] = category }
        };

        var useCases = CreateUseCases(categoryRepository);

        // Act
        await useCases.DeleteCategory(1);

        // Assert
        Assert.Same(category, categoryRepository.DeletedCategory);
        Assert.Equal(1, categoryRepository.DeleteCallCount);
        Assert.Equal(1, categoryRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task DeleteCategory_WhenCategoryDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var categoryRepository = new FakeCategoryRepository();
        var useCases = CreateUseCases(categoryRepository);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => useCases.DeleteCategory(99));

        // Assert
        Assert.Equal("Category not found", exception.Message);
    }

    [Fact]
    public async Task DeleteCategory_WhenCategoryDoesNotExist_DoesNotSaveChanges()
    {
        // Arrange
        var categoryRepository = new FakeCategoryRepository();
        var useCases = CreateUseCases(categoryRepository);

        // Act
        await Assert.ThrowsAsync<NotFoundException>(
            () => useCases.DeleteCategory(99));

        // Assert
        Assert.Equal(0, categoryRepository.DeleteCallCount);
        Assert.Equal(0, categoryRepository.SaveChangesCallCount);
    }

    #endregion

    #region Category Updates

    [Fact]
    public async Task UpdateCategory_WithValidData_UpdatesCategoryProperties()
    {
        // Arrange
        var category = CreateCategory(
            1,
            "Electrónica",
            "ELECTRONICS",
            "Descripción anterior",
            "old.jpg");

        var dto = new UpdateCategoryDTO
        {
            Name = "Informática",
            Description = "Ordenadores y periféricos",
            ImageUrl = "computers.jpg"
        };

        var categoryRepository = new FakeCategoryRepository
        {
            CategoriesById = { [1] = category }
        };

        var useCases = CreateUseCases(categoryRepository);

        // Act
        await useCases.UpdateCategory(1, dto);

        // Assert
        Assert.Equal(dto.Name, category.Name);
        Assert.Equal(dto.Description, category.Description);
        Assert.Equal(dto.ImageUrl, category.ImageUrl);
    }

    [Fact]
    public async Task UpdateCategory_WithValidData_SavesChanges()
    {
        // Arrange
        var category = CreateCategory(1, "Electrónica", "ELECTRONICS");

        var dto = new UpdateCategoryDTO
        {
            Name = "Informática",
            Description = "Ordenadores y periféricos",
            ImageUrl = "computers.jpg"
        };

        var categoryRepository = new FakeCategoryRepository
        {
            CategoriesById = { [1] = category }
        };

        var useCases = CreateUseCases(categoryRepository);

        // Act
        await useCases.UpdateCategory(1, dto);

        // Assert
        Assert.Equal(1, categoryRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task UpdateCategory_WhenCategoryDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var dto = new UpdateCategoryDTO
        {
            Name = "Informática",
            Description = "Ordenadores y periféricos",
            ImageUrl = "computers.jpg"
        };

        var categoryRepository = new FakeCategoryRepository();
        var useCases = CreateUseCases(categoryRepository);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => useCases.UpdateCategory(99, dto));

        // Assert
        Assert.Equal("Category not found", exception.Message);
    }

    [Fact]
    public async Task UpdateCategory_WithInvalidData_ThrowsCategoryDomainException()
    {
        // Arrange
        var category = CreateCategory(1, "Electrónica", "ELECTRONICS");

        var dto = new UpdateCategoryDTO
        {
            Name = "",
            Description = "Nueva descripción",
            ImageUrl = "new.jpg"
        };

        var categoryRepository = new FakeCategoryRepository
        {
            CategoriesById = { [1] = category }
        };

        var useCases = CreateUseCases(categoryRepository);

        // Act
        var exception = await Assert.ThrowsAsync<CategoryDomainException>(
            () => useCases.UpdateCategory(1, dto));

        // Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task UpdateCategory_WhenUpdateFails_DoesNotSaveChanges()
    {
        // Arrange
        var category = CreateCategory(1, "Electrónica", "ELECTRONICS");

        var dto = new UpdateCategoryDTO
        {
            Name = "",
            Description = "Nueva descripción",
            ImageUrl = "new.jpg"
        };

        var categoryRepository = new FakeCategoryRepository
        {
            CategoriesById = { [1] = category }
        };

        var useCases = CreateUseCases(categoryRepository);

        // Act
        await Assert.ThrowsAsync<CategoryDomainException>(
            () => useCases.UpdateCategory(1, dto));

        // Assert
        Assert.Equal(0, categoryRepository.SaveChangesCallCount);
    }

    #endregion

    #region Category Name Management

    [Fact]
    public async Task ChangeCategoryName_WithValidName_UpdatesCategoryNameAndSavesChanges()
    {
        // Arrange
        var category = CreateCategory(1, "Electrónica", "ELECTRONICS");

        var categoryRepository = new FakeCategoryRepository
        {
            CategoriesById = { [1] = category }
        };

        var useCases = CreateUseCases(categoryRepository);

        // Act
        await useCases.ChangeCategoryName(1, "Informática");

        // Assert
        Assert.Equal("Informática", category.Name);
        Assert.Equal(1, categoryRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task ChangeCategoryName_WhenCategoryDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var categoryRepository = new FakeCategoryRepository();
        var useCases = CreateUseCases(categoryRepository);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => useCases.ChangeCategoryName(99, "Informática"));

        // Assert
        Assert.Equal("Category not found", exception.Message);
        Assert.Equal(0, categoryRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task ChangeCategoryName_WithInvalidName_ThrowsCategoryDomainException()
    {
        // Arrange
        var category = CreateCategory(1, "Electrónica", "ELECTRONICS");

        var categoryRepository = new FakeCategoryRepository
        {
            CategoriesById = { [1] = category }
        };

        var useCases = CreateUseCases(categoryRepository);

        // Act
        await Assert.ThrowsAsync<CategoryDomainException>(
            () => useCases.ChangeCategoryName(1, ""));

        // Assert
        Assert.Equal("Electrónica", category.Name);
        Assert.Equal(0, categoryRepository.SaveChangesCallCount);
    }

    #endregion

    #region Category Description Management

    [Fact]
    public async Task ChangeCategoryDescription_WithValidDescription_UpdatesCategoryDescriptionAndSavesChanges()
    {
        // Arrange
        var category = CreateCategory(1, "Electrónica", "ELECTRONICS");

        var categoryRepository = new FakeCategoryRepository
        {
            CategoriesById = { [1] = category }
        };

        var useCases = CreateUseCases(categoryRepository);

        // Act
        await useCases.ChangeCategoryDescription(1, "Productos electrónicos");

        // Assert
        Assert.Equal("Productos electrónicos", category.Description!);
        Assert.Equal(1, categoryRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task ChangeCategoryDescription_WhenCategoryDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var categoryRepository = new FakeCategoryRepository();
        var useCases = CreateUseCases(categoryRepository);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => useCases.ChangeCategoryDescription(99, "Descripción"));

        // Assert
        Assert.Equal("Category not found", exception.Message);
        Assert.Equal(0, categoryRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task ChangeCategoryDescription_WithInvalidDescription_ThrowsCategoryDomainException()
    {
        // Arrange
        var category = CreateCategory(1, "Electrónica", "ELECTRONICS");
        var originalDescription = category.Description;

        var categoryRepository = new FakeCategoryRepository
        {
            CategoriesById = { [1] = category }
        };

        var useCases = CreateUseCases(categoryRepository);
        var invalidDescription = new string('D', 501);

        // Act
        await Assert.ThrowsAsync<CategoryDomainException>(
            () => useCases.ChangeCategoryDescription(1, invalidDescription));

        // Assert
        Assert.Equal(originalDescription, category.Description!);
        Assert.Equal(0, categoryRepository.SaveChangesCallCount);
    }

    #endregion

    #region Category Code Management

    [Fact]
    public async Task ChangeCategoryCode_WithValidCode_UpdatesCategoryCodeAndSavesChanges()
    {
        // Arrange
        var category = CreateCategory(1, "Electrónica", "ELECTRONICS");

        var categoryRepository = new FakeCategoryRepository
        {
            CategoriesById = { [1] = category }
        };

        var useCases = CreateUseCases(categoryRepository);

        // Act
        await useCases.ChangeCategoryCode(1, "COMPUTERS");

        // Assert
        Assert.Equal("COMPUTERS", category.Code);
        Assert.Equal(1, categoryRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task ChangeCategoryCode_WhenCategoryDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var categoryRepository = new FakeCategoryRepository();
        var useCases = CreateUseCases(categoryRepository);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => useCases.ChangeCategoryCode(99, "COMPUTERS"));

        // Assert
        Assert.Equal("Category not found", exception.Message);
        Assert.Equal(0, categoryRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task ChangeCategoryCode_WithInvalidCode_ThrowsCategoryDomainException()
    {
        // Arrange
        var category = CreateCategory(1, "Electrónica", "ELECTRONICS");

        var categoryRepository = new FakeCategoryRepository
        {
            CategoriesById = { [1] = category }
        };

        var useCases = CreateUseCases(categoryRepository);

        // Act
        await Assert.ThrowsAsync<CategoryDomainException>(
            () => useCases.ChangeCategoryCode(1, "electronics"));

        // Assert
        Assert.Equal("ELECTRONICS", category.Code);
        Assert.Equal(0, categoryRepository.SaveChangesCallCount);
    }

    #endregion

    #region Category Image Management

    [Fact]
    public async Task ChangeCategoryImage_WithValidImageUrl_UpdatesCategoryImageAndSavesChanges()
    {
        // Arrange
        var category = CreateCategory(1, "Electrónica", "ELECTRONICS");

        var categoryRepository = new FakeCategoryRepository
        {
            CategoriesById = { [1] = category }
        };

        var useCases = CreateUseCases(categoryRepository);

        // Act
        await useCases.ChangeCategoryImage(1, "new-image.jpg");

        // Assert
        Assert.Equal("new-image.jpg", category.ImageUrl);
        Assert.Equal(1, categoryRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task ChangeCategoryImage_WhenCategoryDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var categoryRepository = new FakeCategoryRepository();
        var useCases = CreateUseCases(categoryRepository);

        // Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => useCases.ChangeCategoryImage(99, "new-image.jpg"));

        // Assert
        Assert.Equal("Category not found", exception.Message);
        Assert.Equal(0, categoryRepository.SaveChangesCallCount);
    }

    [Fact]
    public async Task ChangeCategoryImage_WithInvalidImageUrl_ThrowsCategoryDomainException()
    {
        // Arrange
        var category = CreateCategory(1, "Electrónica", "ELECTRONICS");
        var originalImageUrl = category.ImageUrl;

        var categoryRepository = new FakeCategoryRepository
        {
            CategoriesById = { [1] = category }
        };

        var useCases = CreateUseCases(categoryRepository);
        var invalidImageUrl = new string('i', 501);

        // Act
        await Assert.ThrowsAsync<CategoryDomainException>(
            () => useCases.ChangeCategoryImage(1, invalidImageUrl));

        // Assert
        Assert.Equal(originalImageUrl, category.ImageUrl);
        Assert.Equal(0, categoryRepository.SaveChangesCallCount);
    }

    #endregion

    #region Test Helpers

    private static CategoryUseCases CreateUseCases(
        FakeCategoryRepository categoryRepository)
    {
        return new CategoryUseCases(categoryRepository);
    }

    private static CategoryEntity CreateCategory(
        int id,
        string name,
        string code,
        string description = "Test description",
        string imageUrl = "category.jpg")
    {
        var category = new CategoryEntity(name, code, description, imageUrl);
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

    #region Fake Repository

    private sealed class FakeCategoryRepository : ICategoryRepository
    {
        public List<CategoryEntity> Categories { get; set; } = [];

        public Dictionary<int, CategoryEntity> CategoriesById { get; } = [];

        public CategoryEntity? ReceivedCategory { get; private set; }

        public CategoryEntity? DeletedCategory { get; private set; }

        public int AddCallCount { get; private set; }

        public int DeleteCallCount { get; private set; }

        public int SaveChangesCallCount { get; private set; }

        public Task<List<CategoryEntity>> GetCategoriesAsync()
        {
            return Task.FromResult(Categories);
        }

        public Task<CategoryEntity?> GetByIdAsync(int categoryId)
        {
            CategoriesById.TryGetValue(categoryId, out var category);
            return Task.FromResult(category);
        }

        public Task AddAsync(CategoryEntity category)
        {
            ReceivedCategory = category;
            AddCallCount++;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(CategoryEntity category)
        {
            DeletedCategory = category;
            DeleteCallCount++;
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync()
        {
            SaveChangesCallCount++;
            return Task.CompletedTask;
        }
    }

    #endregion
}
