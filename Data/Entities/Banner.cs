namespace Data.Entities;

public class Banner
{
    public int Id { get; set; }
    public string ImagenUrl { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
}
