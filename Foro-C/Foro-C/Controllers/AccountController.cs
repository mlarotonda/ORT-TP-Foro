using Foro_C.Data;
using Foro_C.Helpers;
using Foro_C.Models;
using Foro_C.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Foro_C.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Persona> _usermanager;
        private readonly SignInManager<Persona> _signInManager;
        private readonly RoleManager<Rol> _roleManager;
        private readonly ForoContext _context;

        public AccountController(UserManager<Persona> _usermanager, SignInManager<Persona> _signInManager, RoleManager<Rol> roleManager, ForoContext context)
        {
            this._usermanager = _usermanager;
            this._signInManager = _signInManager;
            this._roleManager = roleManager;
            this._context = context;
        }

        public async Task<IActionResult> Index()
        {
            Miembro miembro = CurrentUser();
            if (miembro.Id == null || miembro == null)
            {
                return NotFound();
            }

            return View(miembro);
        }

        public async Task<IActionResult> Edit()
        {
            Miembro miembro = CurrentUser();
            if (miembro.Id == null || miembro == null)
            {
                return NotFound();
            }

            return View(miembro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Telefono,Id,UserName,Password,Nombre,Apellido")] Miembro miembro)
        {
            ModelState.Remove("Email");
            ModelState.Remove("FechaAlta");
            if (ModelState.IsValid)
            {
                try
                {
                    var miembroBD = _context.Miembros.Find(miembro.Id);
                    if (miembroBD != null)
                    {
                        miembroBD.Nombre = miembro.Nombre;
                        miembroBD.Apellido = miembro.Apellido;
                        miembroBD.Telefono = miembro.Telefono;
                    }

                    _context.Update(miembroBD);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MiembroExists(miembro.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(miembro);
        }

        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrar([Bind("UserName,Password,PasswordConfirm,Nombre,Apellido,Email")] RegistroUsuario viewModel)
        {

            if (ModelState.IsValid)
            {
                //Se avanza con la registracion
                Miembro miembro = new Miembro()
                {
                    UserName = viewModel.UserName,
                    Nombre = viewModel.Nombre,
                    Apellido = viewModel.Apellido,
                    Email = viewModel.Email,
                    FechaAlta = DateTime.Now
                };

                var resultadoCreate = await _usermanager.CreateAsync(miembro, viewModel.Password);

                if (resultadoCreate.Succeeded)
                {
                    //Agregar rol
                    if (viewModel.UserName.Contains("admin"))
                    {
                        var resultadoAddRole = await _usermanager.AddToRoleAsync(miembro, UsersConfig.AdminRoleName);
                    }
                    else
                    {
                        var resultadoAddRole = await _usermanager.AddToRoleAsync(miembro, UsersConfig.UserRoleName);
                    }
                    await _signInManager.SignInAsync(miembro, false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in resultadoCreate.Errors)
                {
                    //Proceso errores al momento de crear
                    ModelState.AddModelError(String.Empty, error.Description);
                }
            }
            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ListarRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        public IActionResult AccesoDenegado(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public IActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(Login viewModel)
        {
            if (ModelState.IsValid)
            {
                var resultadoSing = await _signInManager.PasswordSignInAsync(viewModel.UserName, viewModel.Password, viewModel.Recordarme, false);
                if (resultadoSing.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Inicio de sesion invalido.");
            }

            return View(viewModel);
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public Miembro CurrentUser()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return _context.Miembros.FirstOrDefault(p => p.NormalizedUserName == User.Identity.Name.ToUpper());
            }

            return null;
        }

        private bool MiembroExists(int id)
        {
            return _context.Miembros.Any(e => e.Id == id);
        }

    }
}