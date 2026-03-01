using Microsoft.EntityFrameworkCore;

namespace MusicShop.Data;

/// <summary>
/// Represents the Entity Framework database context for managing music-related data, including songs.
/// </summary>
public class MusicContext : DbContext
{
    /// <summary>
    /// Gets or sets the DbSet of songs in the music shop database.
    /// </summary>
    public DbSet<Song> Songs { get; set; }

    /// <summary>
    /// Configures the database context to use SQL Server with a LocalDB connection.
    /// </summary>
    /// <param name="optionsBuilder">The builder being used to configure the context.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)=> optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MusicShopDb;Trusted_Connection=True;");
}
