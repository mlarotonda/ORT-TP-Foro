using Foro_C.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foro_C.Models
{
    public class Reaccion
    {
        [Required(ErrorMessage = ErrorMessages.Required)]
        [Key, ForeignKey("Miembro")]
        public int MiembroId { get; set; }

        [Key, ForeignKey("Respuesta")]
        [Required(ErrorMessage = ErrorMessages.Required)]
        public int RespuestaId { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DataFormats.fecha)]
        public DateTime Fecha { get; set; }

        public bool? MeGusta { get; set; }

        public Miembro Miembro { get; set; }

        public Respuesta Respuesta { get; set; }

    }
}
