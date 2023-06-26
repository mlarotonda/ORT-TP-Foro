using Foro_C.Data;
using Foro_C.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Foro_C.Controllers
{

    public class RespuestasController : Controller
    {
        private readonly ForoContext _context;
        private readonly UserManager<Persona> _userManager;

        public RespuestasController(ForoContext context, UserManager<Persona> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Respuestas
        public async Task<IActionResult> Index()
        {
            var foroContext = _context.Respuestas.Include(r => r.Miembro).Include(r => r.Pregunta);
            return View(await foroContext.ToListAsync());
        }

        // GET: Respuestas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Respuestas == null)
            {
                return NotFound();
            }

            var respuesta = await _context.Respuestas
                .Include(r => r.Miembro)
                .Include(r => r.Pregunta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (respuesta == null)
            {
                return NotFound();
            }

            return View(respuesta);
        }

        // GET: Respuestas/Create
        [Authorize]
        public IActionResult Create(int preguntaId)
        {
            var pregunta = _context.Preguntas.FirstOrDefault(p => p.Id == preguntaId);

            if (pregunta.Activa == false)
            {
                return RedirectToAction("AccesoDenegado", "Account");
            }

            ViewData["PreguntaId"] = preguntaId;
            return View();
        }

        // POST: Respuestas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int preguntaId, [Bind("Id,Descripcion,MiembroId,PreguntaId")] Respuesta respuesta)
        {
            respuesta.Fecha = DateTime.Now;
            if (ModelState.IsValid)
            {
                var usuarioActual = _userManager.GetUserAsync(User).Result;
                respuesta.MiembroId = usuarioActual.Id;
                respuesta.PreguntaId = preguntaId;
                _context.Add(respuesta);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Preguntas", new { id = preguntaId });
            }
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "Id", "UserName", respuesta.MiembroId);
            ViewData["PreguntaId"] = new SelectList(_context.Preguntas, "Id", "Descripcion", respuesta.PreguntaId);
            return View(respuesta);
        }

        // GET: Respuestas/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Respuestas == null)
            {
                return NotFound();
            }

            var respuesta = await _context.Respuestas.FindAsync(id);
            if (respuesta == null)
            {
                return NotFound();
            }
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "Id", "UserName", respuesta.MiembroId);
            ViewData["PreguntaId"] = new SelectList(_context.Preguntas, "Id", "Descripcion", respuesta.PreguntaId);
            return View(respuesta);
        }

        // POST: Respuestas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descripcion,Fecha,MiembroId,PreguntaId")] Respuesta respuesta)
        {
            if (id != respuesta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(respuesta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RespuestaExists(respuesta.Id))
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
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "Id", "UserName", respuesta.MiembroId);
            ViewData["PreguntaId"] = new SelectList(_context.Preguntas, "Id", "Descripcion", respuesta.PreguntaId);
            return View(respuesta);
        }

        // GET: Respuestas/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Respuestas == null)
            {
                return NotFound();
            }

            var respuesta = await _context.Respuestas
                .Include(r => r.Miembro)
                .Include(r => r.Pregunta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (respuesta == null)
            {
                return NotFound();
            }

            return View(respuesta);
        }

        // POST: Respuestas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Respuestas == null)
            {
                return Problem("Entity set 'ForoContext.Respuestas'  is null.");
            }
            var respuesta = await _context.Respuestas.FindAsync(id);
            if (respuesta != null)
            {
                _context.Respuestas.Remove(respuesta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RespuestaExists(int id)
        {
            return _context.Respuestas.Any(e => e.Id == id);
        }
    }
}
