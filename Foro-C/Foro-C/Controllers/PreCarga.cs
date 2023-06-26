using Foro_C.Data;
using Foro_C.Helpers;
using Foro_C.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Foro_C.Controllers
{
    public class PreCarga : Controller
    {
        private readonly UserManager<Persona> _userManager;
        private readonly RoleManager<Rol> _rolManager;
        private readonly SignInManager<Persona> _signInManager;
        private readonly ForoContext context;

        private List<string> roles = new List<string>() { "Admin", "User" };

        public PreCarga(UserManager<Persona> userManager, RoleManager<Rol> roleManager, ForoContext context, SignInManager<Persona> signInManager)
        {
            this._userManager = userManager;
            this._rolManager = roleManager;
            this.context = context;
            this._signInManager = signInManager;
        }

        public async Task<IActionResult> Seed()
        {


            CreateRoles().Wait();

            //Creacion Miembros
            Miembro miembro1 = await CreacionUsario("Miembro1", "miembro1@ort.edu.ar", false);
            Miembro miembro2 = await CreacionUsario("Miembro2", "miembro2@ort.edu.ar", false);
            Miembro miembro3 = await CreacionUsario("Miembro3", "miembro3@ort.edu.ar", false);
            Miembro miembro = await CreacionUsario("Admin", "admin@ort.edu.ar", true);


            //Creacion Categeorias
            int idCategoriaDeportes = await CreacionCategoria("Deportes");
            int idCategoriaMedicina = await CreacionCategoria("Medicina");
            int idCategoriaTecnologia = await CreacionCategoria("Tecnologia");
            int idCategoriaEntretenimiento = await CreacionCategoria("Entretenimiento");
            int idCategoriaMusica = await CreacionCategoria("Musica");

            //Creacion Entradas
            int idEntradaFutbol = await CreacionEntrada(miembro1, idCategoriaDeportes, false, "Futbol");
            int idEntradaRugby = await CreacionEntrada(miembro2, idCategoriaDeportes, false, "Rugby");
            int idEntradaBasquet = await CreacionEntrada(miembro3, idCategoriaDeportes, true, "Basquet");

            int idEntradaHospitales = await CreacionEntrada(miembro1, idCategoriaMedicina, false, "Hospitales");
            int idEntradaTecnologiaMed = await CreacionEntrada(miembro2, idCategoriaMedicina, false, "Tecnologia en la Medicina");

            int idEntradaInteligenciaArtifial = await CreacionEntrada(miembro1, idCategoriaTecnologia, false, "Inteligencai Artificial");
            int idEntradaComputadoras = await CreacionEntrada(miembro2, idCategoriaTecnologia, false, "Computadoras");

            int idEntradaSerieTv = await CreacionEntrada(miembro1, idCategoriaEntretenimiento, false, "Serie Tv");
            int idEntradaVideoJuegos = await CreacionEntrada(miembro3, idCategoriaEntretenimiento, true, "Video Juegos");

            // Creación de preguntas para la entrada de fútbol
            int idPregunta1Futbol = await CreacionPregunta(miembro1, idEntradaFutbol, "¿Cuál es tu equipo de fútbol favorito?", true);
            int idPregunta2Futbol = await CreacionPregunta(miembro2, idEntradaFutbol, "¿Quién es tu jugador favorito en la posición de delantero?", false);
            int idPregunta3Futbol = await CreacionPregunta(miembro3, idEntradaFutbol, "¿Qué opinas sobre el uso del VAR en el fútbol?", true);
            int idPregunta4Futbol = await CreacionPregunta(miembro1, idEntradaFutbol, "¿Cuál crees que es el mejor gol en la historia del fútbol?", true);

            // Creación de preguntas para la entrada de rugby
            int idPregunta1Rugby = await CreacionPregunta(miembro2, idEntradaRugby, "¿Cuál es la posición en el rugby que más te gusta?", true);
            int idPregunta2Rugby = await CreacionPregunta(miembro3, idEntradaRugby, "¿Has jugado rugby alguna vez? ¿Qué te pareció?", true);
            int idPregunta3Rugby = await CreacionPregunta(miembro1, idEntradaRugby, "¿Cuál consideras que es el torneo más importante de rugby a nivel mundial?", true);

            // Creación de preguntas para la entrada de básquet
            int idPregunta1Basquet = await CreacionPregunta(miembro3, idEntradaBasquet, "¿Qué jugador de básquet te inspira?", true);
            int idPregunta2Basquet = await CreacionPregunta(miembro1, idEntradaBasquet, "¿Cuál es tu equipo favorito de la NBA?", true);
            int idPregunta3Basquet = await CreacionPregunta(miembro2, idEntradaBasquet, "¿Cuál consideras que es el mejor jugador de la historia del básquet?", true);

            // Creación de preguntas para la entrada de hospitales
            int idPregunta1Hospitales = await CreacionPregunta(miembro2, idEntradaHospitales, "¿Has tenido alguna experiencia negativa en un hospital? ¿Podrías compartirla?", true);
            int idPregunta2Hospitales = await CreacionPregunta(miembro3, idEntradaHospitales, "¿Qué medidas crees que se podrían tomar para mejorar la calidad de los servicios de salud?", true);
            int idPregunta3Hospitales = await CreacionPregunta(miembro1, idEntradaHospitales, "¿Cuál es el aspecto más importante que debería tener un hospital?", true);

            // Creación de preguntas para la entrada de tecnología en la medicina
            int idPregunta1TecnologiaMed = await CreacionPregunta(miembro2, idEntradaTecnologiaMed, "¿Qué avances tecnológicos consideras más impactantes en el campo de la medicina?", true);
            int idPregunta2TecnologiaMed = await CreacionPregunta(miembro1, idEntradaTecnologiaMed, "¿Crees que la inteligencia artificial tendrá un papel importante en la medicina del futuro?", true);
            int idPregunta3TecnologiaMed = await CreacionPregunta(miembro3, idEntradaTecnologiaMed, "¿Cuáles son los desafíos éticos que enfrenta la tecnología en la medicina?", false);
            int idPregunta4TecnologiaMed = await CreacionPregunta(miembro2, idEntradaTecnologiaMed, "¿Cómo crees que la tecnología puede mejorar el diagnóstico de enfermedades?", true);

            // Respuestas para la pregunta 1 de fútbol
            int idRespuesta1Pregunta1Futbol = await CreacionRespuesta(miembro2, idPregunta1Futbol, "Mi equipo favorito es Boca.");
            int idRespuesta2Pregunta1Futbol = await CreacionRespuesta(miembro3, idPregunta1Futbol, "Soy hincha de River Plate.");
            int idRespuesta3Pregunta1Futbol = await CreacionRespuesta(miembro1, idPregunta1Futbol, "San Lorenzo!");

            // Respuestas para la pregunta 2 de fútbol
            int idRespuesta1Pregunta2Futbol = await CreacionRespuesta(miembro1, idPregunta2Futbol, "Mi jugador favorito en la posición de delantero es Lionel Messi.");
            int idRespuesta2Pregunta2Futbol = await CreacionRespuesta(miembro3, idPregunta2Futbol, "Prefiero a Cristiano Ronaldo.");

            // Respuestas para la pregunta 3 de fútbol
            int idRespuesta1Pregunta3Futbol = await CreacionRespuesta(miembro1, idPregunta3Futbol, "Creo que el VAR ha mejorado la justicia en el fútbol.");
            int idRespuesta2Pregunta3Futbol = await CreacionRespuesta(miembro2, idPregunta3Futbol, "Considero que el VAR ha generado más polémicas que beneficios.");

            // Respuestas para la pregunta 4 de fútbol
            int idRespuesta1Pregunta4Futbol = await CreacionRespuesta(miembro2, idPregunta4Futbol, "El mejor gol en la historia del fútbol es el de Diego Maradona contra Inglaterra en el Mundial 1986.");
            int idRespuesta2Pregunta4Futbol = await CreacionRespuesta(miembro3, idPregunta4Futbol, "Para mí, el mejor gol fue el de Zinedine Zidane en la final de la Champions League 2002.");

            // Respuestas para la pregunta 1 de rugby
            int idRespuesta1Pregunta1Rugby = await CreacionRespuesta(miembro1, idPregunta1Rugby, "Me gusta jugar de apertura en el rugby.");
            int idRespuesta2Pregunta1Rugby = await CreacionRespuesta(miembro2, idPregunta1Rugby, "Prefiero jugar de segunda línea.");

            // Respuestas para la pregunta 2 de rugby
            int idRespuesta1Pregunta2Rugby = await CreacionRespuesta(miembro3, idPregunta2Rugby, "Sí, he jugado rugby y me encantó la camaradería del equipo.");
            int idRespuesta2Pregunta2Rugby = await CreacionRespuesta(miembro1, idPregunta2Rugby, "No he tenido la oportunidad de jugar rugby, pero me gustaría intentarlo.");

            // Respuestas para la pregunta 3 de rugby
            int idRespuesta1Pregunta3Rugby = await CreacionRespuesta(miembro2, idPregunta3Rugby, "Considero que el torneo más importante de rugby a nivel mundial es la Copa del Mundo.");
            int idRespuesta2Pregunta3Rugby = await CreacionRespuesta(miembro3, idPregunta3Rugby, "Para mí, el Torneo de las Seis Naciones es el más emocionante.");

            // Respuestas para la pregunta 1 de básquet
            int idRespuesta1Pregunta1Basquet = await CreacionRespuesta(miembro3, idPregunta1Basquet, "Mi jugador favorito de básquet es Michael Jordan.");
            int idRespuesta2Pregunta1Basquet = await CreacionRespuesta(miembro1, idPregunta1Basquet, "Admiro mucho a LeBron James.");

            // Respuestas para la pregunta 2 de básquet
            int idRespuesta1Pregunta2Basquet = await CreacionRespuesta(miembro2, idPregunta2Basquet, "Mi equipo favorito de la NBA es Los Angeles Lakers.");
            int idRespuesta2Pregunta2Basquet = await CreacionRespuesta(miembro3, idPregunta2Basquet, "Soy seguidor de los Golden State Warriors.");

            // Respuestas para la pregunta 3 de básquet
            int idRespuesta1Pregunta3Basquet = await CreacionRespuesta(miembro1, idPregunta3Basquet, "Considero que Michael Jordan es el mejor jugador de la historia del básquet.");
            int idRespuesta2Pregunta3Basquet = await CreacionRespuesta(miembro2, idPregunta3Basquet, "Para mí, LeBron James es el mejor jugador en la actualidad.");

            // Respuestas para la pregunta 1 de hospitales
            int idRespuesta1Pregunta1Hospitales = await CreacionRespuesta(miembro2, idPregunta1Hospitales, "Sí, tuve una mala experiencia en un hospital cuando recibí un trato poco amable del personal.");
            int idRespuesta2Pregunta1Hospitales = await CreacionRespuesta(miembro3, idPregunta1Hospitales, "Afortunadamente, no he tenido experiencias negativas en hospitales.");

            // Respuestas para la pregunta 2 de hospitales
            int idRespuesta1Pregunta2Hospitales = await CreacionRespuesta(miembro3, idPregunta2Hospitales, "Creo que la tecnología ha mejorado la calidad de atención en los hospitales.");
            int idRespuesta2Pregunta2Hospitales = await CreacionRespuesta(miembro1, idPregunta2Hospitales, "Considero que la tecnología en los hospitales puede generar problemas de privacidad y seguridad.");

            // Respuestas para la pregunta 1 de tecnología en la medicina
            int idRespuesta1Pregunta1TecnologiaMed = await CreacionRespuesta(miembro1, idPregunta1TecnologiaMed, "La inteligencia artificial tiene el potencial de revolucionar la medicina.");
            int idRespuesta2Pregunta1TecnologiaMed = await CreacionRespuesta(miembro3, idPregunta1TecnologiaMed, "Creo que la inteligencia artificial en la medicina podría generar deshumanización en el trato con los pacientes.");

            // Respuestas para la pregunta 2 de tecnología en la medicina
            int idRespuesta1Pregunta2TecnologiaMed = await CreacionRespuesta(miembro2, idPregunta2TecnologiaMed, "La telemedicina ha demostrado ser una solución efectiva para brindar atención médica a distancia.");
            int idRespuesta2Pregunta2TecnologiaMed = await CreacionRespuesta(miembro1, idPregunta2TecnologiaMed, "La telemedicina no puede reemplazar por completo la atención médica presencial.");

            // Reacciones para la respuesta 1 de la pregunta 1 de fútbol
            await CreacionReaccion(miembro2, idRespuesta1Pregunta1Futbol, true);
            await CreacionReaccion(miembro3, idRespuesta1Pregunta1Futbol, false);

            // Reacciones para la respuesta 2 de la pregunta 1 de fútbol

            // Reacciones para la respuesta 3 de la pregunta 1 de fútbol
            await CreacionReaccion(miembro2, idRespuesta3Pregunta1Futbol, true);
            await CreacionReaccion(miembro3, idRespuesta3Pregunta1Futbol, true);

            // Reacciones para la respuesta 1 de la pregunta 2 de fútbol
            await CreacionReaccion(miembro1, idRespuesta1Pregunta2Futbol, true);
            await CreacionReaccion(miembro3, idRespuesta1Pregunta2Futbol, false);

            // Reacciones para la respuesta 2 de la pregunta 2 de fútbol
            await CreacionReaccion(miembro2, idRespuesta2Pregunta2Futbol, true);
            await CreacionReaccion(miembro1, idRespuesta2Pregunta2Futbol, true);

            // Reacciones para la respuesta 1 de la pregunta 3 de fútbol
            await CreacionReaccion(miembro2, idRespuesta1Pregunta3Futbol, true);
            await CreacionReaccion(miembro3, idRespuesta1Pregunta3Futbol, true);

            // Reacciones para la respuesta 2 de la pregunta 3 de fútbol
            await CreacionReaccion(miembro1, idRespuesta2Pregunta3Futbol, true);
            await CreacionReaccion(miembro2, idRespuesta2Pregunta3Futbol, false);

            // Reacciones para la respuesta 1 de la pregunta 4 de fútbol
            await CreacionReaccion(miembro1, idRespuesta1Pregunta4Futbol, true);
            await CreacionReaccion(miembro2, idRespuesta1Pregunta4Futbol, true);

            // Reacciones para la respuesta 2 de la pregunta 4 de fútbol
            await CreacionReaccion(miembro3, idRespuesta2Pregunta4Futbol, false);
            await CreacionReaccion(miembro1, idRespuesta2Pregunta4Futbol, true);

            // Reacciones para las respuestas de la entrada de deportes "Básquet"
            await CreacionReaccion(miembro1, idRespuesta1Pregunta2Basquet, true);
            await CreacionReaccion(miembro2, idRespuesta2Pregunta2Basquet, true);

            // Reacciones para las respuestas de la entrada de medicina "Hospitales"
            await CreacionReaccion(miembro2, idRespuesta1Pregunta1Hospitales, true);
            await CreacionReaccion(miembro3, idRespuesta2Pregunta2Hospitales, true);

            TempData["precarga-msg"] = "<script>alert('Pre-carga exitosa');</script>";
            return RedirectToAction("Index", "Home");
        }

        private async Task CreateRoles()
        {
            foreach (var rolName in roles)
            {
                if (!await _rolManager.RoleExistsAsync(rolName))
                {
                    await _rolManager.CreateAsync(new Rol() { Name = rolName });
                }
            }
        }


        private async Task<Miembro> CreacionUsario(string userName, string email, bool admin)
        {
            Miembro miembro = new Miembro()
            {
                UserName = userName,
                Nombre = UsersConfig.NameGeneric,
                Apellido = UsersConfig.ApellidoGeneric,
                Email = email,
                FechaAlta = DateTime.Now,
            };

            var resultadoCreate = await _userManager.CreateAsync(miembro, UsersConfig.PasswordGeneric);

            if (resultadoCreate.Succeeded)
            {
                if (admin)
                {
                    //Agregar rol Admin
                    var resultadoAddRole = await _userManager.AddToRoleAsync(miembro, UsersConfig.AdminRoleName);
                    // await _signInManager.SignInAsync(miembro, false);
                }
                else
                {
                    var resultadoAddRole = await _userManager.AddToRoleAsync(miembro, UsersConfig.UserRoleName);
                    //   await _signInManager.SignInAsync(miembro, false);
                }
            }

            return miembro;
        }

        private async Task<int> CreacionCategoria(string nombre)
        {
            Categoria categoria = new Categoria()
            {
                Nombre = nombre
            };

            context.Categorias.Add(categoria);
            try
            {
                await context.SaveChangesAsync();
                return categoria.Id;
            }
            catch (DbUpdateException ex)
            {
                SqlHelper.ValidarDuplicado(ex, ModelState, nombre);
                return 0; // o algún otro valor predeterminado si ocurre un error
            }
        }

        private async Task<int> CreacionEntrada(Miembro miembro, int categoriaId, bool privada, string titulo)
        {
            Entrada entrada = new Entrada()
            {
                MiembroId = miembro.Id,
                CategoriaId = categoriaId,
                Titulo = titulo,
                Fecha = DateTime.Now,
                Privada = privada
            };

            context.Entradas.Add(entrada);
            try
            {
                await context.SaveChangesAsync();
                return entrada.Id;
            }
            catch (DbUpdateException ex)
            {
                // Manejo de errores
                return 0; // o algún otro valor predeterminado si ocurre un error
            }
        }

        private async Task<int> CreacionPregunta(Miembro miembro, int entradaId, string
             descripcion, bool activa)
        {
            Pregunta pregunta = new Pregunta()
            {
                Descripcion = descripcion,
                Fecha = DateTime.Now,
                Activa = activa,
                MiembroId = miembro.Id,
                EntradaId = entradaId
            };

            context.Preguntas.Add(pregunta);
            try
            {
                await context.SaveChangesAsync();
                return pregunta.Id;
            }
            catch (DbUpdateException ex)
            {
                // Manejo de errores
                return 0; // o algún otro valor predeterminado si ocurre un error
            }
        }

        private async Task<int> CreacionRespuesta(Miembro miembro, int preguntaId, string descripcion)
        {
            Respuesta respuesta = new Respuesta()
            {
                MiembroId = miembro.Id,
                PreguntaId = preguntaId,
                Descripcion = descripcion,
                Fecha = DateTime.Now
            };

            context.Respuestas.Add(respuesta);
            try
            {
                await context.SaveChangesAsync();
                return respuesta.Id;
            }
            catch (DbUpdateException ex)
            {
                // Manejo de errores
                return 0; // o algún otro valor predeterminado si ocurre un error
            }
        }

        private async Task CreacionReaccion(Miembro miembro, int respuestaId, bool meGusta)
        {
            Reaccion reaccion = new Reaccion()
            {
                MiembroId = miembro.Id,
                RespuestaId = respuestaId,
                Fecha = DateTime.Now,
                MeGusta = meGusta // Ejemplo de valor para la propiedad MeGusta
            };

            context.Reacciones.Add(reaccion);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Manejo de errores
                ;
            }
        }
    }
}
