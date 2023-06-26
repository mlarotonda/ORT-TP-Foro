using Foro_C.Data;
using Foro_C.Helpers;
using Foro_C.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Foro_C.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MiembrosController : Controller
    {
        private readonly ForoContext _context;
        private readonly UserManager<Persona> _userManager;

        public MiembrosController(ForoContext context, UserManager<Persona> userManager)
        {
            _context = context;
            this._userManager = userManager;
        }

        // GET: Miembros
        public async Task<IActionResult> Index()
        {
            return View(await _context.Miembros.ToListAsync());
        }

        // GET: Miembros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Miembros == null)
            {
                return NotFound();
            }

            var miembro = await _context.Miembros
                .FirstOrDefaultAsync(m => m.Id == id);
            if (miembro == null)
            {
                return NotFound();
            }
            return View(miembro);
        }

        // GET: Miembros/Create 
        public IActionResult Create()
        {
            #region objeto_test
            Miembro miembro = new Miembro()
            {
                Telefono = "1122334455",
                UserName = "testMiembro",
                Nombre = "testJorge",
                Apellido = "testLopez",
                Email = "miembro@test.com"
            };
            #endregion
            return View(miembro);
        }

        // POST: Miembros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(bool esAdmin, [Bind("Telefono,Id,UserName,Password,Nombre,Apellido,Email")] Miembro miembro)
        {
            miembro.FechaAlta = DateTime.Now;
            if (ModelState.IsValid)
            {
                //_context.Add(miembro);
                // await _context.SaveChangesAsync();
                //Creacion usuario
                var resultadoNewMiembro = await _userManager.CreateAsync(miembro, UsersConfig.PasswordGeneric);

                if (resultadoNewMiembro.Succeeded)
                {
                    string roleName = esAdmin ? UsersConfig.AdminRoleName : UsersConfig.UserRoleName;
                    var resultadoAddRole = await _userManager.AddToRoleAsync(miembro, roleName);
                    if (resultadoAddRole.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                }
                return RedirectToAction(nameof(Index));
            }
            return View(miembro);
        }

        // GET: Miembros/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Miembros == null)
            {
                return NotFound();
            }

            var miembro = await _context.Miembros.FindAsync(id);
            if (miembro == null)
            {
                return NotFound();
            }
            return View(miembro);
        }

        // POST: Miembros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Telefono,Id,UserName,Password,Nombre,Apellido")] Miembro miembro)
        {
            if (id != miembro.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Email");
            ModelState.Remove("FechaAlta");
            if (ModelState.IsValid)
            {
                try
                {
                    var miembroBD = _context.Miembros.Find(id);
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

        // GET: Miembros/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Miembros == null)
            {
                return NotFound();
            }

            var miembro = await _context.Miembros
                .FirstOrDefaultAsync(m => m.Id == id);
            if (miembro == null)
            {
                return NotFound();
            }

            return View(miembro);
        }

        // POST: Miembros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Miembros == null)
            {
                return Problem("Entity set 'ForoContext.Miembros'  is null.");
            }
            var miembro = await _context.Miembros.FindAsync(id);
            if (miembro != null)
            {
                _context.Miembros.Remove(miembro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MiembroExists(int id)
        {
            return _context.Miembros.Any(e => e.Id == id);
        }
    }
}
