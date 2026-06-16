using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorldCupStickers.Data;
using WorldCupStickers.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

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

}
