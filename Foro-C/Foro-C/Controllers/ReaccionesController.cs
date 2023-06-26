using Foro_C.Data;
using Foro_C.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Foro_C.Controllers
{
    public class ReaccionesController : Controller
    {
        private readonly ForoContext _context;
        private readonly UserManager<Persona> _userManager;

        public ReaccionesController(ForoContext context, UserManager<Persona> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reacciones
        public async Task<IActionResult> Index()
        {
            var foroContext = _context.Reacciones.Include(r => r.Miembro).Include(r => r.Respuesta);
            return View(await foroContext.ToListAsync());
        }

        // GET: Reacciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reacciones == null)
            {
                return NotFound();
            }

            var reaccion = await _context.Reacciones
                .Include(r => r.Miembro)
                .Include(r => r.Respuesta)
                .FirstOrDefaultAsync(m => m.RespuestaId == id);
            if (reaccion == null)
            {
                return NotFound();
            }

            return View(reaccion);
        }

        // GET: Reacciones/Create
        [Authorize]
        public IActionResult Create(int respuestaId, bool meGusta)
        {
            ViewData["MeGusta"] = meGusta;
            ViewData["RespuestaId"] = respuestaId;
            return View();
        }

        // POST: Reacciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int respuestaId, bool meGusta, [Bind("MeGusta,RespuestaId,MiembroId")] Reaccion reaccion)
        {
            reaccion.Fecha = DateTime.Now;
            if (ModelState.IsValid)
            {
                var usuarioActual = _userManager.GetUserAsync(User).Result;
                reaccion.MiembroId = usuarioActual.Id;
                reaccion.RespuestaId = respuestaId;
                reaccion.MeGusta = meGusta;
                _context.Add(reaccion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "Id", "UserName", reaccion.MiembroId);
            ViewData["RespuestaId"] = new SelectList(_context.Respuestas, "Id", "Descripcion", reaccion.RespuestaId);
            return View(reaccion);
        }

        // GET: Reacciones/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reacciones == null)
            {
                return NotFound();
            }

            var reaccion = await _context.Reacciones.FindAsync(id);
            if (reaccion == null)
            {
                return NotFound();
            }
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "Id", "UserName", reaccion.MiembroId);
            ViewData["RespuestaId"] = new SelectList(_context.Respuestas, "Id", "Descripcion", reaccion.RespuestaId);
            return View(reaccion);
        }

        // POST: Reacciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Fecha,MeGusta,RespuestaId,MiembroId")] Reaccion reaccion)
        {
            if (id != reaccion.RespuestaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reaccion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReaccionExists(reaccion.RespuestaId))
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
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "Id", "UserName", reaccion.MiembroId);
            ViewData["RespuestaId"] = new SelectList(_context.Respuestas, "Id", "Descripcion", reaccion.RespuestaId);
            return View(reaccion);
        }

        // GET: Reacciones/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reacciones == null)
            {
                return NotFound();
            }

            var reaccion = await _context.Reacciones
                .Include(r => r.Miembro)
                .Include(r => r.Respuesta)
                .FirstOrDefaultAsync(m => m.RespuestaId == id);
            if (reaccion == null)
            {
                return NotFound();
            }

            return View(reaccion);
        }

        // POST: Reacciones/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reacciones == null)
            {
                return Problem("Entity set 'ForoContext.Reacciones'  is null.");
            }
            var reaccion = await _context.Reacciones.FindAsync(id);
            if (reaccion != null)
            {
                _context.Reacciones.Remove(reaccion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public IActionResult EliminarReaccion(int miembroId, int respuestaId, int preguntaId)
        {
            var reaccion = _context.Reacciones.FirstOrDefault(r => r.MiembroId == miembroId && r.RespuestaId == respuestaId);

            if (reaccion == null)
            {
                return RedirectToAction("Index");
            }

            _context.Reacciones.Remove(reaccion);
            _context.SaveChanges();

            return RedirectToAction("Details", "Preguntas", new { id = preguntaId });
        }

        [Authorize]
        public IActionResult CrearReaccion(int respuestaId, bool meGusta, int preguntaId)
        {
            int userId = _userManager.GetUserAsync(User).Result.Id;
            if (ReaccionExists(respuestaId, userId))
            {
                var reaccionExistente = _context.Reacciones.FirstOrDefault(r => r.MiembroId == userId && r.RespuestaId == respuestaId);
                _context.Reacciones.Remove(reaccionExistente);
                _context.SaveChanges();
            }
            Reaccion reaccion = new Reaccion();
            reaccion.Fecha = DateTime.Now;
            if (ModelState.IsValid)
            {
                reaccion.MiembroId = userId;
                reaccion.RespuestaId = respuestaId;
                reaccion.MeGusta = meGusta;
                _context.Reacciones.Add(reaccion);
                _context.SaveChanges();
            }
            return RedirectToAction("Details", "Preguntas", new { id = preguntaId });
        }

        private bool ReaccionExists(int id)
        {
            return _context.Reacciones.Any(e => e.RespuestaId == id);
        }

        private bool ReaccionExists(int respuestaId, int miembroId)
        {
            return _context.Reacciones.Any(e => e.RespuestaId == respuestaId && e.MiembroId == miembroId);
        }
    }
}
