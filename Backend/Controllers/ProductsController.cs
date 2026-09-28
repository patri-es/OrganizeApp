using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Products")]
public class ProductsController : ControllerBase
{
    #region dbContext
    // agrego el contexto de la bbdd
    private readonly AppDbContext _context;

    // constructor que inyectará la AppDbContext:
    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

#endregion

#region EndPoints

    // POST /api/products
    [HttpPost]
    public async Task<IActionResult> AddProduct(Product product)
    {
        try
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return Ok(product);

    }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // GET /api/products
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        try
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products); // 200 Ok status code + product object in the body
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // GET /api/products/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        try
        {
            if (id == 0) return BadRequest("id no puede ser 0");

        var product = await _context.Products.FindAsync(id);
            if (product is null)
                return NotFound(); // 404 Not Found status code 

        return Ok(product); // 200 Ok status code + product object in the body
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // PUT /api/products/1
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
    {
        try
        {
            if (id != product.Id)
            {
                return BadRequest("Id en la url y en el Body no coinciden");
            }
            if (!await _context.Products.AnyAsync(p => p.Id == id))
            {
                return NotFound();
            }
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // Delete /api/products/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            if (id == 0) return BadRequest("El id no puede ser 0");

        var product = await _context.Products.FindAsync(id);
            if (product is null)
                return NotFound("Error, producto no encontrado"); // 400

        _context.Products.Remove(product);
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
