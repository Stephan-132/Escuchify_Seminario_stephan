using TechTalk.SpecFlow;
using Xunit;
using escuchify_api.Services;
using escuchify_api.Data;
using escuchify_api.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using Moq; // Vamos a usar Moq, es el estándar para esto

namespace Escuchify.Tests.Steps
{
    [Binding]
    public class ObtenerBiografiaSteps
    {
        private AppDbContext _context;
        private ArtistasService _servicio;
        private int _artistaId;
        private string _resultadoBiografia;

        public ObtenerBiografiaSteps()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "EscuchifyBddDb")
                .Options;
            _context = new AppDbContext(options);
            
            // Usamos Moq para crear un IHttpClientFactory que no falle
            var mockFactory = new Mock<IHttpClientFactory>();
            mockFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(new HttpClient());
            
            _servicio = new ArtistasService(_context, mockFactory.Object);
        }

        [Given(@"que el artista ""(.*)"" existe en mi base de datos")]
        public async Task DadoQueElArtistaExisteEnMiBaseDeDatos(string nombre)
        {
            var artista = new Artista { Nombre = nombre, Genero = "Rock" };
            _context.Artistas.Add(artista);
            await _context.SaveChangesAsync();
            _artistaId = artista.Id;
        }

        [When(@"solicito actualizar su biografía desde Wikipedia")]
        public async Task CuandoSolicitoActualizarSuBiografiaDesdeWikipedia()
        {
            _resultadoBiografia = await _servicio.ActualizarBiografiaDesdeWikipedia(_artistaId);
        }

        [Then(@"la biografía debería ser distinta de ""(.*)""")]
        public void EntoncesLaBiografiaDeberiaSerDistintaDe(string mensajeError)
        {
            Assert.NotEqual(mensajeError, _resultadoBiografia);
            Assert.NotEmpty(_resultadoBiografia);
        }
    }
}