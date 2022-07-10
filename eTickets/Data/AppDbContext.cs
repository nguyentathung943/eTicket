using eTickets.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace eTickets.Data;
using Microsoft.EntityFrameworkCore;

// DB Context for Data Relationships
public class AppDbContext: IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Actor_Movie>()
            .HasKey(am => new { am.ActorId, am.MovieId });

        modelBuilder.Entity<Actor_Movie>()
            .HasOne(m => m.Movie)
            .WithMany(am => am.Actors_Movies)
            .HasForeignKey(m => m.MovieId);
        
        modelBuilder.Entity<Actor_Movie>()
            .HasOne(a => a.Actor)
            .WithMany(am => am.Actors_Movies)
            .HasForeignKey(a => a.ActorId);

        base.OnModelCreating(modelBuilder);
    }
    
    public DbSet<Actor> Actors { get; set; }
        
    public DbSet<Movie> Movies { get; set; }
        
    public DbSet<Actor_Movie> Actors_Movies { get; set; }
        
    public DbSet<Cinema> Cinemas { get; set; }
        
    public DbSet<Producer> Producers { get; set; }
    
    public DbSet<Order> Orders { get; set; }
    
    public DbSet<OrderItem> OrderItems { get; set; }
    
    public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
}