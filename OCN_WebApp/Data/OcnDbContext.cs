using Microsoft.EntityFrameworkCore;
using OCN_WebApp.Models;

namespace  OCN_WebApp.Data;

public class OcnDbContext : DbContext
{
    public OcnDbContext(DbContextOptions<OcnDbContext>options)
    : base(options)
    {
    }
    public DbSet<Zona> Zonas {get;set;}
    public DbSet<RegistroOCN> RegistrosOCN {get;set;}
    public DbSet<Problematica> Problematicas{get;set;}
}