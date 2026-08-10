namespace Data.Entities;

public enum EstadoPedido
{
    Pendiente,
    Pagado,
    Enviado,
    Cancelado
}

public class Pedido
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public EstadoPedido Estado { get; set; }
    public string? NumeroGuia { get; set; }
    public decimal Total { get; set; }
    public DateTime Fecha { get; set; }
}
