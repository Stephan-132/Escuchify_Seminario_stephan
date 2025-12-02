using escuchify_api.Data;
using escuchify_api.Core.Entities;
using escuchify_api.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;

namespace escuchify_api.Services;

public class ArtistasService
{
    private readonly AppDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;

    public ArtistasService(AppDbContext context, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
    }

    // Método para OBTENER todos los artistas
    public async Task<List<ArtistaDto>> ObtenerTodos()
    {
        var artistas = await _context.Artistas.ToListAsync();

        // Convertimos de Entidad (BD) a DTO (Público)
        return artistas.Select(a => new ArtistaDto
        {
            Id = a.Id,
            Nombre = a.Nombre,
            Genero = a.Genero,
            ImagenUrl = a.ImagenUrl,
            Biografia = a.Biografia
        }).ToList();
    }

    // Método para CREAR un artista nuevo
    public async Task<Artista> Crear(ArtistaDto dto)
    {
        if (string.IsNullOrEmpty(dto.Nombre))
        {
            throw new ArgumentException("El nombre del artista es obligatorio.");
        }

        if (string.IsNullOrEmpty(dto.Genero))
        {
            throw new ArgumentException("El género del artista es obligatorio.");
        }
        
        var biografia = dto.Biografia;
        if (string.IsNullOrEmpty(biografia))
        {
            biografia = await ObtenerResumenDeWikipedia(dto.Nombre) ?? "No se encontró biografía en Wikipedia.";
        }

        var nuevoArtista = new Artista 
        {
            Nombre = dto.Nombre,
            Genero = dto.Genero,
            ImagenUrl = dto.ImagenUrl,
            Biografia = biografia
        };

        _context.Artistas.Add(nuevoArtista);
        
        await _context.SaveChangesAsync(); 
        
        return nuevoArtista;
    }

    private async Task<string?> ObtenerResumenDeWikipedia(string nombreArtista)
    {
        // Intento 1: Buscar con el nombre completo
        var resumen = await BuscarEnWikipedia(nombreArtista);
        if (!string.IsNullOrEmpty(resumen))
        {
            return resumen;
        }

        return null; // Si ningún intento funcionó
    }

    private async Task<string?> BuscarEnWikipedia(string terminoBusqueda)
    {
        var cliente = _httpClientFactory.CreateClient();
        var url = $"https://es.wikipedia.org/w/api.php?action=query&prop=extracts&exintro&explaintext&format=json&redirects=1&titles={Uri.EscapeDataString(terminoBusqueda)}";

        try
        {
            var respuesta = await cliente.GetAsync(url);
            if (!respuesta.IsSuccessStatusCode) return null;

            var json = await respuesta.Content.ReadAsStringAsync();
            var wikiRespuesta = JsonSerializer.Deserialize<WikipediaResponse>(json);

            var pagina = wikiRespuesta?.Query?.Pages?.Values.FirstOrDefault();
            
            // Si la página no existe (pageid -1) o el extracto está vacío o es una desambiguación, intentamos una búsqueda
            if (pagina == null || pagina.PageId == -1 || string.IsNullOrEmpty(pagina.Extract) || pagina.Extract.EndsWith("puede referirse a:") || pagina.Extract.Contains("homónimos"))
            {
                // Si la búsqueda directa falla, usamos el buscador de Wikipedia
                var urlBusqueda = $"https://es.wikipedia.org/w/api.php?action=query&list=search&srsearch={Uri.EscapeDataString(terminoBusqueda)}&format=json";
                var respuestaBusqueda = await cliente.GetAsync(urlBusqueda);
                if (!respuestaBusqueda.IsSuccessStatusCode) return null;

                var jsonBusqueda = await respuestaBusqueda.Content.ReadAsStringAsync();
                var wikiBusquedaRespuesta = JsonSerializer.Deserialize<WikipediaSearchResponse>(jsonBusqueda);

                var primerResultado = wikiBusquedaRespuesta?.Query?.Search?.FirstOrDefault();
                if (primerResultado != null)
                {
                    // Volvemos a llamar a la función, pero con el título exacto que encontró la búsqueda
                    return await BuscarEnWikipedia(primerResultado.Title);
                }
                return null;
            }

            return pagina.Extract;
        }
        catch (Exception)
        {
            return null;
        }
    }

    // Método para traer un solo artista con sus discos
    public async Task<ArtistaDto?> ObtenerPorId(int id)
    {
        var artista = await _context.Artistas
            .Include(a => a.Discos) // <--- ¡MAGIA! Trae los discos relacionados
            .FirstOrDefaultAsync(a => a.Id == id);

        if (artista == null) return null;

        return new ArtistaDto
        {
            Id = artista.Id,
            Nombre = artista.Nombre,
            Genero = artista.Genero,
            ImagenUrl = artista.ImagenUrl,
            Biografia = artista.Biografia,
            // Convertimos los discos de la BD a DTOs
            Discos = artista.Discos.Select(d => new DiscoDto
            {
                Id = d.Id,
                Titulo = d.Titulo,
                AnioLanzamiento = d.AnioLanzamiento,
                Genero = d.Genero
            }).ToList()
        };
    }
    // Método para BORRAR (Delete)
    public async Task<bool> Borrar(int id)
    {
        // 1. Buscamos si existe
        var artista = await _context.Artistas.FindAsync(id);
        if (artista == null) return false;

        // 2. Lo borramos (EF Core se encarga del SQL)
        _context.Artistas.Remove(artista);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<bool> Actualizar(int id, ArtistaDto dto)
    {
        var artista = await _context.Artistas.FindAsync(id);
        if (artista == null) return false;

        artista.Nombre = dto.Nombre;
        artista.Genero = dto.Genero;
        artista.ImagenUrl = dto.ImagenUrl;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<string?> ActualizarBiografiaDesdeWikipedia(int id)
    {
        var artista = await _context.Artistas.FindAsync(id);
        if (artista == null) return null;

        var biografia = await ObtenerResumenDeWikipedia(artista.Nombre);
        if (string.IsNullOrEmpty(biografia))
        {
            return "No se encontró una nueva biografía en Wikipedia.";
        }

        artista.Biografia = biografia;
        await _context.SaveChangesAsync();

        return biografia;
    }


    public async Task<ResumenDto> ObtenerResumen()
    {
        var resumen = new ResumenDto
        {
            CantidadArtistas = await _context.Artistas.CountAsync(),
            CantidadDiscos = await _context.Discos.CountAsync(),
            CantidadCanciones = await _context.Canciones.CountAsync()
        };
        return resumen;
    }
}


// --- Clases para deserializar la respuesta de Wikipedia ---
public class WikipediaResponse
{
    [JsonPropertyName("query")]
    public Query? Query { get; set; }
}
public class WikipediaSearchResponse
{
    [JsonPropertyName("query")]
    public SearchQuery? Query { get; set; }
}

public class Query
{
    [JsonPropertyName("pages")]
    public Dictionary<string, Page>? Pages { get; set; }
    
}
public class SearchQuery
{
    [JsonPropertyName("search")]
    public List<SearchResult>? Search { get; set; }
}

public class SearchResult
{
    [JsonPropertyName("title")]
    public string Title { get; set; }
}

public class Page
{
    [JsonPropertyName("pageid")]
    public int PageId { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("extract")]
    public string? Extract { get; set; }
}
