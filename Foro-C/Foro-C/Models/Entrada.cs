using Foro_C.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Foro_C.Models
{
    public class Entrada
    {
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        public int MiembroId { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = ErrorMessages.StrLenght)]
        public string Titulo { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DataFormats.fecha)]
        public DateTime Fecha { get; set; }

        public bool Privada { get; set; } = false;

        public List<Pregunta> Preguntas { get; set; }

        public List<EntradaMiembro> MiembrosHabilitados { get; set; }

        public Categoria Categoria { get; set; }

        public Miembro Miembro { get; set; }
    }
}
