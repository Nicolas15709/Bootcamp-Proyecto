using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorldCupStickers.Data;
using WorldCupStickers.Models;
using WorldCupStickers.Services;
using WorldCupStickers.ViewModels;

namespace WorldCupStickers.Controllers;

public class CromosController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IFileUploadService _fileUpload;

    public CromosController(ApplicationDbContext context, IFileUploadService fileUpload)
    {
        _context = context;
        _fileUpload = fileUpload;
    }

    // GET: Cromos
    public async Task<IActionResult> Index(int? numeroCromo, string? jugador, int? equipoId, int? paisId, int? albumId, string? edicion)
    {
        var query = _context.Cromos
            .Include(c => c.Jugador)
            .Include(c => c.Equipo)
                .ThenInclude(e => e!.Pais)
            .Include(c => c.Album)
            .AsQueryable();

        if (numeroCromo.HasValue)
            query = query.Where(c => c.NumeroCromo == numeroCromo.Value);

        if (!string.IsNullOrWhiteSpace(jugador))
            query = query.Where(c => c.Jugador!.Nombre.Contains(jugador));

        if (equipoId.HasValue)
            query = query.Where(c => c.EquipoId == equipoId.Value);

        if (paisId.HasValue)
            query = query.Where(c => c.Equipo!.PaisId == paisId.Value);

        if (albumId.HasValue)
            query = query.Where(c => c.AlbumId == albumId.Value);

        if (!string.IsNullOrWhiteSpace(edicion))
            query = query.Where(c => c.Edicion == edicion);

        var modelo = new CromoFiltroViewModel
        {
            NumeroCromo = numeroCromo,
            Jugador = jugador,
            EquipoId = equipoId,
            PaisId = paisId,
            AlbumId = albumId,
            Edicion = edicion,
            Cromos = await query.OrderBy(c => c.NumeroCromo).ToListAsync(),
            Equipos = new SelectList(await _context.Equipos.OrderBy(e => e.Nombre).ToListAsync(), "Id", "Nombre", equipoId),
            Paises = new SelectList(await _context.Paises.OrderBy(p => p.Nombre).ToListAsync(), "Id", "Nombre", paisId),
            Albumes = new SelectList(await _context.Albumes.OrderBy(a => a.Nombre).ToListAsync(), "Id", "Nombre", albumId),
            Ediciones = new SelectList(await _context.Cromos.Select(c => c.Edicion).Distinct().OrderBy(e => e).ToListAsync(), edicion)
        };

        return View(modelo);
    }

    // GET: Cromos/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var cromo = await _context.Cromos
            .Include(c => c.Jugador)
            .Include(c => c.Equipo)
                .ThenInclude(e => e!.Pais)
            .Include(c => c.Album)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cromo == null)
            return NotFound();

        return View(cromo);
    }

    // GET: Cromos/Create
    public async Task<IActionResult> Create()
    {
        await PoblarListasAsync();
        return View();
    }

    // POST: Cromos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("NumeroCromo,Edicion,ValorMercado,FotoUrl,JugadorId,EquipoId,AlbumId")] Cromo cromo,
        IFormFile? fotoFile)
    {
        await ProcesarFotoAsync(cromo, fotoFile);

        if (await _context.Cromos.AnyAsync(c => c.NumeroCromo == cromo.NumeroCromo))
            ModelState.AddModelError(nameof(Cromo.NumeroCromo), "Ya existe un cromo con ese número.");

        if (ModelState.IsValid)
        {
            _context.Add(cromo);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Cromo #{cromo.NumeroCromo} creado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        await PoblarListasAsync(cromo);
        return View(cromo);
    }

    // GET: Cromos/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var cromo = await _context.Cromos.FindAsync(id);
        if (cromo == null)
            return NotFound();

        await PoblarListasAsync(cromo);
        return View(cromo);
    }

    // POST: Cromos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,NumeroCromo,Edicion,ValorMercado,FotoUrl,JugadorId,EquipoId,AlbumId")] Cromo cromo,
        IFormFile? fotoFile)
    {
        if (id != cromo.Id)
            return NotFound();

        await ProcesarFotoAsync(cromo, fotoFile);

        if (await _context.Cromos.AnyAsync(c => c.NumeroCromo == cromo.NumeroCromo && c.Id != cromo.Id))
            ModelState.AddModelError(nameof(Cromo.NumeroCromo), "Ya existe otro cromo con ese número.");

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(cromo);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Cromo #{cromo.NumeroCromo} actualizado correctamente.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Cromos.AnyAsync(c => c.Id == cromo.Id))
                    return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        await PoblarListasAsync(cromo);
        return View(cromo);
    }

    // GET: Cromos/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var cromo = await _context.Cromos
            .Include(c => c.Jugador)
            .Include(c => c.Equipo)
            .Include(c => c.Album)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cromo == null)
            return NotFound();

        return View(cromo);
    }

    // POST: Cromos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var cromo = await _context.Cromos.FindAsync(id);
        if (cromo == null)
            return NotFound();

        _fileUpload.DeleteImage(cromo.FotoUrl);
        _context.Cromos.Remove(cromo);
        await _context.SaveChangesAsync();
        TempData["Success"] = $"Cromo #{cromo.NumeroCromo} eliminado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    private async Task ProcesarFotoAsync(Cromo cromo, IFormFile? fotoFile)
    {
        if (fotoFile is not null && fotoFile.Length > 0)
        {
            try
            {
                cromo.FotoUrl = await _fileUpload.UploadImageAsync(fotoFile, "cromos");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("fotoFile", ex.Message);
            }
        }
    }

    private async Task PoblarListasAsync(Cromo? cromo = null)
    {
        ViewBag.Jugadores = new SelectList(await _context.Jugadores.OrderBy(j => j.Nombre).ToListAsync(), "Id", "Nombre", cromo?.JugadorId);
        ViewBag.Equipos = new SelectList(await _context.Equipos.OrderBy(e => e.Nombre).ToListAsync(), "Id", "Nombre", cromo?.EquipoId);
        ViewBag.Albumes = new SelectList(await _context.Albumes.OrderBy(a => a.Nombre).ToListAsync(), "Id", "Nombre", cromo?.AlbumId);
    }
}
