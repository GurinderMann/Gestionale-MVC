using System;

namespace Gestionale.Models
{
    public class Stato
    {
        public int Id { get; set; }
        public DateTime DataConsegna { get; set; }
        public string StatoSpedizione { get; set; }
        public DateTime DataAggiornamento { get; set; }
        public bool InConsegna { get; set; }


    }
}