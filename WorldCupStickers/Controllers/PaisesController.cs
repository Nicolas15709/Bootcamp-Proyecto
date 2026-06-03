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

    // GET: Paises/Create
    public IActionResult Create() => View();

    // POST: Paises/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nombre,Continente,CodigoFifa,RankingFifa")] Pais pais)
    {
        pais.CodigoFifa = pais.CodigoFifa?.ToUpperInvariant() ?? string.Empty;

        if (await _context.Paises.AnyAsync(p => p.CodigoFifa == pais.CodigoFifa))
            ModelState.AddModelError(nameof(Pais.CodigoFifa), "Ya existe un país con ese código FIFA.");

        if (ModelState.IsValid)
        {
            _context.Add(pais);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"País \"{pais.Nombre}\" creado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        return View(pais);
    }

    // GET: Paises/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var pais = await _context.Paises.FindAsync(id);
        if (pais == null)
            return NotFound();

        return View(pais);
    }

    // POST: Paises/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Continente,CodigoFifa,RankingFifa")] Pais pais)
    {
        if (id != pais.Id)
            return NotFound();

        pais.CodigoFifa = pais.CodigoFifa?.ToUpperInvariant() ?? string.Empty;

        if (await _context.Paises.AnyAsync(p => p.CodigoFifa == pais.CodigoFifa && p.Id != pais.Id))
            ModelState.AddModelError(nameof(Pais.CodigoFifa), "Ya existe otro país con ese código FIFA.");

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(pais);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"País \"{pais.Nombre}\" actualizado correctamente.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Paises.AnyAsync(p => p.Id == pais.Id))
                    return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(pais);
    }

    // GET: Paises/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var pais = await _context.Paises
            .Include(p => p.Equipos)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pais == null)
            return NotFound();

        return View(pais);
    }

    // POST: Paises/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var pais = await _context.Paises
            .Include(p => p.Equipos)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pais == null)
            return NotFound();

        if (pais.Equipos.Any())
        {
            TempData["Error"] = $"No se puede eliminar \"{pais.Nombre}\" porque tiene equipos asociados.";
            return RedirectToAction(nameof(Index));
        }

        _context.Paises.Remove(pais);
        await _context.SaveChangesAsync();
        TempData["Success"] = $"País \"{pais.Nombre}\" eliminado correctamente.";
        return RedirectToAction(nameof(Index));
    }
}
