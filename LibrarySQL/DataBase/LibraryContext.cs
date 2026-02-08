using LibrarySQL.Books;
using LibrarySQL.Users;
using Microsoft.EntityFrameworkCore;

namespace LibrarySQL.DataBase;

/// <summary>
/// Represents the Entity Framework database context for the library application, providing access to books and users
/// stored in the database.
/// </summary>
public class LibraryContext : DbContext
{
    /// <summary>
    /// Gets or sets the collection of books in the database context.
    /// </summary>
    public DbSet<Book> Books { get; set; }

    /// <summary>
    /// Gets or sets the collection of users in the database context.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Configures the database context to use a SQL Server database with the specified connection string.
    /// </summary>
    /// <param name="optionsBuilder">The builder used to configure options for the database context, such as the database provider and connection
    /// string.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=LibraryDb;Trusted_Connection=True;");
    }
}
