using Microsoft.EntityFrameworkCore;
using WorldCupStickers.Models;
using WorldCupStickers.Services;

namespace WorldCupStickers.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(ApplicationDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.Paises.AnyAsync())
            return; // La base ya tiene datos sembrados.

        // ---------------- PAÍSES (8) ----------------
        var paises = new List<Pais>
        {
            new() { Nombre = "Argentina",     Continente = "América",  CodigoFifa = "ARG", RankingFifa = 1 },
            new() { Nombre = "Brasil",        Continente = "América",  CodigoFifa = "BRA", RankingFifa = 3 },
            new() { Nombre = "Francia",       Continente = "Europa",   CodigoFifa = "FRA", RankingFifa = 2 },
            new() { Nombre = "España",        Continente = "Europa",   CodigoFifa = "ESP", RankingFifa = 8 },
            new() { Nombre = "Inglaterra",    Continente = "Europa",   CodigoFifa = "ENG", RankingFifa = 5 },
            new() { Nombre = "Portugal",      Continente = "Europa",   CodigoFifa = "POR", RankingFifa = 6 },
            new() { Nombre = "Alemania",      Continente = "Europa",   CodigoFifa = "GER", RankingFifa = 16 },
            new() { Nombre = "Países Bajos",  Continente = "Europa",   CodigoFifa = "NED", RankingFifa = 7 }
        };
        context.Paises.AddRange(paises);
        await context.SaveChangesAsync();

        int P(string codigo) => paises.First(p => p.CodigoFifa == codigo).Id;

        // ---------------- EQUIPOS (8 selecciones nacionales) ----------------
        var equipos = new List<Equipo>
        {
            new() { Nombre = "Selección Argentina",     DirectorTecnico = "Lionel Scaloni",   AnioFundacion = 1893, GrupoMundialista = "C", PaisId = P("ARG"), LogoUrl = "https://flagcdn.com/w320/ar.png" },
            new() { Nombre = "Selección Brasil",        DirectorTecnico = "Dorival Júnior",   AnioFundacion = 1914, GrupoMundialista = "G", PaisId = P("BRA"), LogoUrl = "https://flagcdn.com/w320/br.png" },
            new() { Nombre = "Selección Francia",       DirectorTecnico = "Didier Deschamps", AnioFundacion = 1904, GrupoMundialista = "D", PaisId = P("FRA"), LogoUrl = "https://flagcdn.com/w320/fr.png" },
            new() { Nombre = "Selección España",        DirectorTecnico = "Luis de la Fuente",AnioFundacion = 1909, GrupoMundialista = "E", PaisId = P("ESP"), LogoUrl = "https://flagcdn.com/w320/es.png" },
            new() { Nombre = "Selección Inglaterra",    DirectorTecnico = "Gareth Southgate", AnioFundacion = 1863, GrupoMundialista = "B", PaisId = P("ENG"), LogoUrl = "https://flagcdn.com/w320/gb-eng.png" },
            new() { Nombre = "Selección Portugal",      DirectorTecnico = "Roberto Martínez", AnioFundacion = 1914, GrupoMundialista = "H", PaisId = P("POR"), LogoUrl = "https://flagcdn.com/w320/pt.png" },
            new() { Nombre = "Selección Alemania",      DirectorTecnico = "Julian Nagelsmann",AnioFundacion = 1900, GrupoMundialista = "E", PaisId = P("GER"), LogoUrl = "https://flagcdn.com/w320/de.png" },
            new() { Nombre = "Selección Países Bajos",  DirectorTecnico = "Ronald Koeman",    AnioFundacion = 1889, GrupoMundialista = "A", PaisId = P("NED"), LogoUrl = "https://flagcdn.com/w320/nl.png" }
        };
        context.Equipos.AddRange(equipos);
        await context.SaveChangesAsync();

        int E(string nombreContiene) => equipos.First(e => e.Nombre.Contains(nombreContiene)).Id;

        // ---------------- ÁLBUM (1) ----------------
        var album = new Album
        {
            Nombre = "Mundial Qatar 2022",
            Anio = 2022,
            CantidadCromos = 670,
            EdicionEspecial = true
        };
        context.Albumes.Add(album);
        await context.SaveChangesAsync();

        // ---------------- JUGADORES (20) ----------------
        var jugadores = new List<Jugador>
        {
            new() { Nombre = "Lionel Messi",        Posicion = "Delantero",     NumeroCamiseta = 10, FechaNacimiento = new DateTime(1987, 6, 24),  EquipoId = E("Argentina") },
            new() { Nombre = "Emiliano Martínez",   Posicion = "Portero",       NumeroCamiseta = 23, FechaNacimiento = new DateTime(1992, 9, 2),   EquipoId = E("Argentina") },
            new() { Nombre = "Julián Álvarez",      Posicion = "Delantero",     NumeroCamiseta = 9,  FechaNacimiento = new DateTime(2000, 1, 31),  EquipoId = E("Argentina") },
            new() { Nombre = "Vinícius Júnior",     Posicion = "Extremo",       NumeroCamiseta = 7,  FechaNacimiento = new DateTime(2000, 7, 12),  EquipoId = E("Brasil") },
            new() { Nombre = "Rodrygo",             Posicion = "Extremo",       NumeroCamiseta = 10, FechaNacimiento = new DateTime(2001, 1, 9),   EquipoId = E("Brasil") },
            new() { Nombre = "Casemiro",            Posicion = "Mediocampista", NumeroCamiseta = 5,  FechaNacimiento = new DateTime(1992, 2, 23),  EquipoId = E("Brasil") },
            new() { Nombre = "Kylian Mbappé",       Posicion = "Delantero",     NumeroCamiseta = 10, FechaNacimiento = new DateTime(1998, 12, 20), EquipoId = E("Francia") },
            new() { Nombre = "Antoine Griezmann",   Posicion = "Mediocampista", NumeroCamiseta = 7,  FechaNacimiento = new DateTime(1991, 3, 21),  EquipoId = E("Francia") },
            new() { Nombre = "Aurélien Tchouaméni", Posicion = "Mediocampista", NumeroCamiseta = 8,  FechaNacimiento = new DateTime(2000, 1, 27),  EquipoId = E("Francia") },
            new() { Nombre = "Rodri",               Posicion = "Mediocampista", NumeroCamiseta = 16, FechaNacimiento = new DateTime(1996, 6, 22),  EquipoId = E("España") },
            new() { Nombre = "Lamine Yamal",        Posicion = "Extremo",       NumeroCamiseta = 19, FechaNacimiento = new DateTime(2007, 7, 13),  EquipoId = E("España") },
            new() { Nombre = "Pedri",               Posicion = "Mediocampista", NumeroCamiseta = 8,  FechaNacimiento = new DateTime(2002, 11, 25), EquipoId = E("España") },
            new() { Nombre = "Harry Kane",          Posicion = "Delantero",     NumeroCamiseta = 9,  FechaNacimiento = new DateTime(1993, 7, 28),  EquipoId = E("Inglaterra") },
            new() { Nombre = "Jude Bellingham",     Posicion = "Mediocampista", NumeroCamiseta = 10, FechaNacimiento = new DateTime(2003, 6, 29),  EquipoId = E("Inglaterra") },
            new() { Nombre = "Cristiano Ronaldo",   Posicion = "Delantero",     NumeroCamiseta = 7,  FechaNacimiento = new DateTime(1985, 2, 5),   EquipoId = E("Portugal") },
            new() { Nombre = "Bruno Fernandes",     Posicion = "Mediocampista", NumeroCamiseta = 8,  FechaNacimiento = new DateTime(1994, 9, 8),   EquipoId = E("Portugal") },
            new() { Nombre = "Rafael Leão",         Posicion = "Extremo",       NumeroCamiseta = 17, FechaNacimiento = new DateTime(1999, 6, 10),  EquipoId = E("Portugal") },
            new() { Nombre = "Jamal Musiala",       Posicion = "Mediocampista", NumeroCamiseta = 10, FechaNacimiento = new DateTime(2003, 2, 26),  EquipoId = E("Alemania") },
            new() { Nombre = "Florian Wirtz",       Posicion = "Mediocampista", NumeroCamiseta = 17, FechaNacimiento = new DateTime(2003, 5, 3),   EquipoId = E("Alemania") },
            new() { Nombre = "Virgil van Dijk",     Posicion = "Defensa",       NumeroCamiseta = 4,  FechaNacimiento = new DateTime(1991, 7, 8),   EquipoId = E("Países Bajos") }
        };
        context.Jugadores.AddRange(jugadores);
        await context.SaveChangesAsync();

        // ---------------- CROMOS (20, uno por jugador) ----------------
        string Foto(string nombre) =>
            $"https://placehold.co/300x400/0B1F3A/D4AF37/png?text={Uri.EscapeDataString(nombre)}";

        var cromos = new List<Cromo>();
        int numero = 1;
        var valores = new decimal[] { 250, 60, 90, 180, 110, 70, 220, 80, 75, 95, 160, 130, 120, 200, 150, 85, 90, 140, 135, 65 };
        foreach (var j in jugadores)
        {
            cromos.Add(new Cromo
            {
                NumeroCromo = numero,
                Edicion = "Qatar 2022",
                ValorMercado = valores[numero - 1],
                FotoUrl = Foto(j.Nombre),
                JugadorId = j.Id,
                EquipoId = j.EquipoId,
                AlbumId = album.Id
            });
            numero++;
        }
        context.Cromos.AddRange(cromos);
        await context.SaveChangesAsync();

        // ---------------- USUARIOS + COLECCIÓN (relación N:M) ----------------
        var usuarios = new List<Usuario>
        {
            new() { Nombre = "Nicolás López",   Email = "nicolas@cromos.com",
                    NombreUsuario = "nicolaslopez",
                    PasswordHash  = BCrypt.Net.BCrypt.HashPassword("Password1!") },
            new() { Nombre = "Melissa Torres", Email = "melitorres@cromos.com",
                    NombreUsuario = "melissatorres",
                    PasswordHash  = BCrypt.Net.BCrypt.HashPassword("Password2!") }
        };
        context.Usuarios.AddRange(usuarios);
        await context.SaveChangesAsync();

        context.UsuarioCromos.AddRange(
            new UsuarioCromo { UsuarioId = usuarios[0].Id, CromoId = cromos[0].Id, FechaAdquisicion = DateTime.Today.AddDays(-10), Estado = EstadoCromo.Favorito },
            new UsuarioCromo { UsuarioId = usuarios[0].Id, CromoId = cromos[6].Id, FechaAdquisicion = DateTime.Today.AddDays(-5),  Estado = EstadoCromo.Nuevo },
            new UsuarioCromo { UsuarioId = usuarios[0].Id, CromoId = cromos[3].Id, FechaAdquisicion = DateTime.Today.AddDays(-3),  Estado = EstadoCromo.Repetido },
            new UsuarioCromo { UsuarioId = usuarios[1].Id, CromoId = cromos[14].Id, FechaAdquisicion = DateTime.Today.AddDays(-2), Estado = EstadoCromo.Intercambiable }
        );
        await context.SaveChangesAsync();
    }

    // ─────────────────────────────────────────────────────────────────
    // EXPANSIÓN: agrega más jugadores y cromos sin tocar los existentes
    // ─────────────────────────────────────────────────────────────────
    public static async Task ExpandirJugadoresAsync(ApplicationDbContext context, ITheSportsDbService? sportsDb = null)
    {
        // Equipos existentes indexados por fragmento de nombre
        var equipos = await context.Equipos.ToListAsync();
        int E(string frag) => equipos.First(e => e.Nombre.Contains(frag)).Id;

        // Álbum existente
        var albumId = (await context.Albumes.FirstAsync()).Id;

        // Nombres ya existentes (para no duplicar)
        var existentes = new HashSet<string>(
            await context.Jugadores.Select(j => j.Nombre).ToListAsync(),
            StringComparer.OrdinalIgnoreCase);

        // Helper: solo agrega si no existe
        var nuevos = new List<Jugador>();
        void Add(string nombre, string posicion, int num, DateTime nac, string equipo)
        {
            if (!existentes.Contains(nombre))
                nuevos.Add(new() { Nombre = nombre, Posicion = posicion, NumeroCamiseta = num, FechaNacimiento = nac, EquipoId = E(equipo) });
        }

        // ── ARGENTINA ──────────────────────────────────────────
        Add("Ángel Di María",       "Extremo",       11, new(1988,  2, 14), "Argentina");
        Add("Alexis Mac Allister",  "Mediocampista",  7, new(1998, 12, 24), "Argentina");
        Add("Rodrigo De Paul",      "Mediocampista",  7, new(1994,  5, 24), "Argentina");
        Add("Nicolás Otamendi",     "Defensa",       30, new(1988,  2, 12), "Argentina");
        Add("Lisandro Martínez",    "Defensa",        2, new(1998,  1, 18), "Argentina");
        Add("Enzo Fernández",       "Mediocampista", 24, new(2001,  1, 17), "Argentina");
        Add("Nicolás Tagliafico",   "Defensa",        3, new(1992,  8, 31), "Argentina");

        // ── BRASIL ─────────────────────────────────────────────
        Add("Alisson Becker",       "Portero",        1, new(1992, 10,  2), "Brasil");
        Add("Marquinhos",           "Defensa",        4, new(1994,  5, 14), "Brasil");
        Add("Éder Militão",         "Defensa",        3, new(1998,  1, 18), "Brasil");
        Add("Lucas Paquetá",        "Mediocampista", 10, new(1997,  8, 27), "Brasil");
        Add("Raphinha",             "Extremo",        26, new(1996, 12, 14), "Brasil");
        Add("Gabriel Martinelli",   "Extremo",        11, new(2001,  6, 18), "Brasil");
        Add("Gabriel Jesus",        "Delantero",       9, new(1997,  4,  3), "Brasil");

        // ── FRANCIA ────────────────────────────────────────────
        Add("Hugo Lloris",          "Portero",         1, new(1986,  12, 26), "Francia");
        Add("Raphaël Varane",       "Defensa",         4, new(1993,  4, 25), "Francia");
        Add("Dayot Upamecano",      "Defensa",         5, new(1998, 10, 27), "Francia");
        Add("Adrien Rabiot",        "Mediocampista",  14, new(1995,  4,  3), "Francia");
        Add("Ousmane Dembélé",      "Extremo",        11, new(1997,  5, 15), "Francia");
        Add("Marcus Thuram",        "Delantero",        9, new(1997,  8,  6), "Francia");
        Add("Kingsley Coman",       "Extremo",        10, new(1996,  6, 13), "Francia");

        // ── ESPAÑA ─────────────────────────────────────────────
        Add("Unai Simón",           "Portero",         1, new(1997,  6,  11), "España");
        Add("Dani Carvajal",        "Defensa",         2, new(1992,  1, 11), "España");
        Add("Aymeric Laporte",      "Defensa",        14, new(1994,  5, 27), "España");
        Add("Fabián Ruiz",          "Mediocampista",  26, new(1996,  4,  3), "España");
        Add("Dani Olmo",            "Mediocampista",  10, new(1998,  5,  7), "España");
        Add("Ferran Torres",        "Extremo",        11, new(2000, 2, 29), "España");
        Add("Álvaro Morata",        "Delantero",        7, new(1992,  10, 23), "España");

        // ── INGLATERRA ─────────────────────────────────────────
        Add("Jordan Pickford",      "Portero",         1, new(1994,  3,  7), "Inglaterra");
        Add("Kyle Walker",          "Defensa",         2, new(1990,  5, 28), "Inglaterra");
        Add("John Stones",          "Defensa",         5, new(1994,  5, 28), "Inglaterra");
        Add("Declan Rice",          "Mediocampista",   4, new(1999,  1, 14), "Inglaterra");
        Add("Bukayo Saka",          "Extremo",         7, new(2001,  9,  5), "Inglaterra");
        Add("Phil Foden",           "Mediocampista",  47, new(2000,  5, 28), "Inglaterra");
        Add("Marcus Rashford",      "Extremo",        10, new(1997, 10, 31), "Inglaterra");

        // ── PORTUGAL ───────────────────────────────────────────
        Add("Rui Patrício",         "Portero",         1, new(1988,  2, 15), "Portugal");
        Add("Rúben Dias",           "Defensa",         3, new(1997,  5, 14), "Portugal");
        Add("João Cancelo",         "Defensa",         5, new(1994,  5, 27), "Portugal");
        Add("Bernardo Silva",       "Mediocampista",  10, new(1994,  8, 10), "Portugal");
        Add("João Félix",           "Delantero",       7, new(1999, 11, 10), "Portugal");
        Add("Gonçalo Ramos",        "Delantero",       9, new(2001,  6, 20), "Portugal");
        Add("Pepe",                 "Defensa",         3, new(1983,  2, 26), "Portugal");

        // ── ALEMANIA ───────────────────────────────────────────
        Add("Manuel Neuer",         "Portero",         1, new(1986,  3, 27), "Alemania");
        Add("Antonio Rüdiger",      "Defensa",         2, new(1993,  3,  3), "Alemania");
        Add("Joshua Kimmich",       "Mediocampista",   6, new(1995,  2,  8), "Alemania");
        Add("Leroy Sané",           "Extremo",        10, new(1996,  1, 11), "Alemania");
        Add("Thomas Müller",        "Mediocampista",  25, new(1989,  9, 13), "Alemania");
        Add("Kai Havertz",          "Delantero",        7, new(1999,  6, 11), "Alemania");
        Add("Serge Gnabry",         "Extremo",        22, new(1995,  7, 14), "Alemania");

        // ── PAÍSES BAJOS ───────────────────────────────────────
        Add("Jasper Cillessen",     "Portero",         1, new(1989,  4, 22), "Países Bajos");
        Add("Matthijs de Ligt",     "Defensa",         3, new(1999,  8, 12), "Países Bajos");
        Add("Nathan Aké",           "Defensa",         6, new(1995,  2, 18), "Países Bajos");
        Add("Frenkie de Jong",      "Mediocampista",   8, new(1997,  5, 12), "Países Bajos");
        Add("Xavi Simons",          "Mediocampista",  26, new(2003,  4, 21), "Países Bajos");
        Add("Cody Gakpo",           "Extremo",        11, new(1999,  5,  7), "Países Bajos");
        Add("Memphis Depay",        "Delantero",       10, new(1994,  2, 13), "Países Bajos");

        if (!nuevos.Any()) return; // nada que agregar

        context.Jugadores.AddRange(nuevos);
        await context.SaveChangesAsync();

        // Número de cromo siguiente al último existente
        int siguienteNum = (await context.Cromos.MaxAsync(c => (int?)c.NumeroCromo) ?? 0) + 1;

        string Placeholder(string nombre) =>
            $"https://placehold.co/300x400/0B1F3A/D4AF37/png?text={Uri.EscapeDataString(nombre)}";

        var valoresBase = new decimal[]
        { 75, 80, 85, 90, 95, 100, 105, 110, 115, 120, 125, 130, 135, 140 };
        int idx = 0;

        // Crear cromos — si hay servicio de API, buscar foto real directamente
        var cromos = new List<Cromo>();
        foreach (var j in nuevos)
        {
            string? fotoUrl = null;

            if (sportsDb != null)
            {
                fotoUrl = await sportsDb.BuscarFotoAsync(j.Nombre);
                await Task.Delay(350); // respetar rate-limit de la API gratuita
            }

            cromos.Add(new Cromo
            {
                NumeroCromo  = siguienteNum++,
                Edicion      = "Qatar 2022",
                ValorMercado = valoresBase[idx++ % valoresBase.Length],
                FotoUrl      = fotoUrl ?? Placeholder(j.Nombre),
                JugadorId    = j.Id,
                EquipoId     = j.EquipoId,
                AlbumId      = albumId
            });
        }

        context.Cromos.AddRange(cromos);
        await context.SaveChangesAsync();
    }
}
