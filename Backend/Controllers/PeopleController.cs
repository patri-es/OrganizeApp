using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PeopleController : ControllerBase
{
    #region dbContext
    // agrego el contexto de la bbdd
    private readonly AppDbContext _context;

    // constructor que inyectará la AppDbContext:
    public PeopleController(AppDbContext context)
    {
        _context = context;
    }

    #endregion

    #region EndPoints

    // POST /api/people
    [HttpPost]
    public async Task<IActionResult> AddPerson(Person person)
    {
        try
        {
            _context.People.Add(person);
            await _context.SaveChangesAsync();
            return Ok(person); // 200 Ok status code + person object in the body

            //return CreatedAtRoute("GetPerson", new { id = person.Id }, person);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // GET /api/people
    [HttpGet]
    public async Task<IActionResult> GetPeople()
    {
        try
        {
            var people = await _context.People.ToListAsync();
            return Ok(people); // 200 Ok status code + person object in the body
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // GET /api/people/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPerson(int id)
    {
        try
        {
            if (id == 0) return BadRequest("id no puede ser 0");

            var person = await _context.People.FindAsync(id);
            if (person is null)
                return NotFound(); // 404 Not Found status code 

            return Ok(person); // 200 Ok status code + person object in the body
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // PUT /api/people/1
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePerson(int id, [FromBody] Person person)
    {
        try
        {
            if (id != person.Id)
            {
                return BadRequest("Id en la url y en el Body no coinciden");
            }
            if (!await _context.People.AnyAsync(p => p.Id == id))
            {
                return NotFound();
            }
            _context.People.Update(person);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // Delete /api/people/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerson(int id)
    {
        try
        {
            if (id == 0) return BadRequest("El id no puede ser 0");

            var person = await _context.People.FindAsync(id);
            if (person is null)
                return NotFound("Error, persona no encontrada"); // 400

            _context.People.Remove(person);
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