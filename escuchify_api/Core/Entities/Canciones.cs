using System.Text.Json.Serialization;

namespace escuchify_api.Core.Entities;

public class Canciones
{
    public int Id { get; set; }
    
    // CORRECCIÓN: Inicializamos para eliminar las advertencias
    public string Titulo { get; set; } = string.Empty;
    public string Duracion { get; set; } = string.Empty;
    public string Genero { get; set; } = string.Empty;

    public int DiscosID { get; set; }

    [JsonIgnore] 
    public Discos? discos { get; set; }

    public Canciones() { } 

    public Canciones(int id, string titulo, string duracion, string genero)
    {
        Id = id;
        Titulo = titulo;
        Duracion = duracion;
        Genero = genero;
    }
}