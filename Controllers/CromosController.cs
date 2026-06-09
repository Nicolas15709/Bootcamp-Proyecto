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
    private readonly ITheSportsDbService _sportsDb;

    public CromosController(ApplicationDbContext context, IFileUploadService fileUpload, ITheSportsDbService sportsDb)
    {
        _context = context;
        _fileUpload = fileUpload;
        _sportsDb = sportsDb;
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

    // ──────────────────────────────────────────────
    // INTEGRACIÓN THESPORTSDB
    // ──────────────────────────────────────────────

    /// <summary>
    /// GET /Cromos/BuscarFotoApi?nombre=Messi
    /// Devuelve JSON con la foto y datos del jugador encontrado en TheSportsDB.
    /// Usado por el botón "Buscar foto en API" en los formularios.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> BuscarFotoApi(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            return Json(new { ok = false, mensaje = "Nombre vacío." });

        var jugador = await _sportsDb.BuscarJugadorAsync(nombre);

        if (jugador == null || string.IsNullOrEmpty(jugador.FotoUrl))
            return Json(new { ok = false, mensaje = $"No se encontró foto para '{nombre}' en TheSportsDB." });

        return Json(new
        {
            ok = true,
            fotoUrl = jugador.FotoUrl,
            nombre = jugador.Nombre,
            equipo = jugador.Equipo,
            posicion = jugador.Posicion
        });
    }

    /// <summary>
    /// POST /Cromos/ActualizarFotosBulk
    /// Actualiza las fotos de todos los cromos que aún tienen una URL de placeholder
    /// consultando TheSportsDB por el nombre del jugador asociado.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ActualizarFotosBulk()
    {
        var cromos = await _context.Cromos
            .Include(c => c.Jugador)
            .Where(c => c.FotoUrl == null || c.FotoUrl.Contains("placehold.co"))
            .ToListAsync();

        if (!cromos.Any())
        {
            TempData["Success"] = "Todos los cromos ya tienen foto real. No hay nada que actualizar.";
            return RedirectToAction(nameof(Index));
        }

        int actualizados = 0;
        int noEncontrados = 0;

        foreach (var cromo in cromos)
        {
            if (cromo.Jugador == null) continue;

            var foto = await _sportsDb.BuscarFotoAsync(cromo.Jugador.Nombre);
            if (!string.IsNullOrEmpty(foto))
            {
                cromo.FotoUrl = foto;
                actualizados++;
            }
            else
            {
                noEncontrados++;
            }

            // Pequeña pausa para no saturar la API gratuita
            await Task.Delay(350);
        }

        await _context.SaveChangesAsync();

        TempData["Success"] = $"Fotos actualizadas: {actualizados}. " +
                              (noEncontrados > 0 ? $"No encontrados en API: {noEncontrados}." : "");
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
