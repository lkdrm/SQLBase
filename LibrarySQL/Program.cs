using LibrarySQL.Actions;
using LibrarySQL.DataBase;
using Microsoft.EntityFrameworkCore;

using var db = new LibraryContext();

db.Database.EnsureCreated();

while (true)
{
    Console.Clear();
    Console.WriteLine("=== Library: Main menu ===");
    Console.WriteLine("1. [Book] Add new book");
    Console.WriteLine("2. [Book] List of books");
    Console.WriteLine("3. [Book] Change status of book");
    Console.WriteLine("4. [People] Add user");
    Console.WriteLine("5. [People] List of Users");
    Console.WriteLine("6. [Operation] Borrow book");
    Console.WriteLine("7. [Operation] Book returning");
    Console.WriteLine("8. Exit");
    Console.WriteLine("\nYour choice: ");

    var input = Console.ReadLine();

    if (input == "8")
    {
        break;
    }

    switch (input)
    {
        case "1":
            BookManager.AddBook(db);
            break;
        case "2":
            BookManager.ShowAllBooks(db);
            break;
        case "3":
            BookManager.ChangeStatus(db);
            break;
        case "4":
            UserManagement.AddUser(db);
            break;
        case "5":
            UserManagement.ShowAllUsers(db);
            break;
        case "6":
            BookManager.BorrowBook(db);
            break;
        case "7":
            BookManager.ReturnBook(db);
            break;
        default:
            Console.WriteLine("Unknown command.");
            break;
    }
}