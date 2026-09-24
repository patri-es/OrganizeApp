using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    #region dbContext
    // agrego el contexto de la bbdd
    private readonly AppDbContext _context;

    // constructor que inyectará la AppDbContext:
    public CategoriesController(AppDbContext context)
    {
        _context = context;
    }

    #endregion

    #region EndPoints

    // POST /api/categorys
    [HttpPost]
    public async Task<IActionResult> AddCategory(Category category)
    {
        try
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return Ok(category);

        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // GET /api/categorys
    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        try
        {
            var categorys = await _context.Categories.ToListAsync();
            return Ok(categorys); // 200 Ok status code + category object in the body
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // GET /api/categorys/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(int id)
    {
        try
        {
            if (id == 0) return BadRequest("id no puede ser 0");

            var category = await _context.Categories.FindAsync(id);
            if (category is null)
                return NotFound(); // 404 Not Found status code 

            return Ok(category); // 200 Ok status code + category object in the body
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // PUT /api/categorys/1
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category category)
    {
        try
        {
            if (id != category.Id)
            {
                return BadRequest("Id en la url y en el Body no coinciden");
            }
            if (!await _context.Categories.AnyAsync(p => p.Id == id))
            {
                return NotFound();
            }
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // Delete /api/categorys/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        try
        {
            if (id == 0) return BadRequest("El id no puede ser 0");

            var category = await _context.Categories.FindAsync(id);
            if (category is null)
                return NotFound("Error, categoría no encontrada"); // 400

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    #endregion

}
