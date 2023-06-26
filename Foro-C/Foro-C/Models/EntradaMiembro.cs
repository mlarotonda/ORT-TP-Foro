using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foro_C.Models
{
    public class EntradaMiembro
    {
        [Key, ForeignKey("Entrada")]
        public int EntradaId { get; set; }

        [Key, ForeignKey("Miembro")]
        public int MiembroId { get; set; }

        public Entrada Entrada { get; set; }

        public Miembro Miembro { get; set; }

    }
}
