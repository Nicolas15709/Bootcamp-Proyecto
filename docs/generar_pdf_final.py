# -*- coding: utf-8 -*-
"""Documento PDF final de entrega — World Cup Sticker Manager."""
import os
from reportlab.lib.pagesizes import A4
from reportlab.lib.units import cm
from reportlab.lib import colors
from reportlab.lib.styles import getSampleStyleSheet, ParagraphStyle
from reportlab.lib.enums import TA_CENTER, TA_JUSTIFY
from reportlab.platypus import (
    SimpleDocTemplate, Paragraph, Spacer, PageBreak, Image, Table, TableStyle,
    ListFlowable, ListItem, HRFlowable, KeepTogether
)
from reportlab.lib.utils import ImageReader

BASE = os.path.dirname(os.path.abspath(__file__))
CAP = os.path.join(BASE, "capturas")
OUT = os.path.join(BASE, "World Cup Sticker Manager - Documentacion Final.pdf")

AZUL = colors.HexColor("#0B1F3A")
DORADO = colors.HexColor("#D4AF37")
ROJO = colors.HexColor("#C8102E")
VERDE = colors.HexColor("#1a9d63")
GRIS = colors.HexColor("#F5F7FA")
GRISTXT = colors.HexColor("#444444")

styles = getSampleStyleSheet()
def S(name, **kw): styles.add(ParagraphStyle(name, **kw))

S("Portada", parent=styles["Title"], fontSize=30, textColor=AZUL, leading=34, alignment=TA_CENTER, spaceAfter=6)
S("PortSub", parent=styles["Normal"], fontSize=14, textColor=DORADO, alignment=TA_CENTER, fontName="Helvetica-Bold", spaceAfter=4)
S("PortMeta", parent=styles["Normal"], fontSize=11, textColor=GRISTXT, alignment=TA_CENTER, leading=16)
S("H1", parent=styles["Heading1"], fontSize=17, textColor=AZUL, spaceBefore=10, spaceAfter=6)
S("H2", parent=styles["Heading2"], fontSize=13, textColor=ROJO, spaceBefore=8, spaceAfter=4)
S("Body", parent=styles["Normal"], fontSize=10.5, leading=15, alignment=TA_JUSTIFY, textColor=colors.HexColor("#1a1a1a"))
S("Cap", parent=styles["Normal"], fontSize=9, leading=12, alignment=TA_CENTER, textColor=GRISTXT, spaceBefore=3, spaceAfter=12, fontName="Helvetica-Oblique")
S("WB", parent=styles["Normal"], fontSize=10.5, leading=14, textColor=colors.HexColor("#1a1a1a"))

story = []

def hr():
    story.append(Spacer(1, 3)); story.append(HRFlowable(width="100%", thickness=1.2, color=DORADO)); story.append(Spacer(1, 8))

def img(path, w=15.5*cm, caption=None):
    if not os.path.exists(path): return
    iw, ih = ImageReader(path).getSize()
    h = w * ih / iw
    if h > 18*cm:
        h = 18*cm; w = h * iw / ih
    block = [Image(path, width=w, height=h)]
    if caption: block.append(Paragraph(caption, styles["Cap"]))
    story.append(KeepTogether(block))

def bullets(items):
    li = [ListItem(Paragraph(t, styles["WB"]), leftIndent=6) for t in items]
    story.append(ListFlowable(li, bulletType="bullet", bulletColor=DORADO, start="square", leftIndent=14))
    story.append(Spacer(1, 6))

def tabla(data, col_widths, header_bg=AZUL):
    t = Table(data, colWidths=col_widths)
    t.setStyle(TableStyle([
        ("BACKGROUND", (0,0), (-1,0), header_bg),
        ("TEXTCOLOR", (0,0), (-1,0), colors.white),
        ("FONTNAME", (0,0), (-1,0), "Helvetica-Bold"),
        ("FONTSIZE", (0,0), (-1,-1), 9.5),
        ("GRID", (0,0), (-1,-1), 0.5, colors.HexColor("#cccccc")),
        ("ROWBACKGROUNDS", (0,1), (-1,-1), [colors.white, GRIS]),
        ("TEXTCOLOR", (0,1), (-1,-1), GRISTXT),
        ("VALIGN", (0,0), (-1,-1), "MIDDLE"),
        ("TOPPADDING", (0,0), (-1,-1), 5), ("BOTTOMPADDING", (0,0), (-1,-1), 5),
        ("LEFTPADDING", (0,0), (-1,-1), 8),
    ]))
    story.append(t); story.append(Spacer(1, 8))

# ---------------- PORTADA ----------------
story.append(Spacer(1, 3*cm))
story.append(Paragraph("World Cup Sticker Manager", styles["Portada"]))
story.append(Paragraph("Gestor de Cromos Mundialistas", styles["PortSub"]))
story.append(Spacer(1, .5*cm))
story.append(HRFlowable(width="55%", thickness=2, color=DORADO, hAlign="CENTER"))
story.append(Spacer(1, .8*cm))
story.append(Paragraph("Proyecto Final &mdash; Core 6: Backend Pro Models &amp; Tools", styles["PortSub"]))
story.append(Spacer(1, .3*cm))
story.append(Paragraph("Sitio web de cromos del mundial desarrollado con ASP.NET Core MVC y Entity Framework Core.", styles["PortMeta"]))
story.append(Spacer(1, 2*cm))
meta = Table([
    ["Integrantes", "Nicolás López Cadena"],
    ["", "Melissa Torres"],
    ["Stack", "ASP.NET Core MVC (.NET 10) + EF Core 10"],
    ["Base de datos", "SQL Server Express"],
    ["Repositorio", "github.com/Nicolas15709/Bootcamp-Proyecto"],
    ["Fecha", "Junio 2026"],
], colWidths=[4.5*cm, 9.5*cm])
meta.setStyle(TableStyle([
    ("BACKGROUND", (0,0), (0,-1), AZUL), ("TEXTCOLOR", (0,0), (0,-1), colors.white),
    ("FONTNAME", (0,0), (0,-1), "Helvetica-Bold"), ("FONTSIZE", (0,0), (-1,-1), 10),
    ("BACKGROUND", (1,0), (1,-1), GRIS), ("TEXTCOLOR", (1,0), (1,-1), GRISTXT),
    ("GRID", (0,0), (-1,-1), .5, colors.white), ("VALIGN", (0,0), (-1,-1), "MIDDLE"),
    ("TOPPADDING", (0,0), (-1,-1), 7), ("BOTTOMPADDING", (0,0), (-1,-1), 7), ("LEFTPADDING", (0,0), (-1,-1), 10),
]))
story.append(meta)
story.append(PageBreak())

# ---------------- 1. OBJETIVO ----------------
story.append(Paragraph("1. Objetivo del proyecto", styles["H1"])); hr()
story.append(Paragraph(
    "Desarrollar una aplicaci&oacute;n web funcional y visualmente atractiva para la gesti&oacute;n de cromos "
    "del &aacute;lbum del mundial, utilizando el patr&oacute;n <b>MVC</b> en ASP.NET Core junto con "
    "<b>Entity Framework Core</b> para la persistencia de datos. El sistema permite explorar un cat&aacute;logo "
    "de cromos y administrar pa&iacute;ses, equipos, jugadores, cromos y &aacute;lbumes mediante operaciones "
    "<b>CRUD completas</b>, con navegaci&oacute;n entre entidades, filtros, validaciones y subida de "
    "im&aacute;genes.", styles["Body"]))

# ---------------- 2. TECNOLOGIAS ----------------
story.append(Paragraph("2. Tecnolog&iacute;as utilizadas", styles["H1"])); hr()
tabla([
    ["Capa", "Tecnología"],
    ["Lenguaje", "C#"],
    ["Framework", "ASP.NET Core MVC (.NET 10)"],
    ["ORM", "Entity Framework Core 10"],
    ["Base de datos", "SQL Server Express (instancia SQLEXPRESS)"],
    ["Frontend", "Razor Views + Bootstrap 5 + Bootstrap Icons + GSAP"],
    ["Seguridad", "BCrypt (hash de contraseñas) + sesión"],
    ["API externa", "TheSportsDB (fotos reales de jugadores)"],
    ["Patrón", "MVC + Servicios + ViewModels + Inyección de dependencias"],
], [4.5*cm, 10.5*cm])

# ---------------- 3. MODELO DE DATOS ----------------
story.append(Paragraph("3. Modelo de base de datos", styles["H1"])); hr()
story.append(Paragraph("Entidades y relaciones", styles["H2"]))
bullets([
    "<b>Pais</b> (1) &mdash;&lt; <b>Equipo</b> (N)",
    "<b>Equipo</b> (1) &mdash;&lt; <b>Jugador</b> (N)",
    "<b>Equipo</b> (1) &mdash;&lt; <b>Cromo</b> (N)",
    "<b>Jugador</b> (1) &mdash;&lt; <b>Cromo</b> (N)",
    "<b>Album</b> (1) &mdash;&lt; <b>Cromo</b> (N)",
    "<b>Usuario</b> (N) &gt;&mdash;&lt; <b>Cromo</b> (N) &mdash; tabla intermedia <b>UsuarioCromo</b> "
    "(relaci&oacute;n muchos-a-muchos con <i>FechaAdquisicion</i> y <i>Estado</i>).",
])
story.append(Paragraph(
    "Las claves for&aacute;neas usan <b>DeleteBehavior.Restrict</b> para impedir borrados que dejen registros "
    "hu&eacute;rfanos. &Iacute;ndices &uacute;nicos en <b>Cromo.NumeroCromo</b>, <b>Pais.CodigoFifa</b> y "
    "<b>Usuario.Email</b>.", styles["Body"]))
story.append(Spacer(1, 4))
story.append(Paragraph("Campos por modelo (seg&uacute;n especificaci&oacute;n)", styles["H2"]))
tabla([
    ["Modelo", "Campos"],
    ["País", "Nombre, Continente, CódigoFifa, RankingFifa"],
    ["Equipo", "Nombre, DirectorTécnico, AñoFundación, LogoUrl, GrupoMundialista"],
    ["Jugador", "Nombre, Posición, NúmeroCamiseta, FechaNacimiento"],
    ["Cromo", "NúmeroCromo, Edición, ValorMercado, FotoUrl"],
    ["Álbum", "Nombre, Año, CantidadCromos, EdiciónEspecial"],
], [3*cm, 12*cm], header_bg=ROJO)
story.append(PageBreak())

# ---------------- 4. CUMPLIMIENTO ----------------
story.append(Paragraph("4. Cumplimiento de requisitos", styles["H1"])); hr()
story.append(Paragraph("Todos los requisitos m&iacute;nimos y extras de la especificaci&oacute;n est&aacute;n implementados:", styles["Body"]))
story.append(Spacer(1, 4))
def check(data):
    t = Table(data, colWidths=[11*cm, 4*cm])
    t.setStyle(TableStyle([
        ("BACKGROUND", (0,0), (-1,0), AZUL), ("TEXTCOLOR", (0,0), (-1,0), colors.white),
        ("FONTNAME", (0,0), (-1,0), "Helvetica-Bold"), ("FONTSIZE", (0,0), (-1,-1), 9.5),
        ("GRID", (0,0), (-1,-1), .5, colors.HexColor("#cccccc")),
        ("ROWBACKGROUNDS", (0,1), (-1,-1), [colors.white, GRIS]),
        ("TEXTCOLOR", (0,1), (-1,-1), GRISTXT), ("TEXTCOLOR", (1,1), (1,-1), VERDE),
        ("FONTNAME", (1,1), (1,-1), "Helvetica-Bold"), ("ALIGN", (1,0), (1,-1), "CENTER"),
        ("VALIGN", (0,0), (-1,-1), "MIDDLE"),
        ("TOPPADDING", (0,0), (-1,-1), 5), ("BOTTOMPADDING", (0,0), (-1,-1), 5), ("LEFTPADDING", (0,0), (-1,-1), 8),
    ]))
    story.append(t); story.append(Spacer(1, 8))
check([
    ["Requisito", "Estado"],
    ["Modelo de datos (5 modelos relacionados)", "CUMPLE"],
    ["Relación muchos-a-muchos (Usuario–Cromo)", "CUMPLE"],
    ["CRUD completo de los 5 modelos", "CUMPLE"],
    ["Navegación entre entidades", "CUMPLE"],
    ["Filtros (jugadores, cromos, países)", "CUMPLE"],
    ["Subida y visualización de imágenes", "CUMPLE"],
    ["Validaciones cliente y servidor", "CUMPLE"],
    ["ASP.NET Core MVC", "CUMPLE"],
    ["Entity Framework Core", "CUMPLE"],
    ["Base de datos local (SQL Server)", "CUMPLE"],
    ["Migraciones aplicadas", "CUMPLE"],
    ["Inyección de dependencias", "CUMPLE"],
    ["Partial views y layout compartido", "CUMPLE"],
    ["Diseño responsive con Bootstrap", "CUMPLE"],
    ["Repositorio Git con commits del grupo", "CUMPLE"],
    ["Documento PDF con capturas", "CUMPLE"],
])
story.append(PageBreak())

# ---------------- 5. FUNCIONALIDADES ----------------
story.append(Paragraph("5. Funcionalidades", styles["H1"])); hr()
bullets([
    "<b>CRUD completo</b> de Pa&iacute;ses, Equipos, Jugadores, Cromos y &Aacute;lbumes (crear, ver, editar, eliminar).",
    "<b>Login</b> con usuario y contrase&ntilde;a <b>hasheada con BCrypt</b> y manejo de sesi&oacute;n.",
    "<b>Filtros</b>: jugadores (nombre, pa&iacute;s, equipo, posici&oacute;n) y cromos (n&uacute;mero, jugador, pa&iacute;s, &aacute;lbum).",
    "<b>Navegaci&oacute;n</b> entre entidades relacionadas (de un pa&iacute;s a sus equipos, jugadores y cromos).",
    "<b>Subida de im&aacute;genes</b> (logos y fotos) o por URL, con validaci&oacute;n de tipo y tama&ntilde;o.",
    "<b>Integraci&oacute;n con TheSportsDB</b>: descarga autom&aacute;tica de fotos reales de los jugadores.",
    "<b>Dashboard</b> con totales, &uacute;ltimos cromos y equipos destacados.",
    "<b>Validaciones</b> de cliente (jQuery) y servidor (Data Annotations), con bloqueo de borrado si hay registros relacionados.",
])

# ---------------- 6. CAPTURAS ----------------
story.append(Paragraph("6. Capturas del sistema", styles["H1"])); hr()

def seccion_captura(titulo, desc, archivo, figura):
    story.append(Paragraph(titulo, styles["H2"]))
    story.append(Paragraph(desc, styles["Body"]))
    story.append(Spacer(1, 4))
    img(os.path.join(CAP, "jpg", archivo.replace(".png",".jpg")), caption=figura)

seccion_captura("6.1 Inicio de sesi&oacute;n",
    "Pantalla de login con tem&aacute;tica futbolera: fondo de estadio, panel con borde dorado e inputs validados.",
    "cap_login.png", "Figura 1. Login del sistema.")
story.append(PageBreak())

seccion_captura("6.2 Pa&iacute;ses &mdash; listado y CRUD",
    "Listado de pa&iacute;ses con filtros, bot&oacute;n &quot;Nuevo pa&iacute;s&quot; y acciones de ver / editar / eliminar por fila.",
    "cap_paises.png", "Figura 2. Listado de pa&iacute;ses (CRUD completo).")
story.append(PageBreak())

seccion_captura("6.3 Pa&iacute;ses &mdash; formulario de creaci&oacute;n",
    "Formulario de alta de pa&iacute;s con validaciones de cliente y servidor (c&oacute;digo FIFA &uacute;nico).",
    "cap_paises_create.png", "Figura 3. Crear pa&iacute;s.")
story.append(PageBreak())

seccion_captura("6.4 Equipos",
    "Listado de equipos (selecciones) con logo, pa&iacute;s, director t&eacute;cnico y conteo de jugadores y cromos.",
    "cap_equipos.png", "Figura 4. Equipos.")
story.append(PageBreak())

seccion_captura("6.5 Jugadores",
    "Listado de jugadores con filtros por nombre, pa&iacute;s, equipo y posici&oacute;n, con CRUD completo.",
    "cap_jugadores.png", "Figura 5. Jugadores.")
story.append(PageBreak())

seccion_captura("6.6 Cat&aacute;logo de cromos",
    "Cat&aacute;logo tipo carta coleccionable con fotos reales (TheSportsDB), filtros y fondo de estadio.",
    "cap_cromos.png", "Figura 6. Cat&aacute;logo de cromos.")
story.append(PageBreak())

seccion_captura("6.7 &Aacute;lbumes",
    "Listado de &aacute;lbumes con progreso de completado (cromos registrados sobre la meta).",
    "cap_albumes.png", "Figura 7. &Aacute;lbumes.")
story.append(PageBreak())

# ---------------- 7. DECISIONES TECNICAS ----------------
story.append(Paragraph("7. Decisiones t&eacute;cnicas", styles["H1"])); hr()
story.append(Paragraph("Arquitectura por capas", styles["H2"]))
story.append(Paragraph(
    "Se sigui&oacute; el patr&oacute;n <b>MVC</b> extendido con una capa de <b>Servicios</b> "
    "(<i>FileUploadService</i> para im&aacute;genes y <i>TheSportsDbService</i> para la API externa) y "
    "<b>ViewModels</b> para el dashboard y los filtros. Todos los servicios y el "
    "<i>ApplicationDbContext</i> se registran por <b>inyecci&oacute;n de dependencias</b>.", styles["Body"]))
story.append(Spacer(1, 5))
story.append(Paragraph("Migraciones + Seed autom&aacute;tico", styles["H2"]))
story.append(Paragraph(
    "Al arrancar, la aplicaci&oacute;n aplica las migraciones pendientes y siembra los datos iniciales "
    "(pa&iacute;ses, equipos, jugadores con fotos reales, cromos, &aacute;lbum y usuarios) solo si la base "
    "est&aacute; vac&iacute;a. As&iacute; el proyecto se levanta y funciona sin pasos manuales.", styles["Body"]))
story.append(Spacer(1, 5))
story.append(Paragraph("Seguridad", styles["H2"]))
bullets([
    "Contrase&ntilde;as <b>hasheadas con BCrypt</b> (nunca en texto plano).",
    "<b>ValidateAntiForgeryToken</b> en todos los formularios POST (protecci&oacute;n CSRF).",
    "Validaci&oacute;n de extensi&oacute;n y tama&ntilde;o de im&aacute;genes; nombres con <b>Guid</b>.",
    "Bloqueo de borrado cuando existen registros relacionados.",
])
story.append(Paragraph("Fotos reales v&iacute;a API", styles["H2"]))
story.append(Paragraph(
    "Se integr&oacute; <b>TheSportsDB</b> para obtener autom&aacute;ticamente las fotos de los jugadores. "
    "El servicio prueba variantes del nombre (sin tildes, solo apellido) para maximizar los aciertos.", styles["Body"]))

# ---------------- 8. EJECUCION ----------------
story.append(Paragraph("8. Instrucciones de ejecuci&oacute;n", styles["H1"])); hr()
code = ParagraphStyle("Code", parent=styles["Code"], fontSize=9.5, leading=13,
                      backColor=AZUL, textColor=colors.white, borderPadding=8, spaceBefore=4, spaceAfter=8)
story.append(Paragraph("Requisitos: .NET 10 SDK, SQL Server Express y (opcional) Navicat/SSMS.", styles["Body"]))
story.append(Paragraph("1) Configurar la cadena de conexi&oacute;n en <b>appsettings.json</b>:", styles["Body"]))
story.append(Paragraph("Server=.\\\\SQLEXPRESS;Database=WorldCupStickersDb;<br/>Trusted_Connection=True;TrustServerCertificate=True", code))
story.append(Paragraph("2) Restaurar, crear la base de datos y ejecutar:", styles["Body"]))
story.append(Paragraph("dotnet restore<br/>dotnet ef database update<br/>dotnet run", code))
story.append(Paragraph(
    "La app aplica migraciones y siembra datos autom&aacute;ticamente. Abrir el navegador en la URL indicada "
    "(ej. <i>http://localhost:5199</i>). Usuarios de prueba: <b>nicolaslopez</b> y <b>melissatorres</b>.", styles["Body"]))

# ---------------- 9. INTEGRANTES ----------------
story.append(Paragraph("9. Integrantes y repositorio", styles["H1"])); hr()
bullets(["Nicol&aacute;s L&oacute;pez Cadena", "Melissa Torres"])
story.append(Paragraph("Repositorio: <a href='https://github.com/Nicolas15709/Bootcamp-Proyecto'>github.com/Nicolas15709/Bootcamp-Proyecto</a>", styles["Body"]))
story.append(Spacer(1, 14))
story.append(HRFlowable(width="100%", thickness=1, color=DORADO))
story.append(Spacer(1, 5))
story.append(Paragraph("World Cup Sticker Manager &mdash; Proyecto Final, Core 6: Backend Pro.",
    ParagraphStyle("foot", parent=styles["Normal"], fontSize=9, textColor=GRISTXT, alignment=TA_CENTER)))

def on_page(canvas, doc):
    canvas.saveState()
    canvas.setFillColor(DORADO); canvas.rect(0, A4[1]-0.35*cm, A4[0], 0.35*cm, fill=1, stroke=0)
    canvas.setFont("Helvetica", 8); canvas.setFillColor(GRISTXT)
    if doc.page > 1:
        canvas.drawRightString(A4[0]-2*cm, 1*cm, "Página %d" % doc.page)
        canvas.drawString(2*cm, 1*cm, "World Cup Sticker Manager")
    canvas.restoreState()

doc = SimpleDocTemplate(OUT, pagesize=A4, leftMargin=2.2*cm, rightMargin=2.2*cm,
                        topMargin=2*cm, bottomMargin=1.8*cm,
                        title="World Cup Sticker Manager - Documentacion Final",
                        author="Nicolas Lopez Cadena, Melissa Torres")
doc.build(story, onFirstPage=on_page, onLaterPages=on_page)
print("PDF:", OUT)
print("Tamaño:", os.path.getsize(OUT), "bytes")
