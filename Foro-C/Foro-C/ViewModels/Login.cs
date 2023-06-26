using Foro_C.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Foro_C.ViewModels
{
    public class Login
    {

        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(20, MinimumLength = 4, ErrorMessage = ErrorMessages.StrLenght)]
        [RegularExpression(RegExs.UserName, ErrorMessage = ErrorMessages.NotValid)]
        [DisplayName("Usuario")]
        public string UserName { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(40, MinimumLength = 8, ErrorMessage = ErrorMessages.StrLenght)]
        [DataType(DataType.Password)]
        [DisplayName("Contraseña")]
        public string Password { get; set; }

        public Boolean Recordarme { get; set; }


    }
}
