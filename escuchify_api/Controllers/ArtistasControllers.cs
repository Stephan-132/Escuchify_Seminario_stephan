using Microsoft.AspNetCore.Mvc;
using escuchify_api.Services;
using escuchify_api.DTOs;

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
        var creado = await _service.Crear(dto);
        return Ok(creado);
    }
}