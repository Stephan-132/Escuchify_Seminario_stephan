using System.Net.Http.Json;
using escuchify_app.Models;

namespace escuchify_app.Services;

public class ApiService
{
    private readonly HttpClient _http;

    public ApiService(HttpClient http)
    {
        _http = http;
    }

    // --- ARTISTAS ---
    public async Task<List<Artista>> ObtenerArtistas()
    {
        try
        {
            // OJO: "api/artistas" debe coincidir con tu Controller en la API
            var respuesta = await _http.GetFromJsonAsync<List<Artista>>("api/artistas");
            return respuesta ?? new List<Artista>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return new List<Artista>();
        }
    }

    public async Task<(bool Success, string? ErrorMessage)> CrearArtista(Artista artista)
    {
        // TRUCO DE LIMPIEZA: Creamos un objeto anónimo SIN EL ID
        var datosLimpios = new 
        {
            Nombre = artista.Nombre,
            Genero = artista.Genero,
            ImagenUrl = artista.ImagenUrl,
            Biografia = artista.Biografia
        };

        var response = await _http.PostAsJsonAsync("api/artistas", datosLimpios);
        
        if (response.IsSuccessStatusCode)
        {
            return (true, null);
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            return (false, errorContent);
        }
    }
    public async Task<Artista?> ObtenerArtistaPorId(int id)
    {
        try
        {
            return await _http.GetFromJsonAsync<Artista>($"api/artistas/{id}");
        }
        catch
        {
            return null;
        }
    }
    public async Task CrearDisco(int artistaId, Disco disco)
    {
        // Enviamos el disco a la ruta: api/discos/{id del artista}
        await _http.PostAsJsonAsync($"api/discos/{artistaId}", disco);
    }
    public async Task BorrarArtista(int id)
    {
        await _http.DeleteAsync($"api/artistas/{id}");
    }
    // Obtener un disco con sus canciones
    public async Task<Disco?> ObtenerDiscoPorId(int id)
    {
        try
        {
            // Necesitarás crear este endpoint en DiscosController si no existe
            // O usar el include en el backend
            return await _http.GetFromJsonAsync<Disco>($"api/discos/{id}");
        }
        catch { return null; }
    }

    public async Task AgregarCancion(int discoId, Cancion cancion)
    {
        await _http.PostAsJsonAsync($"api/discos/{discoId}/canciones", cancion);
    }
    public async Task BorrarCancion(int id)
    {
        await _http.DeleteAsync($"api/discos/cancion/{id}");
    }
    public async Task BorrarDisco(int id)
    {
        await _http.DeleteAsync($"api/discos/{id}");
    }
    public async Task ActualizarArtista(int id, Artista artista)
    {
        await _http.PutAsJsonAsync($"api/artistas/{id}", artista);
    }

    public async Task<string?> ActualizarBiografia(int artistaId)
    {
        try
        {
            var response = await _http.PostAsync($"api/artistas/{artistaId}/actualizar-biografia", null);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
                return result.GetProperty("biografia").GetString();
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task ActualizarDisco(int id, escuchify_app.Models.Disco disco)
    {
        // TRUCO: Creamos un objeto anónimo SIN la lista de canciones
        var datosLimpios = new
        {
            Id = disco.Id,
            Titulo = disco.Titulo,
            AnioLanzamiento = disco.AnioLanzamiento,
            Genero = disco.Genero
        };

        await _http.PutAsJsonAsync($"api/discos/{id}", datosLimpios);
    }
    public async Task ActualizarCancion(int id, escuchify_app.Models.Cancion cancion)
    {
        // TRUCO: Objeto limpio sin relaciones
        var datosLimpios = new
        {
            Id = cancion.Id,
            Titulo = cancion.Titulo,
            Duracion = cancion.Duracion,
            Genero = cancion.Genero
        };

        // Asegúrate de que la ruta coincida con tu controlador (api/discos/cancion/{id})
        await _http.PutAsJsonAsync($"api/discos/cancion/{id}", datosLimpios);
    }
    public async Task<Resumen> ObtenerResumen()
    {
        try
        {
            var resumen = await _http.GetFromJsonAsync<Resumen>("api/artistas/resumen");
            return resumen ?? new Resumen();
        }
        catch { return new Resumen(); }
    }
}