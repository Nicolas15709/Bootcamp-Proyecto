using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorldCupStickers.Data;
using WorldCupStickers.Models;
using WorldCupStickers.ViewModels;

namespace WorldCupStickers.Controllers;

public class JugadoresController : Controller
{
    private readonly ApplicationDbContext _context;

    public JugadoresController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Jugadores
    public async Task<IActionResult> Index(string? nombre, int? paisId, int? equipoId, string? posicion)
    {
        var query = _context.Jugadores
            .Include(j => j.Equipo)
                .ThenInclude(e => e!.Pais)
            .Include(j => j.Cromos)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(nombre))
            query = query.Where(j => j.Nombre.Contains(nombre));

        if (paisId.HasValue)
            query = query.Where(j => j.Equipo!.PaisId == paisId.Value);

        if (equipoId.HasValue)
            query = query.Where(j => j.EquipoId == equipoId.Value);

        if (!string.IsNullOrWhiteSpace(posicion))
            query = query.Where(j => j.Posicion == posicion);

        var modelo = new JugadorFiltroViewModel
        {
            Nombre = nombre,
            PaisId = paisId,
            EquipoId = equipoId,
            Posicion = posicion,
            Jugadores = await query.OrderBy(j => j.Nombre).ToListAsync(),
            Paises = new SelectList(await _context.Paises.OrderBy(p => p.Nombre).ToListAsync(), "Id", "Nombre", paisId),
            Equipos = new SelectList(await _context.Equipos.OrderBy(e => e.Nombre).ToListAsync(), "Id", "Nombre", equipoId),
            Posiciones = new SelectList(await _context.Jugadores.Select(j => j.Posicion).Distinct().OrderBy(p => p).ToListAsync(), posicion)
        };

        return View(modelo);
    }

    // GET: Jugadores/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var jugador = await _context.Jugadores
            .Include(j => j.Equipo)
                .ThenInclude(e => e!.Pais)
            .Include(j => j.Cromos)
                .ThenInclude(c => c.Album)
            .FirstOrDefaultAsync(j => j.Id == id);

        if (jugador == null)
            return NotFound();

        return View(jugador);
    }

    // GET: Jugadores/Create
    public async Task<IActionResult> Create()
    {
        await PoblarEquiposAsync();
        return View();
    }

    // POST: Jugadores/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nombre,Posicion,NumeroCamiseta,FechaNacimiento,EquipoId")] Jugador jugador)
    {
        if (ModelState.IsValid)
        {
            _context.Add(jugador);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Jugador \"{jugador.Nombre}\" creado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        await PoblarEquiposAsync(jugador.EquipoId);
        return View(jugador);
    }

    // GET: Jugadores/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var jugador = await _context.Jugadores.FindAsync(id);
        if (jugador == null)
            return NotFound();

        await PoblarEquiposAsync(jugador.EquipoId);
        return View(jugador);
    }

    // POST: Jugadores/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Posicion,NumeroCamiseta,FechaNacimiento,EquipoId")] Jugador jugador)
    {
        if (id != jugador.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(jugador);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Jugador \"{jugador.Nombre}\" actualizado correctamente.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Jugadores.AnyAsync(j => j.Id == jugador.Id))
                    return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        await PoblarEquiposAsync(jugador.EquipoId);
        return View(jugador);
    }

    // GET: Jugadores/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var jugador = await _context.Jugadores
            .Include(j => j.Equipo)
            .Include(j => j.Cromos)
            .FirstOrDefaultAsync(j => j.Id == id);

        if (jugador == null)
            return NotFound();

        return View(jugador);
    }

    // POST: Jugadores/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var jugador = await _context.Jugadores
            .Include(j => j.Cromos)
            .FirstOrDefaultAsync(j => j.Id == id);

        if (jugador == null)
            return NotFound();

        if (jugador.Cromos.Any())
        {
            TempData["Error"] = $"No se puede eliminar \"{jugador.Nombre}\" porque tiene cromos asociados.";
            return RedirectToAction(nameof(Index));
        }

        _context.Jugadores.Remove(jugador);
        await _context.SaveChangesAsync();
        TempData["Success"] = $"Jugador \"{jugador.Nombre}\" eliminado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    private async Task PoblarEquiposAsync(int? seleccionado = null)
    {
        ViewBag.Equipos = new SelectList(
            await _context.Equipos.OrderBy(e => e.Nombre).ToListAsync(),
            "Id", "Nombre", seleccionado);
    }
}
