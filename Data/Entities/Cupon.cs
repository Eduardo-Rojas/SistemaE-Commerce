using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Entities
{
    public class Cupon
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public decimal MontoDescuento { get; set; }
        public int LimiteUsos { get; set; }
        public int UsosActuales { get; set; }
    }
}
