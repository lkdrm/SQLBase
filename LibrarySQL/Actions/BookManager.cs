using LibrarySQL.Books;
using LibrarySQL.DataBase;
using Microsoft.EntityFrameworkCore;

namespace LibrarySQL.Actions;

/// <summary>
/// Provides static methods for managing books within a library context, including adding, listing, borrowing,
/// returning, and updating the status of books.
/// </summary>
public static class BookManager
{
    /// <summary>
    /// Adds a new book to the library database by prompting the user for a book title.
    /// </summary>
    /// <param name="db">The database context used to add the new book. Must not be null and should be connected to the library's data
    /// store.</param>
    public static void AddBook(LibraryContext db)
    {
        Console.WriteLine("\n--- Adding new book ---");
        Console.WriteLine("Enter a book name: ");
        string title = Console.ReadLine();

        var newBook = new Book
        {
            Title = title,
            Status = BookStatus.Available,
            BorrowedDate = null
        };

        db.Books.Add(newBook);
        db.SaveChanges();

        Console.WriteLine($"The book '{title}' has been saved.");
        Console.WriteLine("Press 'Enter' to continue...");
        Console.ReadLine();
    }

    /// <summary>
    /// Displays a list of all books in the library to the console.
    /// </summary>
    /// <param name="db">The database context containing the collection of books to display. Cannot be null.</param>
    public static void ShowAllBooks(LibraryContext db)
    {
        Console.WriteLine("\n--- The list of books ---");

        var books = db.Books.ToList();

        if (books.Count == 0)
        {
            Console.WriteLine("The library is empty.");
        }
        else
        {
            foreach (var book in books)
            {
                var borrowed = book.Status == BookStatus.Borrowed ? book.Borrower.Name : "No";
                Console.WriteLine($"{book.Id}. {book.Title} -- Status: {book.Status} -- Borrowed { borrowed }");
            }
        }

        Console.WriteLine("\nPress 'Enter' back to main menu...");
        Console.ReadLine();
    }

    /// <summary>
    /// Changes the status of a book in the library database by prompting the user for input.
    /// </summary>
    /// <param name="db">The database context used to access and update book records. Cannot be null.</param>
    public static void ChangeStatus(LibraryContext db)
    {
        Console.WriteLine("\n--- Change status of book ---");
        Console.WriteLine("Enter the ID of book: ");

        if (!int.TryParse(Console.ReadLine(), out int bookId))
        {
            return;
        }
        var book = db.Books.FirstOrDefault(b => b.Id == bookId);

        if (book == null)
        {
            Console.WriteLine($"Can't find book with ID: {bookId}");
            return;
        }

        Console.WriteLine($"Picked book: {book.Title}");
        Console.WriteLine($"Current book status: {book.Status}");

        Console.WriteLine("Pick a status of book:");
        Console.WriteLine("1 - Available");
        Console.WriteLine("2 - Borrowed");
        Console.WriteLine("3 - Waiting/Lost");

        var choice = Console.ReadLine();

        book.Status = choice switch
        {
            "1" => BookStatus.Available,
            "2" => BookStatus.Borrowed,
            "3" => BookStatus.Waiting,
            _ => book.Status,
        };

        db.SaveChanges();
        Console.WriteLine("Status has been updated!");
        Console.ReadKey();
    }

    /// <summary>
    /// Initiates the process for a user to borrow a book from the library by prompting for book and user IDs, updating
    /// the book's status, and saving changes to the database.
    /// </summary>
    /// <param name="db">The database context used to access and modify library books and users. Must not be null and should be properly
    /// initialized.</param>
    public static void BorrowBook(LibraryContext db)
    {
        Console.WriteLine("\n--- Borrowing book ---");

        Console.WriteLine("Enter the ID of Book");

        if (!int.TryParse(Console.ReadLine(), out int bookId))
        {
            return;
        }

        var book = db.Books.FirstOrDefault(b => b.Id == bookId);

        if (book == null)
        {
            Console.WriteLine($"Can't find current book: {bookId}");
            return;
        }

        if (book.Status != BookStatus.Available)
        {
            Console.WriteLine($"Book is not available. Current status: {book.Status}");
            return;
        }

        Console.WriteLine("Please enter the user ID: ");

        if (!int.TryParse(Console.ReadLine(), out int userId))
        {
            return;
        }

        var user = db.Users.FirstOrDefault(u => u.Id == userId);

        if (user == null)
        {
            Console.WriteLine($"Can't find user ID: {userId}");
            return;
        }

        book.BorrowerId = userId;
        book.Status = BookStatus.Borrowed;
        book.BorrowedDate = DateTime.Now;

        db.SaveChanges();

        Console.WriteLine($"The book '{book.Title}' has been borrowed to user: {user.Name}");
        Console.ReadLine();
    }

    /// <summary>
    /// Processes the return of a borrowed book in the library system by updating its status and borrower information.
    /// </summary>
    /// <param name="db">The database context used to access and update book records. Cannot be null.</param>
    public static void ReturnBook(LibraryContext db)
    {
        Console.WriteLine("\n--- Book returning ---");
        Console.WriteLine("Enter the book ID: ");
        if (!int.TryParse(Console.ReadLine(), out int bookId))
        {
            return;
        }

        var book = db.Books.Include(b => b.Borrower).FirstOrDefault(b => b.Id == bookId);

        if (book == null)
        {
            Console.WriteLine($"Can't find the current ID of book: {bookId}");
            return;
        }

        if (book.Borrower == null)
        {
            Console.WriteLine($"The book '{book.Title}' is not currently borrowed.");
            return;
        }

        Console.WriteLine($"The book '{book.Title}' has been returned by user: {book.Borrower.Name}");

        book.Borrower = null;
        book.BorrowerId = null;
        book.Status = BookStatus.Available;
        book.BorrowedDate = null;

        db.SaveChanges();
        Console.ReadLine();
    }
}
