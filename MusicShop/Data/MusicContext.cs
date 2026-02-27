using Microsoft.EntityFrameworkCore;

namespace MusicShop.Data;

public class MusicContext : DbContext
{
    public DbSet<Song> Songs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)=> optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MusicShopDb;Trusted_Connection=True;");
}
