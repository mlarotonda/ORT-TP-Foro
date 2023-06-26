using Foro_C.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Foro_C.Models
{
    public class Miembro : Persona
    {

        [Phone(ErrorMessage = ErrorMessages.Phone)]
        public string Telefono { get; set; }

        public List<Pregunta> Preguntas { get; set; }

        public List<Respuesta> Respuestas { get; set; }

        //public List<Pregunta> PreguntasQueMeGustan { get; set; }

        //public List<Respuesta> RespuestasQueMeGustan { get; set; }

        public List<EntradaMiembro> EntradasHabilitadas { get; set; }

        public List<Reaccion> Reacciones { get; set; }

    }
}
