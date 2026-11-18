namespace OCN_WebApp.Models;
using System.ComponentModel.DataAnnotations;   

public class RegistroOCN
{
    public int RegistroOCNId{get; set; }
    [Required(ErrorMessage = "Este campo es obligatorio")]
    public int? ZonaId{get; set;}
    [Required(ErrorMessage = "Este campo es obligatorio")]
    public int? ProblematicaId{get; set; }
    public Zona? Zona{get;set; }
    public Problematica? Problematica{get; set;}
    [Range(1, 5, ErrorMessage = "El número seleccionado tiene que estar entre 1 y 5")]
    public int Intensidad {get; set; }
    [Range(1, 5, ErrorMessage = "El número seleccionado tiene que estar entre 1 y 5")]
    public int Frecuencia {get;set;}
}