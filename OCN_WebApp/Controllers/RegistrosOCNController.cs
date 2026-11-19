using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OCN_WebApp.Data;
using Microsoft.AspNetCore.Mvc.Rendering;  
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


    public IActionResult Create()
    {
        ViewBag.ZonaId = new SelectList(_context.Zonas, "ZonaId", "NombreZona");

        
        ViewBag.ProblematicaId = new SelectList(_context.Problematicas, "ProblematicaId", "NombreProblematica");

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RegistroOCN registro)   
    {
        if (ModelState.IsValid)               
        {
            _context.Add(registro);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));   
        }

    
        ViewBag.ZonaId = new SelectList(_context.Zonas, "ZonaId", "NombreZona", registro.ZonaId);
        ViewBag.ProblematicaId = new SelectList(_context.Problematicas, "ProblematicaId", "NombreProblematica", registro.ProblematicaId);

        return View(registro);
    }


    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var registro = await _context.RegistrosOCN.FindAsync(id);

        if (registro == null)
        {
            return NotFound();
        }

        ViewBag.ZonaId = new SelectList(_context.Zonas, "ZonaId", "NombreZona", registro.ZonaId);
        ViewBag.ProblematicaId = new SelectList(_context.Problematicas, "ProblematicaId", "NombreProblematica", registro.ProblematicaId);

        return View(registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, RegistroOCN registro)
    {
        if (id != registro.RegistroOCNId)          
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _context.Update(registro);      
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.ZonaId = new SelectList(_context.Zonas, "ZonaId", "NombreZona", registro.ZonaId);
        ViewBag.ProblematicaId = new SelectList(_context.Problematicas, "ProblematicaId", "NombreProblematica", registro.ProblematicaId);

        return View(registro);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

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

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var registro = await _context.RegistrosOCN.FindAsync(id);

        if (registro != null)
        {
            _context.Remove(registro);      
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}