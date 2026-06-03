using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorldCupStickers.Data;
using WorldCupStickers.Models;
using WorldCupStickers.ViewModels;

namespace WorldCupStickers.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var modelo = new DashboardViewModel
        {
            TotalPaises = await _context.Paises.CountAsync(),
            TotalEquipos = await _context.Equipos.CountAsync(),
            TotalJugadores = await _context.Jugadores.CountAsync(),
            TotalCromos = await _context.Cromos.CountAsync(),
            TotalAlbumes = await _context.Albumes.CountAsync(),
            UltimosCromos = await _context.Cromos
                .Include(c => c.Jugador)
                .Include(c => c.Equipo)
                    .ThenInclude(e => e!.Pais)
                .Include(c => c.Album)
                .OrderByDescending(c => c.Id)
                .Take(8)
                .ToListAsync(),
            EquiposDestacados = await _context.Equipos
                .Include(e => e.Pais)
                .Include(e => e.Jugadores)
                .Include(e => e.Cromos)
                .OrderByDescending(e => e.Cromos.Count)
                .Take(3)
                .ToListAsync()
        };

        return View(modelo);
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
