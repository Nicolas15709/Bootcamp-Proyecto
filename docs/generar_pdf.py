# -*- coding: utf-8 -*-
"""Genera el documento PDF de entrega del proyecto World Cup Sticker Manager."""
import os
from reportlab.lib.pagesizes import A4
from reportlab.lib.units import cm
from reportlab.lib import colors
from reportlab.lib.styles import getSampleStyleSheet, ParagraphStyle
from reportlab.lib.enums import TA_CENTER, TA_JUSTIFY, TA_LEFT
from reportlab.platypus import (
    SimpleDocTemplate, Paragraph, Spacer, PageBreak, Image, Table, TableStyle,
    ListFlowable, ListItem, HRFlowable, KeepTogether
)
from reportlab.lib.utils import ImageReader

BASE = os.path.dirname(os.path.abspath(__file__))
CAP = os.path.join(BASE, "capturas")
OUT = os.path.join(BASE, "World Cup Sticker Manager - Documentacion.pdf")

# Paleta del proyecto
AZUL = colors.HexColor("#0B1F3A")
AZUL2 = colors.HexColor("#13294e")
DORADO = colors.HexColor("#D4AF37")
ROJO = colors.HexColor("#C8102E")
GRIS = colors.HexColor("#F5F7FA")
GRISTXT = colors.HexColor("#444444")

styles = getSampleStyleSheet()

def S(name, **kw):
    styles.add(ParagraphStyle(name, **kw))

S("Portada", parent=styles["Title"], fontSize=30, textColor=AZUL, leading=36, alignment=TA_CENTER, spaceAfter=6)
S("PortadaSub", parent=styles["Normal"], fontSize=14, textColor=DORADO, alignment=TA_CENTER, spaceAfter=4, fontName="Helvetica-Bold")
S("PortadaMeta", parent=styles["Normal"], fontSize=11, textColor=GRISTXT, alignment=TA_CENTER, leading=16)
S("H1", parent=styles["Heading1"], fontSize=18, textColor=AZUL, spaceBefore=10, spaceAfter=8)
S("H2", parent=styles["Heading2"], fontSize=13.5, textColor=ROJO, spaceBefore=8, spaceAfter=4)
S("Body", parent=styles["Normal"], fontSize=10.5, leading=15, alignment=TA_JUSTIFY, textColor=colors.HexColor("#1a1a1a"))
S("Cap", parent=styles["Normal"], fontSize=9, leading=12, alignment=TA_CENTER, textColor=GRISTXT, spaceBefore=3, spaceAfter=10, fontName="Helvetica-Oblique")
S("WBullet", parent=styles["Normal"], fontSize=10.5, leading=14, textColor=colors.HexColor("#1a1a1a"))

story = []

def hr():
    story.append(Spacer(1, 4))
    story.append(HRFlowable(width="100%", thickness=1.2, color=DORADO))
    story.append(Spacer(1, 8))

def img(path, max_w=15.5*cm, caption=None):
    if not os.path.exists(path):
        return
    ir = ImageReader(path)
    iw, ih = ir.getSize()
    w = max_w
    h = w * ih / iw
    max_h = 19*cm
    if h > max_h:
        h = max_h
        w = h * iw / ih
    block = [Image(path, width=w, height=h)]
    if caption:
        block.append(Paragraph(caption, styles["Cap"]))
    story.append(KeepTogether(block))

def bullets(items):
    li = [ListItem(Paragraph(t, styles["WBullet"]), leftIndent=6) for t in items]
    story.append(ListFlowable(li, bulletType="bullet", bulletColor=DORADO, start="square", leftIndent=14))
    story.append(Spacer(1, 6))

# ---------------- PORTADA ----------------
story.append(Spacer(1, 3.2*cm))
story.append(Paragraph("World Cup Sticker Manager", styles["Portada"]))
story.append(Paragraph("Gestor de Cromos Mundialistas", styles["PortadaSub"]))
story.append(Spacer(1, 0.5*cm))
story.append(HRFlowable(width="55%", thickness=2, color=DORADO, hAlign="CENTER"))
story.append(Spacer(1, 0.8*cm))
story.append(Paragraph("Proyecto Final &mdash; M&oacute;dulo Backend Pro", styles["PortadaSub"]))
story.append(Spacer(1, 0.3*cm))
story.append(Paragraph(
    "Aplicaci&oacute;n web ASP.NET Core MVC para administrar y explorar un cat&aacute;logo "
    "de cromos del mundial: pa&iacute;ses, equipos, jugadores, cromos y &aacute;lbumes.",
    styles["PortadaMeta"]))
story.append(Spacer(1, 2.2*cm))

meta_tbl = Table([
    ["Integrantes", "Nicolás López Cadena"],
    ["", "Melissa Torres"],
    ["Tecnología", "ASP.NET Core MVC (.NET 10) + EF Core 10"],
    ["Base de datos", "SQL Server Express"],
    ["Fecha", "Junio 2026"],
], colWidths=[4.5*cm, 9.5*cm])
meta_tbl.setStyle(TableStyle([
    ("BACKGROUND", (0,0), (0,-1), AZUL),
    ("TEXTCOLOR", (0,0), (0,-1), colors.white),
    ("FONTNAME", (0,0), (0,-1), "Helvetica-Bold"),
    ("FONTNAME", (1,0), (1,-1), "Helvetica"),
    ("FONTSIZE", (0,0), (-1,-1), 10),
    ("BACKGROUND", (1,0), (1,-1), GRIS),
    ("TEXTCOLOR", (1,0), (1,-1), GRISTXT),
    ("GRID", (0,0), (-1,-1), 0.5, colors.white),
    ("VALIGN", (0,0), (-1,-1), "MIDDLE"),
    ("TOPPADDING", (0,0), (-1,-1), 7),
    ("BOTTOMPADDING", (0,0), (-1,-1), 7),
    ("LEFTPADDING", (0,0), (-1,-1), 10),
]))
story.append(meta_tbl)
story.append(PageBreak())

# ---------------- 1. OBJETIVO ----------------
story.append(Paragraph("1. Objetivo del proyecto", styles["H1"]))
hr()
story.append(Paragraph(
    "El objetivo de este proyecto es desarrollar una aplicaci&oacute;n web completa que permita "
    "gestionar una colecci&oacute;n de cromos (stickers) del Mundial de F&uacute;tbol. El sistema "
    "modela el dominio real de un &aacute;lbum de cromos: cada <b>pa&iacute;s</b> agrupa "
    "<b>equipos</b> (selecciones), cada equipo re&uacute;ne a sus <b>jugadores</b>, y cada "
    "<b>cromo</b> representa a un jugador dentro de un equipo y pertenece a un <b>&aacute;lbum</b>. "
    "Adem&aacute;s se modela la relaci&oacute;n muchos-a-muchos entre <b>usuarios</b> y los cromos "
    "que poseen.", styles["Body"]))
story.append(Spacer(1, 6))
story.append(Paragraph(
    "La aplicaci&oacute;n implementa un <b>CRUD completo</b> sobre las cinco entidades principales, "
    "navegaci&oacute;n entre entidades relacionadas, filtros de b&uacute;squeda, subida de "
    "im&aacute;genes, validaciones de cliente y servidor, un <b>dashboard</b> con indicadores y una "
    "interfaz responsiva con la identidad visual del Mundial.", styles["Body"]))
story.append(Spacer(1, 8))
story.append(Paragraph("Objetivos espec&iacute;ficos", styles["H2"]))
bullets([
    "Aplicar el patr&oacute;n <b>MVC</b> con separaci&oacute;n de responsabilidades (Modelos, Vistas, Controladores, Servicios y ViewModels).",
    "Modelar relaciones 1:N y N:M con <b>Entity Framework Core</b> y Fluent API.",
    "Implementar validaciones robustas con <b>Data Annotations</b> a nivel de cliente y servidor.",
    "Construir una experiencia de usuario moderna y coherente con <b>Bootstrap 5</b>.",
    "Sembrar datos iniciales de forma autom&aacute;tica para una demostraci&oacute;n inmediata.",
])

# ---------------- 2. TECNOLOGIAS ----------------
story.append(Paragraph("2. Tecnolog&iacute;as utilizadas", styles["H1"]))
hr()
tech = Table([
    ["Capa", "Tecnología"],
    ["Lenguaje", "C#"],
    ["Framework", "ASP.NET Core MVC (.NET 10)"],
    ["ORM", "Entity Framework Core 10"],
    ["Base de datos", "SQL Server Express"],
    ["Frontend", "Razor Views + Bootstrap 5 + Bootstrap Icons"],
    ["Patrón", "MVC + Servicios + ViewModels"],
    ["Herramientas", "Navicat / SSMS, Visual Studio / dotnet CLI"],
], colWidths=[4.5*cm, 10.5*cm])
tech.setStyle(TableStyle([
    ("BACKGROUND", (0,0), (-1,0), AZUL),
    ("TEXTCOLOR", (0,0), (-1,0), colors.white),
    ("FONTNAME", (0,0), (-1,0), "Helvetica-Bold"),
    ("FONTNAME", (0,1), (0,-1), "Helvetica-Bold"),
    ("FONTSIZE", (0,0), (-1,-1), 10),
    ("GRID", (0,0), (-1,-1), 0.5, colors.HexColor("#cccccc")),
    ("ROWBACKGROUNDS", (0,1), (-1,-1), [colors.white, GRIS]),
    ("TEXTCOLOR", (0,1), (-1,-1), GRISTXT),
    ("TOPPADDING", (0,0), (-1,-1), 6),
    ("BOTTOMPADDING", (0,0), (-1,-1), 6),
    ("LEFTPADDING", (0,0), (-1,-1), 10),
]))
story.append(tech)
story.append(PageBreak())

# ---------------- 3. MODELO DE BD ----------------
story.append(Paragraph("3. Modelo de base de datos", styles["H1"]))
hr()
story.append(Paragraph("Entidades y relaciones", styles["H2"]))
bullets([
    "<b>Pais</b> (1) &mdash;&lt; <b>Equipo</b> (N)",
    "<b>Equipo</b> (1) &mdash;&lt; <b>Jugador</b> (N)",
    "<b>Equipo</b> (1) &mdash;&lt; <b>Cromo</b> (N)",
    "<b>Jugador</b> (1) &mdash;&lt; <b>Cromo</b> (N)",
    "<b>Album</b> (1) &mdash;&lt; <b>Cromo</b> (N)",
    "<b>Usuario</b> (N) &gt;&mdash;&lt; <b>Cromo</b> (N) &mdash; tabla intermedia <b>UsuarioCromo</b> "
    "(con <i>FechaAdquisicion</i> y <i>Estado</i>).",
])
story.append(Paragraph(
    "Las claves for&aacute;neas usan <b>DeleteBehavior.Restrict</b> para impedir borrados que dejen "
    "registros hu&eacute;rfanos; la relaci&oacute;n con <b>Album</b> y la tabla <b>UsuarioCromo</b> "
    "usan borrado en cascada. La tabla <b>UsuarioCromo</b> tiene una <b>clave compuesta</b> "
    "(UsuarioId + CromoId).", styles["Body"]))
story.append(Spacer(1, 6))
story.append(Paragraph("&Iacute;ndices &uacute;nicos", styles["H2"]))
bullets([
    "<b>Cromo.NumeroCromo</b> &mdash; cada cromo tiene un n&uacute;mero irrepetible.",
    "<b>Pais.CodigoFifa</b> &mdash; c&oacute;digo FIFA de 3 letras &uacute;nico por pa&iacute;s.",
    "<b>Usuario.Email</b> &mdash; correo &uacute;nico por usuario.",
])
story.append(Paragraph("Datos sembrados (seed autom&aacute;tico)", styles["H2"]))
seed = Table([
    ["Entidad", "Cantidad"],
    ["Países", "8"], ["Equipos", "8"], ["Jugadores", "20"],
    ["Cromos", "20"], ["Álbumes", "1"], ["Usuarios", "2"], ["UsuarioCromo", "4"],
], colWidths=[7.5*cm, 7.5*cm])
seed.setStyle(TableStyle([
    ("BACKGROUND", (0,0), (-1,0), ROJO),
    ("TEXTCOLOR", (0,0), (-1,0), colors.white),
    ("FONTNAME", (0,0), (-1,0), "Helvetica-Bold"),
    ("FONTSIZE", (0,0), (-1,-1), 10),
    ("GRID", (0,0), (-1,-1), 0.5, colors.HexColor("#cccccc")),
    ("ROWBACKGROUNDS", (0,1), (-1,-1), [colors.white, GRIS]),
    ("ALIGN", (1,0), (1,-1), "CENTER"),
    ("TEXTCOLOR", (0,1), (-1,-1), GRISTXT),
    ("TOPPADDING", (0,0), (-1,-1), 5),
    ("BOTTOMPADDING", (0,0), (-1,-1), 5),
    ("LEFTPADDING", (0,0), (-1,-1), 10),
]))
story.append(seed)
story.append(PageBreak())

# ---------------- 4. CAPTURAS ----------------
story.append(Paragraph("4. Capturas del sistema", styles["H1"]))
hr()
story.append(Paragraph("4.1 Dashboard / P&aacute;gina de inicio", styles["H2"]))
story.append(Paragraph(
    "Panel principal con indicadores (totales por entidad), &uacute;ltimos cromos agregados y "
    "equipos destacados.", styles["Body"]))
story.append(Spacer(1, 4))
img(os.path.join(CAP, "dashboard.png"), caption="Figura 1. Dashboard con tarjetas de indicadores y &uacute;ltimos cromos.")
story.append(PageBreak())

story.append(Paragraph("4.2 Cat&aacute;logo de cromos", styles["H2"]))
story.append(Paragraph(
    "Vista de cat&aacute;logo tipo carta coleccionable con filtros por n&uacute;mero, jugador, "
    "equipo, pa&iacute;s, &aacute;lbum y edici&oacute;n.", styles["Body"]))
story.append(Spacer(1, 4))
img(os.path.join(CAP, "cromos.png"), caption="Figura 2. Cat&aacute;logo de cromos con filtros y tarjetas coleccionables.")
story.append(PageBreak())

story.append(Paragraph("4.3 Detalle de &aacute;lbum", styles["H2"]))
story.append(Paragraph(
    "Detalle del &aacute;lbum con barra de progreso (cromos registrados sobre la meta) y el listado "
    "de cromos que lo componen.", styles["Body"]))
story.append(Spacer(1, 4))
img(os.path.join(CAP, "album.png"), caption="Figura 3. Detalle de &aacute;lbum con progreso de completado.")
story.append(PageBreak())

story.append(Paragraph("4.4 Listado de jugadores", styles["H2"]))
story.append(Paragraph(
    "Listado de jugadores con filtros por nombre, pa&iacute;s, equipo y posici&oacute;n.", styles["Body"]))
story.append(Spacer(1, 4))
img(os.path.join(CAP, "jugadores.png"), caption="Figura 4. Listado de jugadores con filtros de b&uacute;squeda.")
story.append(PageBreak())

# ---------------- 5. DECISIONES TECNICAS ----------------
story.append(Paragraph("5. Decisiones t&eacute;cnicas", styles["H1"]))
hr()
story.append(Paragraph("Arquitectura por capas", styles["H2"]))
story.append(Paragraph(
    "Se sigui&oacute; el patr&oacute;n <b>MVC</b> extendido con una capa de <b>Servicios</b> "
    "(<i>FileUploadService</i>) y <b>ViewModels</b> para el dashboard y los filtros. Los "
    "controladores son delgados: orquestan el acceso a datos v&iacute;a "
    "<i>ApplicationDbContext</i> y delegan responsabilidades transversales (subida de archivos) a "
    "servicios inyectados con DI.", styles["Body"]))
story.append(Spacer(1, 6))
story.append(Paragraph("Migraciones + Seed autom&aacute;tico", styles["H2"]))
story.append(Paragraph(
    "Al arrancar, la aplicaci&oacute;n aplica las migraciones pendientes (<i>MigrateAsync</i>) y "
    "ejecuta <i>DbInitializer</i>, que siembra los datos solo si la base est&aacute; vac&iacute;a. "
    "Esto permite levantar el proyecto y verlo funcionando sin pasos manuales de carga.", styles["Body"]))
story.append(Spacer(1, 6))
story.append(Paragraph("8 pa&iacute;ses y 8 selecciones", styles["H2"]))
story.append(Paragraph(
    "Aunque el m&iacute;nimo era de 5 pa&iacute;ses, se modelaron <b>8 pa&iacute;ses con su "
    "respectiva selecci&oacute;n nacional</b> para dar coherencia tem&aacute;tica al dominio "
    "mundialista, manteniendo una relaci&oacute;n 1:1 natural entre pa&iacute;s y selecci&oacute;n.", styles["Body"]))
story.append(Spacer(1, 6))
story.append(Paragraph("Manejo de im&aacute;genes", styles["H2"]))
story.append(Paragraph(
    "El sistema admite <b>dos modos</b>: subir un archivo (que se guarda en "
    "<i>wwwroot/images</i> con un nombre &uacute;nico basado en <i>Guid</i>) o indicar una "
    "<b>URL</b> externa. El seed utiliza URLs p&uacute;blicas para banderas y placeholders, de modo "
    "que la demostraci&oacute;n no depende de archivos locales.", styles["Body"]))
story.append(Spacer(1, 6))
story.append(Paragraph("Enum como texto", styles["H2"]))
story.append(Paragraph(
    "El estado del cromo (<i>Nuevo, Repetido, Intercambiable, Favorito</i>) se persiste como "
    "<b>cadena</b> (<i>HasConversion&lt;string&gt;</i>) para que los datos sean legibles "
    "directamente desde Navicat / SSMS sin necesidad de traducir valores num&eacute;ricos.", styles["Body"]))
story.append(PageBreak())

# ---------------- 6. SEGURIDAD ----------------
story.append(Paragraph("6. Seguridad y validaciones", styles["H1"]))
hr()
bullets([
    "Validaci&oacute;n de <b>extensi&oacute;n</b> (.jpg, .jpeg, .png, .webp) y <b>tama&ntilde;o</b> (m&aacute;x. 2 MB) de las im&aacute;genes subidas.",
    "Nombres de archivo generados con <b>Guid</b>: nunca se usa el nombre original del archivo del usuario.",
    "Im&aacute;genes guardadas &uacute;nicamente dentro de <b>wwwroot/images</b>.",
    "<b>ValidateAntiForgeryToken</b> en todos los formularios POST (protecci&oacute;n CSRF).",
    "Validaciones de dominio con <b>Data Annotations</b> (rangos, longitudes, expresiones regulares, campos requeridos) en cliente y servidor.",
    "Confirmaci&oacute;n antes de eliminar y <b>bloqueo de borrado</b> cuando existen registros relacionados, evitando inconsistencias.",
])

# ---------------- 7. EJECUCION ----------------
story.append(Paragraph("7. Instrucciones de ejecuci&oacute;n", styles["H1"]))
hr()
story.append(Paragraph("Requisitos", styles["H2"]))
bullets([
    ".NET 10 SDK",
    "SQL Server Express (o LocalDB)",
    "(Opcional) Navicat o SSMS para inspeccionar la base de datos",
])
story.append(Paragraph("Pasos", styles["H2"]))
code_style = ParagraphStyle("Code", parent=styles["Code"], fontSize=9.5, leading=13,
                            backColor=colors.HexColor("#0B1F3A"), textColor=colors.white,
                            borderPadding=8, leftIndent=2, spaceBefore=4, spaceAfter=10)
story.append(Paragraph(
    "1) Configurar la cadena de conexi&oacute;n en <b>appsettings.json</b>:", styles["Body"]))
story.append(Paragraph(
    'Server=.\\\\SQLEXPRESS;Database=WorldCupStickersDb;<br/>'
    'Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True',
    code_style))
story.append(Paragraph("2) Restaurar, crear la base de datos y ejecutar:", styles["Body"]))
story.append(Paragraph(
    "dotnet restore<br/>dotnet ef database update<br/>dotnet run", code_style))
story.append(Paragraph(
    "La aplicaci&oacute;n aplica las migraciones y siembra los datos iniciales autom&aacute;ticamente. "
    "Luego se abre el navegador en la URL indicada por la consola (por ejemplo "
    "<i>http://localhost:5199</i>).", styles["Body"]))

# ---------------- 8. INTEGRANTES / REPO ----------------
story.append(Paragraph("8. Integrantes y repositorio", styles["H1"]))
hr()
story.append(Paragraph("Integrantes", styles["H2"]))
bullets([
    "Nicol&aacute;s L&oacute;pez Cadena",
    "Melissa Torres",
])
story.append(Paragraph("Repositorio", styles["H2"]))
story.append(Paragraph(
    "<a href='https://github.com/Nicolas15709/Bootcamp-Proyecto'>https://github.com/Nicolas15709/Bootcamp-Proyecto</a>", styles["Body"]))
story.append(Spacer(1, 16))
story.append(HRFlowable(width="100%", thickness=1, color=DORADO))
story.append(Spacer(1, 6))
story.append(Paragraph(
    "World Cup Sticker Manager &mdash; Proyecto Final, M&oacute;dulo Backend Pro.",
    ParagraphStyle("foot", parent=styles["Normal"], fontSize=9, textColor=GRISTXT, alignment=TA_CENTER)))


# ---------------- Pie y encabezado ----------------
def on_page(canvas, doc):
    canvas.saveState()
    # franja superior dorada fina
    canvas.setFillColor(DORADO)
    canvas.rect(0, A4[1]-0.35*cm, A4[0], 0.35*cm, fill=1, stroke=0)
    # numero de pagina
    canvas.setFont("Helvetica", 8)
    canvas.setFillColor(GRISTXT)
    if doc.page > 1:
        canvas.drawRightString(A4[0]-2*cm, 1*cm, "Página %d" % doc.page)
        canvas.drawString(2*cm, 1*cm, "World Cup Sticker Manager")
    canvas.restoreState()

doc = SimpleDocTemplate(OUT, pagesize=A4,
                        leftMargin=2.2*cm, rightMargin=2.2*cm,
                        topMargin=2*cm, bottomMargin=1.8*cm,
                        title="World Cup Sticker Manager - Documentacion",
                        author="Nicolas Lopez Cadena")
doc.build(story, onFirstPage=on_page, onLaterPages=on_page)
print("PDF generado:", OUT)
print("Tamaño:", os.path.getsize(OUT), "bytes")
