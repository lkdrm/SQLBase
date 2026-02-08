using LibrarySQL.Users;

namespace LibrarySQL.Books;

/// <summary>
/// Represents a book in the library system, including its identification, status, and borrowing information.
/// </summary>
public class Book
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title associated with the object.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the current status of the book.
    /// </summary>
    public BookStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the borrower associated with the entity.
    /// </summary>
    public int? BorrowerId { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the item was borrowed.
    /// </summary>
    public DateTime? BorrowedDate { get; set; }
    
    /// <summary>
    /// Gets or sets the user who has borrowed the item.
    /// </summary>
    public User Borrower { get; set; }
}
