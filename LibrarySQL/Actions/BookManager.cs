using LibrarySQL.Books;
using LibrarySQL.DataBase;
using Microsoft.EntityFrameworkCore;
using LibrarySQL.UI;

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
        ConsoleHelper.PrintHeader("\n--- Adding new book ---");
        string title = ConsoleHelper.ReadInput("Enter a book name: ");

        var newBook = new Book
        {
            Title = title,
            Status = BookStatus.Available,
            BorrowedDate = null
        };

        db.Books.Add(newBook);
        db.SaveChanges();

        ConsoleHelper.PrintSuccess($"The book '{title}' has been saved.");
        ConsoleHelper.PrintInfo("Press 'Enter' to continue...");
        Console.ReadLine();
    }

    /// <summary>
    /// Displays a list of all books in the library to the console.
    /// </summary>
    /// <param name="db">The database context containing the collection of books to display. Cannot be null.</param>
    public static void ShowAllBooks(LibraryContext db)
    {
        ConsoleHelper.PrintHeader("\n--- The list of books ---");

        var books = db.Books.ToList();

        if (books.Count == 0)
        {
            ConsoleHelper.PrintInfo("The library is empty.");
        }
        else
        {
            foreach (var book in books)
            {
                var borrowed = book.Status == BookStatus.Borrowed ? book.Borrower?.Name : "No";
                ConsoleHelper.PrintInfo($"{book.Id}. {book.Title} -- Status: {book.Status} -- Borrowed { borrowed }");
            }
        }

        ConsoleHelper.PrintInfo("\nPress 'Enter' back to main menu...");
        Console.ReadLine();
    }

    /// <summary>
    /// Changes the status of a book in the library database by prompting the user for input.
    /// </summary>
    /// <param name="db">The database context used to access and update book records. Cannot be null.</param>
    public static void ChangeStatus(LibraryContext db)
    {
        ConsoleHelper.PrintHeader("\n--- Change status of book ---");

        if (!int.TryParse(ConsoleHelper.ReadInput("Enter the ID of book: "), out int bookId))
        {
            return;
        }
        var book = db.Books.FirstOrDefault(b => b.Id == bookId);

        if (book == null)
        {
            ConsoleHelper.PrintError($"Can't find book with ID: {bookId}");
            return;
        }

        ConsoleHelper.PrintInfo($"Picked book: {book.Title}");
        ConsoleHelper.PrintInfo($"Current book status: {book.Status}");

        ConsoleHelper.PrintHeader("Pick a status of book:");
        ConsoleHelper.PrintMenuOption("1 - Available");
        ConsoleHelper.PrintMenuOption("2 - Borrowed");
        ConsoleHelper.PrintMenuOption("3 - Waiting/Lost");

        var choice = Console.ReadLine();

        book.Status = choice switch
        {
            "1" => BookStatus.Available,
            "2" => BookStatus.Borrowed,
            "3" => BookStatus.Waiting,
            _ => book.Status,
        };

        db.SaveChanges();
        ConsoleHelper.PrintSuccess("Status has been updated!");
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
        ConsoleHelper.PrintHeader("\n--- Borrowing book ---");

        if (!int.TryParse(ConsoleHelper.ReadInput("Enter the ID of Book: "), out int bookId))
        {
            return;
        }

        var book = db.Books.FirstOrDefault(b => b.Id == bookId);

        if (book == null)
        {
            ConsoleHelper.PrintError($"Can't find current book: {bookId}");
            return;
        }

        if (book.Status != BookStatus.Available)
        {
            ConsoleHelper.PrintError($"Book is not available. Current status: {book.Status}");
            return;
        }

        if (!int.TryParse(ConsoleHelper.ReadInput("Please enter the user ID: "), out int userId))
        {
            return;
        }

        var user = db.Users.FirstOrDefault(u => u.Id == userId);

        if (user == null)
        {
            ConsoleHelper.PrintError($"Can't find user ID: {userId}");
            return;
        }

        book.BorrowerId = userId;
        book.Status = BookStatus.Borrowed;
        book.BorrowedDate = DateTime.Now;

        db.SaveChanges();

        ConsoleHelper.PrintSuccess($"The book '{book.Title}' has been borrowed to user: {user.Name}");
        Console.ReadLine();
    }

    /// <summary>
    /// Processes the return of a borrowed book in the library system by updating its status and borrower information.
    /// </summary>
    /// <param name="db">The database context used to access and update book records. Cannot be null.</param>
    public static void ReturnBook(LibraryContext db)
    {
        ConsoleHelper.PrintHeader("\n--- Book returning ---");
        if (!int.TryParse(ConsoleHelper.ReadInput("Enter the book ID: "), out int bookId))
        {
            return;
        }

        var book = db.Books.Include(b => b.Borrower).FirstOrDefault(b => b.Id == bookId);

        if (book == null)
        {
            ConsoleHelper.PrintError($"Can't find the current ID of book: {bookId}");
            return;
        }

        if (book.Borrower == null)
        {
            ConsoleHelper.PrintError($"The book '{book.Title}' is not currently borrowed.");
            return;
        }

        ConsoleHelper.PrintSuccess($"The book '{book.Title}' has been returned by user: {book.Borrower.Name}");

        book.Borrower = null;
        book.BorrowerId = null;
        book.Status = BookStatus.Available;
        book.BorrowedDate = null;

        db.SaveChanges();
        Console.ReadLine();
    }
}
