namespace Data.Entities
{
    
    public class Usuario
    {
        public int Id { get; set; }
        public string Correo { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}