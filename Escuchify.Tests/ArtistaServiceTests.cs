using System;
using System.Threading.Tasks;
using Xunit;
using escuchify_api.Services;
using escuchify_api.DTOs; // Agregamos DTOs porque el método Crear lo utiliza

namespace Escuchify.Tests
{
    public class ArtistaServiceTests
    {
        [Fact]
        public async Task Crear_DeberiaLanzarExcepcion_SiElNombreEstaVacio()
        {
            // 1. Arrange
            // Instanciamos ArtistasService (sin 's' al final) con parámetros nulos para la prueba unitaria pura
            var artistaService = new ArtistasService(null, null); 
            
            // Instanciamos el DTO con el nombre vacío para forzar tu validación
            var artistaInvalido = new ArtistaDto { Nombre = "" };

            // 2 & 3. Act & Assert
            // Apuntamos al método Crear, que es donde tenés la lógica de validación
            var excepcion = await Assert.ThrowsAsync<ArgumentException>(() => 
                artistaService.Crear(artistaInvalido)
            );

            Assert.Equal("El nombre del artista es obligatorio.", excepcion.Message);
        }
    }
}