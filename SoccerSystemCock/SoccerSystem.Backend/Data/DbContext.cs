using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Collections.Generic;
using SoccerSystem.Shared.Entites;

namespace SoccerSystem.Backend.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Country> Countries { get; set; }
    public DbSet<Team> Teams { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Country>().HasIndex(x => x.Name).IsUnique();

        //valida que el nombre del equipo sea unico por pais, es decir, no puede haber dos equipos con el mismo nombre en el mismo pais, pero si puede haber dos equipos con el mismo nombre en paises diferentes
        modelBuilder.Entity<Team>().HasIndex(x => new { x.CountryId, x.Name }).IsUnique();

        // deshabilita el borrado en cascada, es decir, si se borra un pais, no se borran sus equipos, sino que se lanza una excepcion, esto es para evitar que se borren datos por accidente
        DisableCascadingDelete(modelBuilder);
    }

    private void DisableCascadingDelete(ModelBuilder modelBuilder)
    {
        var relationships = modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys());
        foreach (var relationship in relationships)
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}