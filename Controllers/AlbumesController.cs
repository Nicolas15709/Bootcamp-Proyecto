using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorldCupStickers.Data;
using WorldCupStickers.Models;

namespace WorldCupStickers.Controllers;

public class AlbumesController : Controller
{
    private readonly ApplicationDbContext _context;

    public AlbumesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Albumes
    public async Task<IActionResult> Index(string? search)
    {
        var query = _context.Albumes
            .Include(a => a.Cromos)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(a => a.Nombre.Contains(search));

        ViewBag.Search = search;
        var albumes = await query.OrderByDescending(a => a.Anio).ToListAsync();
        return View(albumes);
    }

    // GET: Albumes/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var album = await _context.Albumes
            .Include(a => a.Cromos)
                .ThenInclude(c => c.Jugador)
            .Include(a => a.Cromos)
                .ThenInclude(c => c.Equipo)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (album == null)
            return NotFound();

        return View(album);
    }

    // GET: Albumes/Create
    public IActionResult Create() => View();

    // POST: Albumes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nombre,Anio,CantidadCromos,EdicionEspecial")] Album album)
    {
        if (ModelState.IsValid)
        {
            _context.Add(album);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Álbum \"{album.Nombre}\" creado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        return View(album);
    }

    // GET: Albumes/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var album = await _context.Albumes.FindAsync(id);
        if (album == null)
            return NotFound();

        return View(album);
    }

    // POST: Albumes/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Anio,CantidadCromos,EdicionEspecial")] Album album)
    {
        if (id != album.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(album);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Álbum \"{album.Nombre}\" actualizado correctamente.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Albumes.AnyAsync(a => a.Id == album.Id))
                    return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(album);
    }

    // GET: Albumes/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var album = await _context.Albumes
            .Include(a => a.Cromos)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (album == null)
            return NotFound();

        return View(album);
    }

    // POST: Albumes/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var album = await _context.Albumes
            .Include(a => a.Cromos)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (album == null)
            return NotFound();

        if (album.Cromos.Any())
        {
            TempData["Error"] = $"No se puede eliminar \"{album.Nombre}\" porque tiene cromos asociados.";
            return RedirectToAction(nameof(Index));
        }

        _context.Albumes.Remove(album);
        await _context.SaveChangesAsync();
        TempData["Success"] = $"Álbum \"{album.Nombre}\" eliminado correctamente.";
        return RedirectToAction(nameof(Index));
    }
}
