# World Cup Sticker Manager 🏆

**Gestor de Cromos Mundialistas** — Aplicación web ASP.NET Core MVC para administrar y explorar un catálogo de cromos del mundial: países, equipos, jugadores, cromos y álbumes, con CRUD completo, filtros, subida de imágenes, validaciones y dashboard.

Proyecto final del módulo **Backend Pro**.

---

## 🛠️ Tecnologías

| Capa | Tecnología |
|------|------------|
| Lenguaje | C# |
| Framework | ASP.NET Core MVC (.NET 10) |
| ORM | Entity Framework Core 10 |
| Base de datos | SQL Server Express / LocalDB |
| Frontend | Razor Views + Bootstrap 5 + Bootstrap Icons |
| Patrón | MVC + Servicios + ViewModels |

---

## 📁 Estructura del proyecto

```
WorldCupStickers/
├── Controllers/      # Home, Paises, Equipos, Jugadores, Cromos, Albumes
├── Models/           # Pais, Equipo, Jugador, Cromo, Album, Usuario, UsuarioCromo
├── Data/             # ApplicationDbContext, DbInitializer (seed)
├── Services/         # IFileUploadService, FileUploadService
├── ViewModels/       # Dashboard, filtros de Jugadores y Cromos
├── Views/            # Vistas Razor + partials (_CardCromo, _CardEquipo, _AlertMessages)
├── wwwroot/          # css, js, images/{equipos,cromos}
├── Migrations/       # Migraciones EF Core
├── appsettings.json
└── Program.cs
```

---

## 🗄️ Modelo de base de datos

- **Pais** (1) ──< **Equipo** (N)
- **Equipo** (1) ──< **Jugador** (N)
- **Equipo** (1) ──< **Cromo** (N)
- **Jugador** (1) ──< **Cromo** (N)
- **Album** (1) ──< **Cromo** (N)
- **Usuario** (N) >──< **Cromo** (N) — tabla intermedia **UsuarioCromo** (con `FechaAdquisicion` y `Estado`)

Índices únicos: `Cromo.NumeroCromo`, `Pais.CodigoFifa`, `Usuario.Email`.

---

## 🚀 Cómo ejecutar

### 1. Requisitos
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server Express o LocalDB
- (Opcional) Navicat / SSMS para inspeccionar la base de datos

### 2. Clonar y restaurar
```bash
git clone URL_DEL_REPOSITORIO
cd WorldCupStickers
dotnet restore
```

### 3. Configurar la cadena de conexión
En `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=WorldCupStickersDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```
> Si usas **LocalDB**, reemplaza el servidor por `(localdb)\\MSSQLLocalDB`.

### 4. Crear la base de datos y ejecutar
```bash
dotnet ef database update
dotnet run
```

La aplicación aplica las migraciones y **siembra datos iniciales automáticamente** al arrancar
(8 países, 8 equipos, 20 jugadores, 20 cromos, 1 álbum, usuarios de ejemplo).

Abre el navegador en la URL que indique la consola (ej. `http://localhost:5199`).

---

## ✨ Funcionalidades

- **CRUD completo** de Países, Equipos, Jugadores, Cromos y Álbumes.
- **Dashboard** con totales, últimos cromos y equipos destacados.
- **Vista catálogo** de cromos tipo carta coleccionable (responsive 4/2/1 columnas).
- **Filtros**: jugadores (nombre, país, equipo, posición) y cromos (número, jugador, equipo, país, álbum, edición).
- **Subida de imágenes** (logos y fotos) vía `FileUploadService`, o por URL.
- **Validaciones** cliente y servidor con Data Annotations.
- **Mensajes** de éxito/error con TempData.
- **Progreso de álbum** (cromos registrados / meta).

---

## 🔒 Seguridad

- Validación de extensión (`.jpg`, `.jpeg`, `.png`, `.webp`) y tamaño (máx. 2 MB) de imágenes.
- Nombres de archivo con `Guid` (no se usa el nombre original).
- Imágenes guardadas solo en `wwwroot/images`.
- `ValidateAntiForgeryToken` en formularios POST.
- Confirmación antes de eliminar y bloqueo si hay registros relacionados.

---

## 👥 Integrantes

- Nicolás López
- _(completar)_

## 🔗 Repositorio

- _(completar con el link de GitHub)_
