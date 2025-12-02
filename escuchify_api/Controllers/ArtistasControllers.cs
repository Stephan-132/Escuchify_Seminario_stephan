using Microsoft.AspNetCore.Mvc;
using escuchify_api.Services;
using escuchify_api.DTOs;
using escuchify_api.Data;

namespace escuchify_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ArtistasController : ControllerBase
{
    private readonly ArtistasService _service;


    // Inyectamos el servicio
    public ArtistasController(ArtistasService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<ArtistaDto>>> Get()
    {
        return await _service.ObtenerTodos();
    }

    [HttpPost]
    public async Task<IActionResult> Post(ArtistaDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var creado = await _service.Crear(dto);
        return Ok(creado);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ArtistaDto>> Get(int id)
    {
        var artista = await _service.ObtenerPorId(id);
        if (artista == null) return NotFound();
        return artista;
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var exito = await _service.Borrar(id);
        if (!exito) return NotFound();
        return NoContent(); // 204 significa "Borrado con éxito"
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, ArtistaDto dto)
    {
        // Si el DTO viene sin ID, se lo ponemos nosotros para que no falle la comparación
        if (dto.Id == 0) dto.Id = id;

        if (id != dto.Id) return BadRequest("El ID no coincide");

        var exito = await _service.Actualizar(id, dto);
        if (!exito) return NotFound();

        return NoContent();
    }

    [HttpPost("{id}/actualizar-biografia")]
    public async Task<IActionResult> ActualizarBiografia(int id)
    {
        var biografia = await _service.ActualizarBiografiaDesdeWikipedia(id);
        if (biografia == null) return NotFound();
        return Ok(new { biografia });
    }

[HttpGet("resumen")]
    public async Task<ActionResult<ResumenDto>> GetResumen()
    {
        // CORRECCIÓN: Llamamos al método del servicio, no intentamos contar aquí
        var resumen = await _service.ObtenerResumen();
        
        return Ok(resumen);
    }
}