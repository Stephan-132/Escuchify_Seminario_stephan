using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using escuchify_api.Data;
using escuchify_api.DTOs;
using escuchify_api.Services;

namespace Escuchify.Tests
{
    public class ArtistasIntegracionTests
    {
        [Fact]
        public async Task Crear_DeberiaGuardarElArtistaEnLaBaseDeDatos()
        {
            // 1. Arrange: Preparamos la base de datos temporal en la memoria RAM
            var options = new DbContextOptionsBuilder<AppDbContext>()
                // Le ponemos un ID único al nombre para que si corrés muchas pruebas no choquen los datos
                .UseInMemoryDatabase(databaseName: "EscuchifyDb_" + Guid.NewGuid().ToString())
                .Options;

            // Creamos un DTO con datos válidos. 
            // IMPORTANTE: Al mandar la biografía llena, tu código evita llamar a Wikipedia.
            var nuevoArtistaDto = new ArtistaDto 
            { 
                Nombre = "Los Piojos",
                Genero = "Rock",
                Biografia = "Banda de rock argentino." 
            };

            // 2. Act: Usamos el contexto para simular el guardado
            using (var context = new AppDbContext(options))
            {
                // Pasamos null al HttpClientFactory total no lo vamos a usar
                var servicio = new ArtistasService(context, null);
                
                await servicio.Crear(nuevoArtistaDto);
            }

            // 3. Assert: Abrimos una nueva conexión a esa misma base en memoria para verificar
            using (var context = new AppDbContext(options))
            {
                // Buscamos si "Los Piojos" realmente se insertó en el DbSet de Artistas
                var artistaGuardado = await context.Artistas.FirstOrDefaultAsync(a => a.Nombre == "Los Piojos");
                
                // Verificamos que no sea nulo y que los datos coincidan
                Assert.NotNull(artistaGuardado);
                Assert.Equal("Rock", artistaGuardado.Genero);
                Assert.Equal("Banda de rock argentino.", artistaGuardado.Biografia);
            }
        }
    }
}