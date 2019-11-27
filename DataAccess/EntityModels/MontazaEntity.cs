using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityModels
{
    [Table("Montazas")]
    public class MontazaEntity
    {
        [Key]
        public int? MontazaId { get; set; }
        public string Adresa { get; set; }
        public string Radnik { get; set; }
        public DateTime Datum { get; set; }
        public TimeSpan Vreme { get; set; }
    }
}
