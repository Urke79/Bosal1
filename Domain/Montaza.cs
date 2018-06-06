using System;

namespace Domain
{
    public class Montaza
    {
        public int MontazaId { get; set; }
        public string Adresa { get; set; }
        public string Radnik { get; set; }
        public DateTime Datum { get; set; }
        public TimeSpan Vreme { get; set; }
    }
}
