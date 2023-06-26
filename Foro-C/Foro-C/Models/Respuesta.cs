using Foro_C.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Foro_C.Models
{
    public class Respuesta
    {
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        public int MiembroId { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        public int PreguntaId { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(10000, MinimumLength = 3, ErrorMessage = ErrorMessages.StrLenght)]
        public string Descripcion { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DataFormats.fecha)]
        public DateTime Fecha { get; set; }

        public List<Reaccion> Reacciones { get; set; }

        public Miembro Miembro { get; set; }

        public Pregunta Pregunta { get; set; }

    }
}
