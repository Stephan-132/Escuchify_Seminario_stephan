using escuchify_api.Data;
using escuchify_api.Core.Entities;
using escuchify_api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace escuchify_api.Services;

public class ArtistasService
{
    private readonly AppDbContext _context;

    public ArtistasService(AppDbContext context)
    {
        _context = context;
    }

    // Método para OBTENER todos los artistas
    public async Task<List<ArtistaDto>> ObtenerTodos()
    {
        var artistas = await _context.Artistas.ToListAsync();
        
        // Convertimos de Entidad (BD) a DTO (Público)
        return artistas.Select(a => new ArtistaDto {
            Id = a.Id,
            Nombre = a.Nombre,
            Genero = a.Genero,
            ImagenUrl = a.ImagenUrl
        }).ToList();
    }

    // Método para CREAR un artista nuevo
    public async Task<Artista> Crear(ArtistaDto dto)
    {
        var nuevoArtista = new Artista {
            Nombre = dto.Nombre,
            Genero = dto.Genero,
            ImagenUrl = dto.ImagenUrl,
            Biografia = "Sin biografía" // Valor por defecto
        };

        _context.Artistas.Add(nuevoArtista);
        await _context.SaveChangesAsync();
        return nuevoArtista;
    }
}