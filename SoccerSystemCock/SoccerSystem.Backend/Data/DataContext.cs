using SoccerSystem.Shared.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SoccerSystem.Backend.Data;

public class DataContext : IdentityDbContext<User>
{
    //: IdentityDbContext<User> se utiliza para que el contexto de datos herede de IdentityDbContext, lo que permite utilizar las funcionalidades de autenticacion y autorizacion de ASP.NET Core Identity, como la gestion de usuarios, roles, claims, etc. Si no se heredara de IdentityDbContext, se tendria que implementar toda la logica de autenticacion y autorizacion manualmente, lo que seria mucho mas complicado y propenso a errores.
    // por eso no se coloca abajo en el public DbSet<User> Users { get; set; } porque ya esta incluido en IdentityDbContext, y si se colocara, se tendria que configurar manualmente la relacion entre User y las tablas de Identity, lo que seria innecesario y podria causar problemas de compatibilidad.
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Country> Countries { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<TournamentTeam> TournamentTeams { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Country>().HasIndex(x => x.Name).IsUnique();
        modelBuilder.Entity<Tournament>().HasIndex(x => x.Name).IsUnique();

        //valida que el nombre del equipo sea unico por pais, es decir, no puede haber dos equipos con el mismo nombre en el mismo pais, pero si puede haber dos equipos con el mismo nombre en paises diferentes
        modelBuilder.Entity<Team>().HasIndex(x => new { x.CountryId, x.Name }).IsUnique();
        modelBuilder.Entity<TournamentTeam>().HasIndex(x => new { x.TournamentId, x.TeamId }).IsUnique();

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