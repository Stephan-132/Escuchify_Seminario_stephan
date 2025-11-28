using Microsoft.AspNetCore.Mvc;
using escuchify_api.Data;
using escuchify_api.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace escuchify_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DiscosController : ControllerBase
{
    private readonly AppDbContext _context;

    public DiscosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Discos>>> GetDiscos()
    {
        return await _context.Discos.Include(d => d.Canciones).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Discos>> GetDisco(int id)
    {
        var disco = await _context.Discos
            .Include(d => d.Canciones)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (disco == null) return NotFound();
        return disco;
    }

    [HttpPost]
    public async Task<ActionResult<Discos>> PostDisco(Discos disco)
    {
        _context.Discos.Add(disco);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetDisco), new { id = disco.Id }, disco);
    }
    
    // Agrega el PUT y DELETE si quieres, siguiendo el ejemplo anterior
}