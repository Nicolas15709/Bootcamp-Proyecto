# World Cup Sticker Manager 🏆

**Gestor de Cromos Mundialistas** — Aplicación web hecha con **ASP.NET Core MVC** para administrar y explorar un álbum de cromos (stickers) del Mundial de Fútbol.

Permite registrar y consultar **países**, **equipos (selecciones)**, **jugadores**, **cromos** y **álbumes**, con CRUD completo, filtros de búsqueda, subida de imágenes, validaciones, un dashboard con estadísticas y una interfaz responsiva con identidad visual mundialista.

> Proyecto final del módulo **Backend Pro**.

---

## 📖 ¿De qué trata el proyecto?

Es un sistema que simula la gestión de un **álbum de cromos del mundial**. El dominio está modelado tal como funciona en la vida real:

- Cada **País** tiene su **Equipo** (selección nacional).
- Cada **Equipo** tiene varios **Jugadores**.
- Cada **Cromo** representa a un jugador dentro de un equipo y pertenece a un **Álbum**.
- Los **Usuarios** poseen cromos (relación muchos-a-muchos con estado: nuevo, repetido, intercambiable, favorito).

La aplicación te permite:

- **Crear, ver, editar y eliminar** países, equipos, jugadores, cromos y álbumes (CRUD completo).
- **Navegar** entre entidades relacionadas (de un país a sus equipos, de un equipo a sus jugadores, etc.).
- **Filtrar y buscar** jugadores (por nombre, país, equipo, posición) y cromos (por número, jugador, equipo, país, álbum, edición).
- **Subir imágenes** (logos de equipos, fotos de cromos) o usar una URL.
- Ver un **dashboard** con totales, últimos cromos y equipos destacados.
- Seguir el **progreso de cada álbum** (cromos registrados vs. meta).

Al arrancar, la app **crea la base de datos y carga datos de ejemplo automáticamente**, así que la puedes ver funcionando de inmediato sin cargar nada a mano.

---

## 🛠️ Tecnologías y herramientas

| Capa | Tecnología |
|------|------------|
| Lenguaje | C# |
| Framework | ASP.NET Core MVC (.NET 10) |
| ORM (acceso a datos) | Entity Framework Core 10 |
| Base de datos | SQL Server Express (o LocalDB) |
| Frontend | Razor Views + Bootstrap 5 + Bootstrap Icons |
| Patrón de diseño | MVC + Servicios + ViewModels |

---

## ✅ Requisitos previos (qué debes tener instalado)

Antes de ejecutar el proyecto necesitas instalar estas herramientas en tu computadora:

### 1. .NET 10 SDK  *(obligatorio)*
El kit de desarrollo para compilar y correr la aplicación.
- Descarga: <https://dotnet.microsoft.com/download/dotnet/10.0>
- Verifica la instalación abriendo una terminal y ejecutando:
  ```bash
  dotnet --version
  ```

### 2. SQL Server Express  *(obligatorio)*
El motor de base de datos donde se guardan los datos.
- Descarga: <https://www.microsoft.com/sql-server/sql-server-downloads> (elige la edición **Express**, es gratis).
- Durante la instalación se crea una instancia llamada `SQLEXPRESS` (la que usa este proyecto).
- *(Alternativa)* Si prefieres **LocalDB**, ya viene con Visual Studio; solo cambia la cadena de conexión (ver más abajo).

### 3. Herramientas de Entity Framework Core  *(obligatorio si usas migraciones por comando)*
Permiten crear/actualizar la base de datos con `dotnet ef`. Instálalas una sola vez de forma global:
```bash
dotnet tool install --global dotnet-ef
```

### 4. Un editor o IDE  *(recomendado)*
- **Visual Studio 2022/2026** (Community es gratis) — incluye todo lo anterior, o
- **Visual Studio Code** + la extensión de C#.

### 5. Navicat o SQL Server Management Studio (SSMS)  *(opcional)*
Solo si quieres **inspeccionar la base de datos** visualmente (ver tablas, registros, etc.).
- Navicat: <https://www.navicat.com/>
- SSMS (gratis): <https://learn.microsoft.com/sql/ssms/download-sql-server-management-studio-ssms>

---

## 🚀 Cómo ejecutar el proyecto (paso a paso)

### Paso 1 — Clonar el repositorio
```bash
git clone URL_DEL_REPOSITORIO
cd "proyecto backend/WorldCupStickers"
```

### Paso 2 — Restaurar las dependencias
```bash
dotnet restore
```

### Paso 3 — Configurar la cadena de conexión
Abre `appsettings.json` y revisa la sección `ConnectionStrings`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=WorldCupStickersDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```
- Si usas **SQL Server Express** con la instancia por defecto, **no cambies nada**.
- Si usas **LocalDB**, reemplaza el servidor por:
  ```
  Server=(localdb)\\MSSQLLocalDB;Database=WorldCupStickersDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True
  ```

### Paso 4 — Crear la base de datos
```bash
dotnet ef database update
```
> Esto aplica las migraciones y crea la base `WorldCupStickersDb`.
> *(Opcional: la app también aplica las migraciones sola al arrancar, así que este paso es por si quieres prepararla antes).*

### Paso 5 — Ejecutar la aplicación
```bash
dotnet run
```
Al arrancar, el sistema **siembra datos iniciales automáticamente**:
> 8 países, 8 equipos, 20 jugadores, 20 cromos, 1 álbum y usuarios de ejemplo.

### Paso 6 — Abrir en el navegador
Abre la URL que muestra la consola (por ejemplo):
```
http://localhost:5199
```

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
├── appsettings.json  # Configuración (incluye la cadena de conexión)
└── Program.cs        # Punto de entrada, DI y arranque
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

## 🧰 Solución de problemas comunes

- **`dotnet ef` no se reconoce** → instala la herramienta: `dotnet tool install --global dotnet-ef` y reabre la terminal.
- **No conecta a la base de datos** → confirma que el servicio **SQL Server (SQLEXPRESS)** está corriendo y que el nombre del servidor en `appsettings.json` coincide con tu instancia.
- **Error de certificado SSL** → la cadena de conexión ya incluye `TrustServerCertificate=True`; mantenlo.
- **El puerto está ocupado** → cambia el puerto al ejecutar: `dotnet run --urls http://localhost:5050`.

---

## 📄 Documentación

En la carpeta `docs/` encontrarás el documento de entrega en PDF (objetivo, tecnologías, modelo de BD, capturas, decisiones técnicas e instrucciones).

---

## 👥 Integrantes

- Nicolás López Cadena
- Melissa Torres

## 🔗 Repositorio

- https://github.com/Nicolas15709/Bootcamp-Proyecto.git
