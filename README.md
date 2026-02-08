# LibrarySQL - Library Management System

A console-based library management system built with C# and Entity Framework Core for managing books and users.

## Features

### Book Management
- **Add New Book** - Add books to the library catalog
- **List Books** - View all books with their current status
- **Change Book Status** - Update book status (Available, Borrowed, Waiting)

### User Management
- **Add User** - Register new library users
- **List Users** - View all registered users

### Operations
- **Borrow Book** - Allow users to borrow available books
- **Return Book** - Process book returns and update status

## Technology Stack

- **.NET 10**
- **C# 14.0**
- **Entity Framework Core** - ORM for database operations
- **SQL Server (LocalDB)** - Database engine

## Project Structure

```
LibrarySQL/
├── Actions/
│   ├── BookManager.cs      # Book-related operations
│   └── UserManagement.cs   # User-related operations
├── Books/
│   ├── Book.cs            # Book entity model
│   └── BookStatus.cs      # Book status enumeration
├── DataBase/
│   └── LibraryContext.cs  # EF Core DbContext
├── Users/
│   └── User.cs            # User entity model
└── Program.cs             # Application entry point
```

## Database Schema

### Book Entity
- `Id` - Unique identifier
- `Title` - Book title
- `Status` - Current status (Available, Borrowed, InOrder, Waiting)
- `BorrowerId` - Foreign key to User
- `BorrowedDate` - Date when book was borrowed
- `Borrower` - Navigation property to User

### User Entity
- `Id` - Unique identifier
- `Name` - User name
- `BorrowedBooks` - Collection of borrowed books

## Getting Started

### Prerequisites
- .NET 10 SDK
- SQL Server LocalDB

### Running the Application

1. Clone the repository:
```bash
git clone https://github.com/lkdrm/SQLBase
cd SQLBase
```

2. Run the application:
```bash
dotnet run
```

The database will be automatically created on first run using `EnsureCreated()`.

## Usage

When you run the application, you'll see the main menu:

```
=== Library: Main menu ===
1. [Book] Add new book
2. [Book] List of books
3. [Book] Change status of book
4. [People] Add user
5. [People] List of Users
6. [Operation] Borrow book
7. [Operation] Book returning
8. Exit
```

Select an option by entering the corresponding number.

## Database Configuration

The application uses SQL Server LocalDB with the following connection string:
```
Server=(localdb)\mssqllocaldb;Database=LibraryDb;Trusted_Connection=True;
```

You can modify the connection string in `DataBase/LibraryContext.cs`.

## Contributing

Feel free to fork this project and submit pull requests for any improvements.

## License

This project is for educational purposes.

