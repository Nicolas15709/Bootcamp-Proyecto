using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorldCupStickers.Data;

namespace WorldCupStickers.Controllers;

public class LoginController : Controller
{
    private readonly ApplicationDbContext _db;

    public LoginController(ApplicationDbContext db) => _db = db;

    // GET /Login
    [HttpGet]
    public IActionResult Index()
    {
        if (HttpContext.Session.GetInt32("UsuarioId").HasValue)
            return RedirectToAction("Index", "Home");
        return View();
    }

    // POST /Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(string nombreUsuario, string password)
    {
        if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.Error = "Completa todos los campos.";
            return View();
        }

        var usuario = await _db.Usuarios
            .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);

        // Mensaje genérico para no revelar si el usuario existe
        if (usuario == null || !BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash))
        {
            ViewBag.Error = "Usuario o contraseña incorrectos.";
            return View();
        }

        // Guardar datos en sesión
        HttpContext.Session.SetInt32("UsuarioId",       usuario.Id);
        HttpContext.Session.SetString("UsuarioNombre",   usuario.Nombre);
        HttpContext.Session.SetString("UsuarioUsername", usuario.NombreUsuario);

        return RedirectToAction("Index", "Home");
    }

    // GET /Login/Salir
    public IActionResult Salir()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
}
