namespace escuchify_api.DTOs;

public class ArtistaDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Genero { get; set; } = string.Empty;
    public string ImagenUrl { get; set; } = string.Empty;
}