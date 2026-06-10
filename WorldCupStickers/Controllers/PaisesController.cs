using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorldCupStickers.Data;
using WorldCupStickers.Models;

namespace WorldCupStickers.Controllers;

public class PaisesController : Controller
{
    private readonly ApplicationDbContext _context;

    public PaisesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Paises
    public async Task<IActionResult> Index(string? search, string? continente)
    {
        var query = _context.Paises
            .Include(p => p.Equipos)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(p => p.Nombre.Contains(search) || p.CodigoFifa.Contains(search));

        if (!string.IsNullOrWhiteSpace(continente))
            query = query.Where(p => p.Continente == continente);

        ViewBag.Search = search;
        ViewBag.Continente = continente;
        ViewBag.Continentes = await _context.Paises
            .Select(p => p.Continente)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();

        var paises = await query.OrderBy(p => p.RankingFifa).ToListAsync();
        return View(paises);
    }

    // GET: Paises/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var pais = await _context.Paises
            .Include(p => p.Equipos)
                .ThenInclude(e => e.Jugadores)
            .Include(p => p.Equipos)
                .ThenInclude(e => e.Cromos)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pais == null)
            return NotFound();

        ViewBag.TotalJugadores = pais.Equipos.Sum(e => e.Jugadores.Count);
        ViewBag.TotalCromos = pais.Equipos.Sum(e => e.Cromos.Count);

        return View(pais);
    }

}
