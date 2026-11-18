using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OCN_WebApp.Data;
using Microsoft.AspNetCore.Mvc.Rendering;   // para poder usar SelectList
using OCN_WebApp.Models;

namespace OCN_WebApp.Controllers;

public class RegistrosOCNController : Controller
{
    private readonly OcnDbContext _context;

    public RegistrosOCNController(OcnDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var registros = await _context.RegistrosOCN
        .Include(r => r.Zona)
        .Include(r=> r.Problematica)
        .ToListAsync();

        return View(registros);
    }


  public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var registro = await _context.RegistrosOCN            // ¿qué cajón? (DÁMELOS → plural)
            .Include(r => r.Zona)                       // igual que en Index
            .Include(r => r.Problematica)
            .FirstOrDefaultAsync(r => r.RegistroOCNId == id);    // ¿qué propiedad es tu PK?

        if (registro == null)
        {
            return NotFound();
        }

        return View(registro);
    }


     // GET: /RegistrosOCN/Create  → entrega la "hoja en blanco"
    public IActionResult Create()
    {
        // Lista de Zonas: (¿qué cajón del DbContext?, ¿qué se guarda?, ¿qué texto se ve?)
        ViewBag.ZonaId = new SelectList(_context.Zonas, "ZonaId", "NombreZona");

        
        ViewBag.ProblematicaId = new SelectList(_context.Problematicas, "ProblematicaId", "NombreProblematica");

        return View();
    }

    // POST: /RegistrosOCN/Create  → recibe la hoja llena y la revisa
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RegistroOCN registro)   // ¿qué clase llega del formulario? (singular)
    {
        if (ModelState.IsValid)               // ¿qué propiedad dice "todo cumple las reglas"?
        {
            _context.Add(registro);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));   // ¿a qué pantalla regresamos después de guardar?
        }

        // Si llegamos aquí, algo estaba mal → volver a llenar las listas
        // El 4º dato deja seleccionado lo que el usuario ya había elegido
        ViewBag.ZonaId = new SelectList(_context.Zonas, "ZonaId", "NombreZona", registro.ZonaId);
        ViewBag.ProblematicaId = new SelectList(_context.Problematicas, "ProblematicaId", "NombreProblematica", registro.ProblematicaId);

        return View(registro);
    }


     // GET: /RegistrosOCN/Edit/5  → entrega la hoja YA LLENA
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        // Buscar el registro por su llave (pista: es más corto que en Details, aquí no hace falta Include)
        var registro = await _context.RegistrosOCN.FindAsync(id);   // ¿qué cajón?

        if (registro == null)
        {
            return NotFound();
        }

        // Listas de los dropdowns, con la opción ACTUAL del registro ya seleccionada
        ViewBag.ZonaId = new SelectList(_context.Zonas, "ZonaId", "NombreZona", registro.ZonaId);
        ViewBag.ProblematicaId = new SelectList(_context.Problematicas, "ProblematicaId", "NombreProblematica", registro.ProblematicaId);

        return View(registro);
    }

    // POST: /RegistrosOCN/Edit/5  → recibe la hoja corregida
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, RegistroOCN registro)
    {
        // Seguridad: el número de la dirección debe ser el mismo que el de la hoja
        if (id != registro.RegistroOCNId)          // ¿cuál es la llave primaria de RegistroOCN?
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _context.Update(registro);      // ¿Add o Update?
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Si algo estaba mal → volver a llenar las listas (igual que en Create)
        ViewBag.ZonaId = new SelectList(_context.Zonas, "ZonaId", "NombreZona", registro.ZonaId);
        ViewBag.ProblematicaId = new SelectList(_context.Problematicas, "ProblematicaId", "NombreProblematica", registro.ProblematicaId);

        return View(registro);
    }

    // GET: /RegistrosOCN/Delete/5  → "¿Seguro que quieres borrarlo?"
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        // Igual que en Details: aquí SÍ queremos mostrar los nombres de zona y problemática
        var registro = await _context.RegistrosOCN
            .Include(r => r.Zona)
            .Include(r => r.Problematica)
            .FirstOrDefaultAsync(r => r.RegistroOCNId == id);

        if (registro == null)
        {
            return NotFound();
        }

        return View(registro);
    }

    // POST: /RegistrosOCN/Delete/5  → aquí SÍ se borra
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var registro = await _context.RegistrosOCN.FindAsync(id);

        if (registro != null)
        {
            _context.Remove(registro);      // ¿qué acción es lo contrario de Add?
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}