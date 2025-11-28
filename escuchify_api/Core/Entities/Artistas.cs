using System.Text.Json.Serialization;

namespace escuchify_api.Core.Entities;

public class Artista
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Genero { get; set; } = string.Empty;
    public string Biografia { get; set; } = string.Empty;
    public string ImagenUrl { get; set; } = string.Empty; // Para la foto automática

    // Relación: Un artista tiene muchos discos
    [JsonIgnore] // Evita bucles infinitos al pedir datos
    public List<Discos> Discos { get; set; } = new();
}