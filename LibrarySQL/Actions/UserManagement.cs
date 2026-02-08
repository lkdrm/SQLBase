using LibrarySQL.DataBase;
using LibrarySQL.Users;

namespace LibrarySQL.Actions;

/// <summary>
/// Provides static methods for managing user records within the library system.
/// </summary>
public static class UserManagement
{
    /// <summary>
    /// Registers a new user in the database using interactive console input.
    /// </summary>
    /// <param name="db">The database context used to add the new user. Must not be null and should be connected to a valid database
    /// instance.</param>
    public static void AddUser(LibraryContext db)
    {
        Console.WriteLine("\n--- Registration new user ---");
        Console.WriteLine("Enter users name: ");

        string userName = Console.ReadLine();

        var newUser = new User { Name = userName };

        db.Users.Add(newUser);
        db.SaveChanges();

        Console.WriteLine($"User '{userName}' has been added");
        Console.ReadLine();
    }

    /// <summary>
    /// Displays a list of all users in the specified library database to the console.
    /// </summary>
    /// <param name="db">The database context containing the user records to display. Cannot be null.</param>
    public static void ShowAllUsers(LibraryContext db)
    {
        Console.WriteLine("\n--- List of users ---");
        var users = db.Users.ToList();

        foreach (var user in users)
        {
            Console.WriteLine($"ID: {user.Id} | Name: {user.Name}");
        }
        Console.ReadLine();
    }
}
