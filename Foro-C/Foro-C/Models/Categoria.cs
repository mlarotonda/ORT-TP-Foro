using Foro_C.Helpers;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Foro_C.Models
{
    [Index(nameof(Nombre), IsUnique = true)]
    public class Categoria
    {
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = ErrorMessages.StrLenght)]
        public string Nombre { get; set; }

        public List<Entrada> Entradas { get; set; }
    }
}
