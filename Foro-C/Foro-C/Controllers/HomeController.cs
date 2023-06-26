using Foro_C.Data;
using Foro_C.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Foro_C.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ForoContext _context;

        public HomeController(ForoContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ActionResult> Index()
        {
            var categorias = await _context.Categorias
                .Include(c => c.Entradas)
                .OrderBy(c => c.Nombre)
                .ToListAsync();

            var top5Entradas = await _context.Entradas.Include(e => e.Categoria).Include(e => e.Miembro)
                .Where(e => !e.Privada)
                .OrderByDescending(e => e.Fecha)
                .Take(5)
                .ToListAsync();

            var top5EntradasConMasPreguntas = await _context.Entradas
                .Include(e => e.Preguntas)
                .OrderByDescending(e => e.Preguntas.Count)
                .Take(5)
                .ToListAsync();

            DateTime ultimoMes = DateTime.Now.AddMonths(-1);

            var top3Miembros = await _context.Entradas
                .Where(e => e.Fecha >= ultimoMes)
                .GroupBy(e => e.MiembroId)
                .Select(g => new {
                    UserName = g.First().Miembro.UserName,
                    CantidadEntradas = g.Count()
                })
                .OrderByDescending(e => e.CantidadEntradas)
                .Take(3)
                .ToListAsync();

            ViewBag.Top3Miembros = top3Miembros.Select(m => m).ToList();
            ViewBag.Top5EntradasConMasPreguntas = top5EntradasConMasPreguntas;
            ViewBag.Categorias = categorias;
            ViewBag.Top5Entradas = top5Entradas;

            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}