using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorldCupStickers.Data;
using WorldCupStickers.Models;
using WorldCupStickers.Services;

namespace WorldCupStickers.Controllers;

public class EquiposController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IFileUploadService _fileUpload;

    public EquiposController(ApplicationDbContext context, IFileUploadService fileUpload)
    {
        _context = context;
        _fileUpload = fileUpload;
    }

    // GET: Equipos
    public async Task<IActionResult> Index(string? search, int? paisId)
    {
        var query = _context.Equipos
            .Include(e => e.Pais)
            .Include(e => e.Jugadores)
            .Include(e => e.Cromos)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(e => e.Nombre.Contains(search) || e.DirectorTecnico.Contains(search));

        if (paisId.HasValue)
            query = query.Where(e => e.PaisId == paisId.Value);

        ViewBag.Search = search;
        ViewBag.PaisId = paisId;
        await PoblarPaisesAsync(paisId);

        var equipos = await query.OrderBy(e => e.Nombre).ToListAsync();
        return View(equipos);
    }

    // GET: Equipos/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var equipo = await _context.Equipos
            .Include(e => e.Pais)
            .Include(e => e.Jugadores)
            .Include(e => e.Cromos)
                .ThenInclude(c => c.Jugador)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (equipo == null)
            return NotFound();

        return View(equipo);
    }

    // GET: Equipos/Create
    public async Task<IActionResult> Create()
    {
        await PoblarPaisesAsync();
        return View();
    }

    // POST: Equipos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Nombre,DirectorTecnico,AnioFundacion,LogoUrl,GrupoMundialista,PaisId")] Equipo equipo,
        IFormFile? logoFile)
    {
        await ProcesarLogoAsync(equipo, logoFile);

        if (ModelState.IsValid)
        {
            _context.Add(equipo);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Equipo \"{equipo.Nombre}\" creado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        await PoblarPaisesAsync(equipo.PaisId);
        return View(equipo);
    }

    // GET: Equipos/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var equipo = await _context.Equipos.FindAsync(id);
        if (equipo == null)
            return NotFound();

        await PoblarPaisesAsync(equipo.PaisId);
        return View(equipo);
    }

    // POST: Equipos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,Nombre,DirectorTecnico,AnioFundacion,LogoUrl,GrupoMundialista,PaisId")] Equipo equipo,
        IFormFile? logoFile)
    {
        if (id != equipo.Id)
            return NotFound();

        await ProcesarLogoAsync(equipo, logoFile);

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(equipo);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Equipo \"{equipo.Nombre}\" actualizado correctamente.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Equipos.AnyAsync(e => e.Id == equipo.Id))
                    return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        await PoblarPaisesAsync(equipo.PaisId);
        return View(equipo);
    }

    // GET: Equipos/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var equipo = await _context.Equipos
            .Include(e => e.Pais)
            .Include(e => e.Jugadores)
            .Include(e => e.Cromos)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (equipo == null)
            return NotFound();

        return View(equipo);
    }

    // POST: Equipos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var equipo = await _context.Equipos
            .Include(e => e.Jugadores)
            .Include(e => e.Cromos)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (equipo == null)
            return NotFound();

        if (equipo.Jugadores.Any() || equipo.Cromos.Any())
        {
            TempData["Error"] = $"No se puede eliminar \"{equipo.Nombre}\" porque tiene jugadores o cromos asociados.";
            return RedirectToAction(nameof(Index));
        }

        _fileUpload.DeleteImage(equipo.LogoUrl);
        _context.Equipos.Remove(equipo);
        await _context.SaveChangesAsync();
        TempData["Success"] = $"Equipo \"{equipo.Nombre}\" eliminado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    private async Task ProcesarLogoAsync(Equipo equipo, IFormFile? logoFile)
    {
        if (logoFile is not null && logoFile.Length > 0)
        {
            try
            {
                equipo.LogoUrl = await _fileUpload.UploadImageAsync(logoFile, "equipos");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("logoFile", ex.Message);
            }
        }
    }

    private async Task PoblarPaisesAsync(int? seleccionado = null)
    {
        ViewBag.Paises = new SelectList(
            await _context.Paises.OrderBy(p => p.Nombre).ToListAsync(),
            "Id", "Nombre", seleccionado);
    }
}
