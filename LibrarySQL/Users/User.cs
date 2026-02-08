using LibrarySQL.Books;

namespace LibrarySQL.Users;

/// <summary>
/// Represents a library user who can borrow books.
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name associated with the object.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the collection of books currently borrowed by the user.
    /// </summary>
    public List<Book> BorrowedBooks { get; set; } = [];
}
