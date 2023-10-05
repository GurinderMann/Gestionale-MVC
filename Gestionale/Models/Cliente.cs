using System.Collections.Generic;

namespace Gestionale.Models
{
    public class Cliente
    {
        public int idCliente { get; set; }
        public string Nome { get; set; }
        public string CFoPIVA { get; set; }

        public int Value { get; set; }
        public string Text { get; set; }
        public static List<Cliente> ListaClienti { get; set; } = new List<Cliente>();
    }
}