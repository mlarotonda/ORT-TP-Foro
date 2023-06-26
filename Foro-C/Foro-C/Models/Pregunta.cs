using Foro_C.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Foro_C.Models
{
    public class Pregunta
    {
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(300, MinimumLength = 3, ErrorMessage = ErrorMessages.StrLenght)]
        public string Descripcion { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DataFormats.fecha)]
        public DateTime Fecha { get; set; }

        public bool Activa { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        public int MiembroId { get; set; }
        public Miembro Miembro { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        public int EntradaId { get; set; }
        public Entrada Entrada { get; set; }

        public List<Respuesta> Respuestas { get; set; }
    }
}
