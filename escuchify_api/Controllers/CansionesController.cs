using Microsoft.AspNetCore.Mvc;
using escuchify_api.Data;
using escuchify_api.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace escuchify_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CancionesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CancionesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Canciones>>> GetCanciones()
    {
        return await _context.Canciones.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Canciones>> GetCancion(int id)
    {
        var cancion = await _context.Canciones.FindAsync(id);
        if (cancion == null) return NotFound();
        return cancion;
    }

    [HttpPost]
    public async Task<ActionResult<Canciones>> PostCancion(Canciones cancion)
    {
        _context.Canciones.Add(cancion);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCancion), new { id = cancion.Id }, cancion);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCancion(int id)
    {
        var cancion = await _context.Canciones.FindAsync(id);
        if (cancion == null) return NotFound();

        _context.Canciones.Remove(cancion);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}