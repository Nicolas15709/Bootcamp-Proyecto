using Microsoft.EntityFrameworkCore;
using WorldCupStickers.Models;
using WorldCupStickers.Services;

namespace WorldCupStickers.Data;

public static class DbInitializer
{
    private static readonly Dictionary<string, string[]> JugadoresPorPais = new()
    {
        ["ARG"] = new[] { "Lionel Messi", "Emiliano Martinez", "Julian Alvarez", "Alexis Mac Allister", "Rodrigo De Paul" },
        ["BRA"] = new[] { "Vinicius Junior", "Alisson", "Marquinhos", "Casemiro", "Neymar Jr" },
        ["FRA"] = new[] { "Kylian Mbappe", "Antoine Griezmann", "Aurelien Tchouameni", "Raphael Varane", "Ousmane Dembele" },
        ["ESP"] = new[] { "Rodri", "Lamine Yamal", "Pedri", "Dani Olmo", "Alvaro Morata" },
        ["ENG"] = new[] { "Harry Kane", "Jude Bellingham", "Bukayo Saka", "Phil Foden", "Jordan Pickford" },
        ["POR"] = new[] { "Cristiano Ronaldo", "Bruno Fernandes", "Rafael Leao", "Ruben Dias", "Bernardo Silva" },
        ["GER"] = new[] { "Jamal Musiala", "Florian Wirtz", "Manuel Neuer", "Joshua Kimmich", "Kai Havertz" },
        ["NED"] = new[] { "Virgil van Dijk", "Frenkie de Jong", "Cody Gakpo", "Memphis Depay", "Xavi Simons" },
        ["ECU"] = new[] { "Enner Valencia", "Moises Caicedo", "Pervis Estupinian", "Hernan Galindez", "Piero Hincapie" },
        ["URU"] = new[] { "Darwin Nunez", "Federico Valverde", "Ronald Araujo", "Jose Maria Gimenez", "Manuel Ugarte" },
        ["MEX"] = new[] { "Santiago Gimenez", "Raul Jimenez", "Guillermo Ochoa", "Edson Alvarez", "Alexis Vega" },
        ["COL"] = new[] { "James Rodriguez", "Luis Diaz", "Davinson Sanchez", "Jhon Duran", "Cucho Hernandez" },
        ["JPN"] = new[] { "Takumi Minamino", "Ritsu Doan", "Daichi Kamada", "Maya Yoshida", "Shuichi Gonda" },
        ["BEL"] = new[] { "Kevin De Bruyne", "Romelu Lukaku", "Thibaut Courtois", "Eden Hazard", "Jan Vertonghen" },
        ["CRO"] = new[] { "Luka Modric", "Ivan Perisic", "Marcelo Brozovic", "Dominik Livakovic", "Josko Gvardiol" },
    };

    public static async Task InitializeAsync(ApplicationDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.Paises.AnyAsync())
            return; // La base ya tiene datos sembrados.

        // ---------------- PAÍSES (8) ----------------
        var paises = new List<Pais>
        {
            new() { Nombre = "Argentina",    Continente = "América", CodigoFifa = "ARG", RankingFifa = 1  },
            new() { Nombre = "Brasil",       Continente = "América", CodigoFifa = "BRA", RankingFifa = 3  },
            new() { Nombre = "Francia",      Continente = "Europa",  CodigoFifa = "FRA", RankingFifa = 2  },
            new() { Nombre = "España",       Continente = "Europa",  CodigoFifa = "ESP", RankingFifa = 8  },
            new() { Nombre = "Inglaterra",   Continente = "Europa",  CodigoFifa = "ENG", RankingFifa = 5  },
            new() { Nombre = "Portugal",     Continente = "Europa",  CodigoFifa = "POR", RankingFifa = 6  },
            new() { Nombre = "Alemania",     Continente = "Europa",  CodigoFifa = "GER", RankingFifa = 16 },
            new() { Nombre = "Países Bajos", Continente = "Europa",  CodigoFifa = "NED", RankingFifa = 7  },
            new() { Nombre = "Ecuador",      Continente = "América", CodigoFifa = "ECU", RankingFifa = 44 },
            new() { Nombre = "Uruguay",      Continente = "América", CodigoFifa = "URU", RankingFifa = 17 },
            new() { Nombre = "México",       Continente = "América", CodigoFifa = "MEX", RankingFifa = 15 },
            new() { Nombre = "Colombia",     Continente = "América", CodigoFifa = "COL", RankingFifa = 11 },
            new() { Nombre = "Japón",        Continente = "Asia",    CodigoFifa = "JPN", RankingFifa = 18 },
            new() { Nombre = "Bélgica",      Continente = "Europa",  CodigoFifa = "BEL", RankingFifa = 4  },
            new() { Nombre = "Croacia",      Continente = "Europa",  CodigoFifa = "CRO", RankingFifa = 10 },
        };
        context.Paises.AddRange(paises);
        await context.SaveChangesAsync();

        int P(string codigo) => paises.First(p => p.CodigoFifa == codigo).Id;

        // ---------------- EQUIPOS (8 selecciones nacionales) ----------------
        var equipos = new List<Equipo>
        {
            new() { Nombre = "Selección Argentina",    DirectorTecnico = "Lionel Scaloni",      AnioFundacion = 1893, GrupoMundialista = "B", PaisId = P("ARG"), LogoUrl = "https://flagcdn.com/w320/ar.png" },
            new() { Nombre = "Selección Brasil",       DirectorTecnico = "Carlo Ancelotti",     AnioFundacion = 1914, GrupoMundialista = "G", PaisId = P("BRA"), LogoUrl = "https://flagcdn.com/w320/br.png" },
            new() { Nombre = "Selección Francia",      DirectorTecnico = "Didier Deschamps",    AnioFundacion = 1904, GrupoMundialista = "E", PaisId = P("FRA"), LogoUrl = "https://flagcdn.com/w320/fr.png" },
            new() { Nombre = "Selección España",       DirectorTecnico = "Luis de la Fuente",   AnioFundacion = 1909, GrupoMundialista = "A", PaisId = P("ESP"), LogoUrl = "https://flagcdn.com/w320/es.png" },
            new() { Nombre = "Selección Inglaterra",   DirectorTecnico = "Gareth Southgate",    AnioFundacion = 1863, GrupoMundialista = "C", PaisId = P("ENG"), LogoUrl = "https://flagcdn.com/w320/gb-eng.png" },
            new() { Nombre = "Selección Portugal",     DirectorTecnico = "Roberto Martínez",    AnioFundacion = 1914, GrupoMundialista = "H", PaisId = P("POR"), LogoUrl = "https://flagcdn.com/w320/pt.png" },
            new() { Nombre = "Selección Alemania",     DirectorTecnico = "Julian Nagelsmann",   AnioFundacion = 1900, GrupoMundialista = "F", PaisId = P("GER"), LogoUrl = "https://flagcdn.com/w320/de.png" },
            new() { Nombre = "Selección Países Bajos", DirectorTecnico = "Ronald Koeman",       AnioFundacion = 1889, GrupoMundialista = "D", PaisId = P("NED"), LogoUrl = "https://flagcdn.com/w320/nl.png" },
            new() { Nombre = "Selección Ecuador",      DirectorTecnico = "Sebastián Beccacece", AnioFundacion = 1925, GrupoMundialista = "A", PaisId = P("ECU"), LogoUrl = "https://flagcdn.com/w320/ec.png" },
            new() { Nombre = "Selección Uruguay",      DirectorTecnico = "Marcelo Bielsa",      AnioFundacion = 1900, GrupoMundialista = "H", PaisId = P("URU"), LogoUrl = "https://flagcdn.com/w320/uy.png" },
            new() { Nombre = "Selección México",       DirectorTecnico = "Javier Aguirre",      AnioFundacion = 1927, GrupoMundialista = "A", PaisId = P("MEX"), LogoUrl = "https://flagcdn.com/w320/mx.png" },
            new() { Nombre = "Selección Colombia",     DirectorTecnico = "Néstor Lorenzo",      AnioFundacion = 1924, GrupoMundialista = "H", PaisId = P("COL"), LogoUrl = "https://flagcdn.com/w320/co.png" },
            new() { Nombre = "Selección Japón",        DirectorTecnico = "Hajime Moriyasu",     AnioFundacion = 1921, GrupoMundialista = "E", PaisId = P("JPN"), LogoUrl = "https://flagcdn.com/w320/jp.png" },
            new() { Nombre = "Selección Bélgica",      DirectorTecnico = "Domenico Tedesco",    AnioFundacion = 1895, GrupoMundialista = "F", PaisId = P("BEL"), LogoUrl = "https://flagcdn.com/w320/be.png" },
            new() { Nombre = "Selección Croacia",      DirectorTecnico = "Zlatko Dalic",        AnioFundacion = 1912, GrupoMundialista = "D", PaisId = P("CRO"), LogoUrl = "https://flagcdn.com/w320/hr.png" },
        };
        context.Equipos.AddRange(equipos);
        await context.SaveChangesAsync();

        //int E(string nombreContiene) => equipos.First(e => e.Nombre.Contains(nombreContiene)).Id; me dio error

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
        // Jugadores se crean desde la API en ExpandirJugadoresAsync, chai.

        // ---------------- CROMOS (20, uno por jugador) ----------------
        // Cromos se crean desde la API en ExpandirJugadoresAsync, chai.

        // ---------------- USUARIOS + COLECCIÓN (relación N:M) ----------------
        var usuarios = new List<Usuario>
        {
            new() { Nombre = "Nicolás López",   Email = "nicolas@cromos.com",
                    NombreUsuario = "nicolaslopez",
                    PasswordHash  = BCrypt.Net.BCrypt.HashPassword("PasswOrd#1") },
            new() { Nombre = "Melissa Torres", Email = "melissa@cromos.com",
                    NombreUsuario = "melissatorres",
                    PasswordHash  = BCrypt.Net.BCrypt.HashPassword("PassWord#2") }
        };
        context.Usuarios.AddRange(usuarios);
        await context.SaveChangesAsync();

    }

    // ─────────────────────────────────────────────────────────────────
    // EXPANSIÓN: agrega más jugadores y cromos sin tocar los existentes
    // ─────────────────────────────────────────────────────────────────
    public static async Task ExpandirJugadoresAsync(
        ApplicationDbContext context,
        ITheSportsDbService? sportsDb = null)
    {
        if (await context.Jugadores.AnyAsync()) return;

        var equipos = await context.Equipos.Include(e => e.Pais).ToListAsync();
        var album = await context.Albumes.FirstAsync();
        var usuarios = await context.Usuarios.ToListAsync();

        string Placeholder(string nombre) =>
            $"https://placehold.co/300x400/0B1F3A/D4AF37/png?text={Uri.EscapeDataString(nombre)}";

        int numeroCromo = 1;
        var todosLosCromos = new List<Cromo>();

        foreach (var (codigoPais, nombres) in JugadoresPorPais)
        {
            var equipo = equipos.FirstOrDefault(e => e.Pais?.CodigoFifa == codigoPais);
            if (equipo == null) continue;

            foreach (var nombre in nombres)
            {
                string? fotoUrl = null;
                string posicion = "Jugador";

                if (sportsDb != null)
                {
                    try
                    {
                        var datos = await sportsDb.BuscarJugadorAsync(nombre);
                        if (datos != null)
                        {
                            fotoUrl = datos.FotoUrl;
                            posicion = datos.Posicion ?? posicion;
                        }
                        await Task.Delay(400);
                    }
                    catch { }
                }

                var jugador = new Jugador
                {
                    Nombre = nombre,
                    Posicion = posicion,
                    NumeroCamiseta = 0,
                    FechaNacimiento = new DateTime(1995, 1, 1),
                    EquipoId = equipo.Id
                };
                context.Jugadores.Add(jugador);
                await context.SaveChangesAsync();

                var cromo = new Cromo
                {
                    NumeroCromo = numeroCromo++,
                    Edicion = "Mundial 2026",
                    ValorMercado = Math.Round((decimal)(new Random().NextDouble() * 200 + 50), 2),
                    FotoUrl = fotoUrl ?? Placeholder(nombre),
                    JugadorId = jugador.Id,
                    EquipoId = equipo.Id,
                    AlbumId = album.Id
                };
                context.Cromos.Add(cromo);
                await context.SaveChangesAsync();
                todosLosCromos.Add(cromo);
            }
        }

        if (todosLosCromos.Count >= 26 && usuarios.Count >= 2)
        {
            context.UsuarioCromos.AddRange(
                new UsuarioCromo { UsuarioId = usuarios[0].Id, CromoId = todosLosCromos[0].Id, FechaAdquisicion = DateTime.Today.AddDays(-10), Estado = EstadoCromo.Favorito },
                new UsuarioCromo { UsuarioId = usuarios[0].Id, CromoId = todosLosCromos[10].Id, FechaAdquisicion = DateTime.Today.AddDays(-5), Estado = EstadoCromo.Nuevo },
                new UsuarioCromo { UsuarioId = usuarios[0].Id, CromoId = todosLosCromos[5].Id, FechaAdquisicion = DateTime.Today.AddDays(-3), Estado = EstadoCromo.Repetido },
                new UsuarioCromo { UsuarioId = usuarios[1].Id, CromoId = todosLosCromos[25].Id, FechaAdquisicion = DateTime.Today.AddDays(-2), Estado = EstadoCromo.Intercambiable }
            );
            await context.SaveChangesAsync();
        }
    }

}
