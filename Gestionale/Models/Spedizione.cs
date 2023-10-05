using System;
using System.Collections.Generic;

namespace Gestionale.Models
{
    public class Spedizione
    {
        public int id { get; set; }
        public DateTime DataSpedizione { get; set; }
        public int Peso { get; set; }
        public string Destinazione { get; set; }
        public string indirizzo { get; set; }
        public string Nome { get; set; }
        public decimal Costo { get; set; }

        public bool InConsegna { get; set; }

        public int IdCliente { get; set; }
        public string CF { get; set; }

        public int NumeroSpedizioni { get; set; }
        public static List<Spedizione> SpedizioneList { get; set; } = new List<Spedizione>();

    }
}