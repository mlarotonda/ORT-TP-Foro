using Foro_C.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Foro_C.ViewModels
{
    public class RegistroUsuario
    {

        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(20, MinimumLength = 4, ErrorMessage = ErrorMessages.StrLenght)]
        [RegularExpression(RegExs.UserName, ErrorMessage = ErrorMessages.NotValid)]
        [DisplayName("Usuario")]
        public string UserName { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(40, MinimumLength = 8, ErrorMessage = ErrorMessages.StrLenght)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(40, MinimumLength = 8, ErrorMessage = ErrorMessages.StrLenght)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = ErrorMessages.PasswordMissMatch)]
        public string PasswordConfirm { get; set; }

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
        public string Email { get; set; }

    }
}
