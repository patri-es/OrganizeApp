using Microsoft.AspNetCore.Mvc;
using OrganizeApp.Application.Category;
using OrganizeApp.Application.Category.Interfaces;
using OrganizeApp.Application.Category.DTOs;

namespace OrganizeApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Categories")]
public class CategoriesController(ICategoryUseCases categoryUseCases) : ControllerBase
{
    #region uses cases
    private readonly ICategoryUseCases _categoryUseCases = categoryUseCases;

    #endregion

    #region EndPoints

    /// <summary>
    /// Get all categories. Return 200 with the list
    /// </summary>
    // GET /api/categories
    [HttpGet]
    public async Task<ActionResult> GetCategories()
    {
        var categories = await _categoryUseCases.GetCategories();
        return Ok(categories);
    }

    /// <summary>
    /// Obtiene una categoría por id. Devuelve 200 con la categoría o 404 si no existe.
    /// </summary>
    // GET /api/categories/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(int id)
    {
        var category = await _categoryUseCases.GetCategoryById(id);
        if (category is null)
            return NotFound(); 

        return Ok(category);
    }


    /// <summary>
    /// Create a new categoy. Return 200 with the category created.
    /// </summary>
    // POST /api/categories
    [HttpPost]
    public async Task<IActionResult> AddCategory(CreateCategoryDTO category)
    {
        await _categoryUseCases.CreateCategory(category);
        return Ok(category);
    }
    
    /// <summary>
    /// Actualiza una categoría existente. Devuelve 204 si la actualización tuvo éxito.
    /// </summary>
    // PUT /api/categories/1
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDTO category)
    {
        await _categoryUseCases.UpdateCategory(id, category);
        return Ok(category);
    }

    /// <summary>
    /// Elimina una categoría por su id. Devuelve 204 si se eliminó correctamente.
    /// </summary>
    // Delete /api/categories/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        await _categoryUseCases.DeleteCategory(id);
        return Ok("Category successfully deleted.");
    }

    #endregion

}
