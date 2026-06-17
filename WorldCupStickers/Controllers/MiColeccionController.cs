using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorldCupStickers.Data;

namespace WorldCupStickers.Controllers;

public class MiColeccionController : Controller
{
    private readonly ApplicationDbContext _db;

    public MiColeccionController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");
        if (usuarioId is null)
            return RedirectToAction("Index", "Login");

        var cromos = await _db.UsuarioCromos
            .Where(uc => uc.UsuarioId == usuarioId)
            .Include(uc => uc.Cromo)
                .ThenInclude(c => c!.Jugador)
            .Include(uc => uc.Cromo)
                .ThenInclude(c => c!.Equipo)
            .OrderBy(uc => uc.Cromo!.NumeroCromo)
            .ToListAsync();

        // Cuenta aqui cuántas veces tiene cada cromo
        var conteos = cromos
            .GroupBy(uc => uc.CromoId)
            .ToDictionary(g => g.Key, g => g.Count());

        ViewBag.NombreUsuario = HttpContext.Session.GetString("UsuarioNombre");
        ViewBag.Conteos = conteos;
        return View(cromos);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int cromoId)
    {
        int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");
        if (usuarioId is null)
            return RedirectToAction("Index", "Login");

        var uc = await _db.UsuarioCromos
            .FirstOrDefaultAsync(x => x.UsuarioId == usuarioId && x.CromoId == cromoId);

        if (uc is not null)
        {
            _db.UsuarioCromos.Remove(uc);
            await _db.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> IncrementarCantidad(int cromoId)
    {
        int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");
        if (usuarioId is null)
            return RedirectToAction("Index", "Login");

        var uc = await _db.UsuarioCromos
            .FirstOrDefaultAsync(x => x.UsuarioId == usuarioId && x.CromoId == cromoId);

        if (uc is not null)
        {
            uc.Cantidad++;
            await _db.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }
}