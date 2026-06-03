using Microsoft.EntityFrameworkCore;
using WorldCupStickers.Models;

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
            new() { Nombre = "Nicolás López",  Email = "nicolas@cromos.com" },
            new() { Nombre = "Coleccionista 2", Email = "colega@cromos.com" }
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
}
