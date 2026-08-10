namespace Data.Entities;

public class Direccion
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string Calle { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;
    public string CodigoPostal { get; set; } = string.Empty;
    public string? Referencias { get; set; }
}
