using Foro_C.Data;
using Foro_C.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Foro_C.Controllers
{

    public class EntradasController : Controller
    {
        private readonly ForoContext _context;
        private readonly UserManager<Persona> _userManager;


        public EntradasController(ForoContext context, UserManager<Persona> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Entradas
        public async Task<IActionResult> Index()
        {
            var foroContext = _context.Entradas.Include(e => e.Categoria).Include(e => e.Miembro);
            return View(await foroContext.ToListAsync());
        }

        // GET: Entradas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Entradas == null)
            {
                return NotFound();
            }

            var entrada = await _context.Entradas
                .Include(e => e.Categoria)
                .Include(e => e.Miembro)
                .Include(c => c.Preguntas)
                    .ThenInclude(p => p.Miembro)
                .Include(c => c.Preguntas)
                    .ThenInclude(p => p.Respuestas)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (entrada == null)
            {
                return NotFound();
            }

            return View(entrada);
        }

        // GET: Entradas/Create
        [Authorize]
        public IActionResult Create(int categoriaId)
        {

            ViewData["CategoriaId"] = categoriaId;
            return View();
        }

        // POST: Entradas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int categoriaId, [Bind("Id,Titulo,Privada,CategoriaId,MiembroId")] Entrada entrada)
        {
            entrada.Fecha = DateTime.Now;
            if (ModelState.IsValid)
            {
                var usuarioActual = _userManager.GetUserAsync(User).Result;
                entrada.MiembroId = usuarioActual.Id;
                entrada.CategoriaId = categoriaId;
                _context.Add(entrada);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "Preguntas", new { entradaId = entrada.Id });
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", entrada.CategoriaId);
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "Id", "UserName", entrada.MiembroId);
            return View(entrada);
        }
        // GET: Entradas/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Entradas == null)
            {
                return NotFound();
            }

            var entrada = await _context.Entradas.FindAsync(id);
            if (entrada == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", entrada.CategoriaId);
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "Id", "UserName", entrada.MiembroId);
            return View(entrada);
        }

        // POST: Entradas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Fecha,Privada,CategoriaId,MiembroId")] Entrada entrada)
        {
            if (id != entrada.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(entrada);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntradaExists(entrada.Id))
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
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre", entrada.CategoriaId);
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "Id", "UserName", entrada.MiembroId);
            return View(entrada);
        }

        // GET: Entradas/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Entradas == null)
            {
                return NotFound();
            }

            var entrada = await _context.Entradas
                .Include(e => e.Categoria)
                .Include(e => e.Miembro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (entrada == null)
            {
                return NotFound();
            }

            return View(entrada);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entrada = await _context.Entradas.FindAsync(id);
            if (entrada == null)
            {
                return NotFound();
            }


            if (!User.IsInRole("Admin"))
            {
                // El usuario actual no tiene permisos para eliminar la entrada
                return Unauthorized();
            }

            _context.Entradas.Remove(entrada);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult MisEntradas()
        {
            var usuarioActual = _userManager.GetUserAsync(User).Result;
            if (usuarioActual == null)
            {
                // El usuario no está autenticado
                return RedirectToAction("Login", "Account");
            }

            var misEntradas = _context.Entradas
                .Include(e => e.Categoria)
                .Where(e => e.MiembroId == usuarioActual.Id)
                .OrderByDescending(e => e.Fecha)
                .ToList();

            return View(misEntradas);
        }

        private bool EntradaExists(int id)
        {
            return _context.Entradas.Any(e => e.Id == id);
        }

        private string GetCurrentUser()
        {
            return HttpContext.User.Identity.Name;
        }
    }
}
