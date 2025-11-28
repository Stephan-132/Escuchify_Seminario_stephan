using System.Text.Json.Serialization;

namespace escuchify_api.Core.Entities;

public class Discos
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public int AnioLanzamiento { get; set; }
    public string Genero { get; set; } = string.Empty;
    public string SelloDiscografico { get; set; } = string.Empty;
    public string tipodisco { get; set; } = string.Empty;

    // Relación 1: Muchos (Canciones)
    public ICollection<Canciones> Canciones { get; set; } 

    // NUEVA Relación Muchos a Muchos (Artistas)
    public List<Artista> Artistas { get; set; } = new();

    public Discos() 
    {
        Canciones = new List<Canciones>();
    }
}