using Xunit;
using Microsoft.EntityFrameworkCore;
using escuchify_api.Data;
using escuchify_api.Services;
using escuchify_api.DTOs;
using escuchify_api.Core.Entities;

namespace Escuchify.Test;

public class ArtistasServiceTests
{
    // Este método crea una Base de Datos "de mentira" en la memoria RAM
    // para que cada test arranque limpio y no toque tu archivo escuchify.db real.
    private AppDbContext GetDatabaseContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            
        var databaseContext = new AppDbContext(options);
        databaseContext.Database.EnsureCreated();
        return databaseContext;
    }

    [Fact]
    public async Task Crear_DebeGuardarArtistaCorrectamente()
    {
        // 1. ARRANGE (Preparar)
        var context = GetDatabaseContext();
        var servicio = new ArtistasService(context);
        
        var nuevoArtista = new ArtistaDto 
        { 
            Nombre = "Shakira", 
            Genero = "Pop",
            ImagenUrl = "shakira.jpg"
        };

        // 2. ACT (Actuar)
        var resultado = await servicio.Crear(nuevoArtista);

        // 3. ASSERT (Verificar)
        Assert.NotNull(resultado);                 // ¿Devolvió algo?
        Assert.Equal("Shakira", resultado.Nombre); // ¿Se guardó el nombre bien?
        Assert.NotEqual(0, resultado.Id);          // ¿La BD le dio un ID?
    }

    [Fact]
    public async Task ObtenerTodos_DebeRetornarLista()
    {
        // 1. ARRANGE
        var context = GetDatabaseContext();
        // Inyectamos datos falsos
        context.Artistas.Add(new Artista { Nombre = "Bad Bunny", Genero = "Urbano" });
        context.Artistas.Add(new Artista { Nombre = "Adele", Genero = "Pop" });
        await context.SaveChangesAsync();

        var servicio = new ArtistasService(context);

        // 2. ACT
        var lista = await servicio.ObtenerTodos();

        // 3. ASSERT
        Assert.Equal(2, lista.Count); // ¿Hay 2 artistas?
        Assert.Contains(lista, a => a.Nombre == "Bad Bunny"); // ¿Está Bad Bunny?
    }
}