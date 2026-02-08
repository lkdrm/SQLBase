using LibrarySQL.Actions;
using LibrarySQL.DataBase;
using Microsoft.EntityFrameworkCore;
using LibrarySQL.UI;
Console.Title = "SQL Library";
using var db = new LibraryContext();

db.Database.EnsureCreated();

while (true)
{
    Console.Clear();
    ConsoleHelper.PrintHeader("=== Library: Main menu ===");
    ConsoleHelper.PrintMenuOption("1. [Book] Add new book");
    ConsoleHelper.PrintMenuOption("2. [Book] List of books");
    ConsoleHelper.PrintMenuOption("3. [Book] Change status of book");
    ConsoleHelper.PrintMenuOption("4. [People] Add user");
    ConsoleHelper.PrintMenuOption("5. [People] List of Users");
    ConsoleHelper.PrintMenuOption("6. [Operation] Borrow book");
    ConsoleHelper.PrintMenuOption("7. [Operation] Book returning");
    ConsoleHelper.PrintMenuOption("8. Exit");

    var input = ConsoleHelper.ReadInput("\nYour choice: ");

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
            ConsoleHelper.PrintError("Unknown command.");
            break;
    }
}
