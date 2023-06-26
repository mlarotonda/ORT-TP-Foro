using Foro_C.Data;
using Foro_C.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Foro_C.Controllers
{

    public class PreguntasController : Controller
    {
        private readonly ForoContext _context;
        private readonly UserManager<Persona> _userManager;

        public PreguntasController(ForoContext context, UserManager<Persona> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Preguntas
        public async Task<IActionResult> Index()
        {
            var foroContext = _context.Preguntas
                .Include(p => p.Entrada)
                .Include(p => p.Miembro);
            return View(await foroContext.ToListAsync());
        }

        // GET: Preguntas/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null || _context.Preguntas == null)
            {
                return NotFound();
            }

            var pregunta = await _context.Preguntas
                .Include(p => p.Entrada)
                .Include(p => p.Miembro)
                .Include(p => p.Respuestas)
                    .ThenInclude(r => r.Miembro)
                .Include(p => p.Respuestas)
                    .ThenInclude(r => r.Reacciones)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pregunta != null && pregunta.Respuestas != null)
            {
                ViewBag.RespuestaConMasMG = pregunta.Respuestas
                    .Where(r => r.Reacciones.Count(re => re.MeGusta == true) > 0)
                .OrderByDescending(r => r.Reacciones.Count(re => re.MeGusta == true))
                .FirstOrDefault();

                ViewBag.RespuestaConMasNoMG = pregunta.Respuestas
                   .Where(r => r.Reacciones.Count(re => re.MeGusta == false) > 0)
                .OrderByDescending(r => r.Reacciones.Count(re => re.MeGusta == false))
                .FirstOrDefault();
            }

            var usuarioActual = _userManager.GetUserAsync(User).Result;
            if (usuarioActual != null && usuarioActual.Id == pregunta.MiembroId)
            {
                ViewData["IsVisible"] = false;
            }
            else
            {
                ViewData["IsVisible"] = true;
            }

            if (pregunta == null)
            {
                return NotFound();
            }

            ViewBag.UsuarioLogueado = usuarioActual;

            return View(pregunta);
        }

        // GET: Preguntas/Create
        [Authorize]
        public IActionResult Create(int entradaId)
        {
            ViewData["EntradaId"] = entradaId;
            return View();
        }

        // POST: Preguntas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int entradaId, [Bind("Id,Descripcion,Activa,MiembroId,EntradaId")] Pregunta pregunta)
        {
            pregunta.Fecha = DateTime.Now;
            if (ModelState.IsValid)
            {
                var usuarioActual = _userManager.GetUserAsync(User).Result;
                pregunta.MiembroId = usuarioActual.Id;
                pregunta.EntradaId = entradaId;
                _context.Add(pregunta);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Entradas", new { id = pregunta.EntradaId });
            }
            ViewData["EntradaId"] = new SelectList(_context.Entradas, "Id", "Titulo", pregunta.EntradaId);
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "Id", "UserName", pregunta.MiembroId);
            return View(pregunta);
        }

        // GET: Preguntas/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Preguntas == null)
            {
                return NotFound();
            }

            var pregunta = await _context.Preguntas.FindAsync(id);
            if (pregunta == null)
            {
                return NotFound();
            }
            ViewData["EntradaId"] = new SelectList(_context.Entradas, "Id", "Titulo", pregunta.EntradaId);
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "Id", "UserName", pregunta.MiembroId);
            return View(pregunta);
        }

        // POST: Preguntas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descripcion,Fecha,Activa,MiembroId,EntradaId")] Pregunta pregunta)
        {
            if (id != pregunta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pregunta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PreguntaExists(pregunta.Id))
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
            ViewData["EntradaId"] = new SelectList(_context.Entradas, "Id", "Titulo", pregunta.EntradaId);
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "Id", "UserName", pregunta.MiembroId);
            return View(pregunta);
        }

        // GET: Preguntas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Preguntas == null)
            {
                return NotFound();
            }

            var pregunta = await _context.Preguntas
                .Include(p => p.Entrada)
                .Include(p => p.Miembro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pregunta == null)
            {
                return NotFound();
            }

            return View(pregunta);
        }

        // POST: Preguntas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Preguntas == null)
            {
                return Problem("Entity set 'ForoContext.Preguntas'  is null.");
            }
            var pregunta = await _context.Preguntas.FindAsync(id);
            if (pregunta != null)
            {
                _context.Preguntas.Remove(pregunta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PreguntaExists(int id)
        {
            return _context.Preguntas.Any(e => e.Id == id);
        }
    }
}
