using Foro_C.Helpers;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Foro_C.Models
{
    public class Persona : IdentityUser<int>
    {
        //public int Id { get; set; } Se comenta ya que hereda de la clase IdentityUser

        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(20, MinimumLength = 4, ErrorMessage = ErrorMessages.StrLenght)]
        [RegularExpression(RegExs.UserName, ErrorMessage = ErrorMessages.NotValid)]
        [DisplayName("Usuario")]
        public override string UserName
        {
            get { return base.UserName; }
            set { base.UserName = value; }
        }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(30, MinimumLength = 1, ErrorMessage = ErrorMessages.StrLenght)]
        [RegularExpression(RegExs.Alfabetico, ErrorMessage = ErrorMessages.StrAlfabetico)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(30, MinimumLength = 1, ErrorMessage = ErrorMessages.StrLenght)]
        [RegularExpression(RegExs.Alfabetico, ErrorMessage = ErrorMessages.StrAlfabetico)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(50, MinimumLength = 5, ErrorMessage = ErrorMessages.StrLenght)]
        [EmailAddress(ErrorMessage = ErrorMessages.Email)]
        public override string Email
        {
            get { return base.Email; }
            set { base.Email = value; }
        }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DataFormats.fecha)]
        public DateTime FechaAlta { get; set; }

        public string NombreCompleto
        {
            get
            {
                if (string.IsNullOrEmpty(Apellido) && string.IsNullOrEmpty(Nombre)) return "Sin definir";
                if (string.IsNullOrEmpty(Apellido)) return Nombre;
                if (string.IsNullOrEmpty(Nombre)) return Apellido.ToUpper();
                return $"{Apellido.ToUpper()}, {Nombre}";
            }
        }
    }
}
