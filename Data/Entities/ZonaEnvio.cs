using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Entities
{
    public class ZonaEnvio
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Costo { get; set; }
    }
}
