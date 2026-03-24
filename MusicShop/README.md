# 🎵 MusicShop

A Windows desktop application for browsing, managing, and purchasing songs. Built with WPF and a modern Fluent Design interface.

---

## 📖 What Does This App Do?

MusicShop is a music catalog manager with a built-in shopping cart. It lets you maintain a database of songs and simulate purchasing them, generating a receipt file at the end of each transaction.

Key capabilities:

- View the full song catalog in a data grid
- Search songs in real time by title or artist
- Add, edit, and delete songs from the catalog
- Add songs to a shopping cart and manage quantities
- Complete a purchase and automatically save a receipt as a `.txt` file
- Switch between **Light** and **Dark** themes

---

## 🚀 How to Use

### Prerequisites

- Windows 10 or later
- [.NET 10 Runtime](https://dotnet.microsoft.com/download/dotnet/10.0)
- SQL Server LocalDB (included with Visual Studio)

### Running the App

1. Clone or download the repository.
2. Open `MusicShop.sln` in Visual Studio 2026.
3. Build and run the project (`F5`).
4. The database (`MusicShopDb`) is created automatically on first launch via LocalDB.

---

## 🖥️ Application Windows

### Main Window

The central hub of the application.

| Button | Action |
|--------|--------|
| ➕ **Add Song** | Opens the Add Song dialog to insert a new song into the catalog |
| ✏️ **Edit Song** | Opens the Edit Song dialog for the selected row |
| 🗑️ **Delete Song** | Prompts for confirmation, then removes the selected song |
| 🛒 **Buy Song** | Adds the selected song to the shopping cart |
| 🛒 **Cart button (top right)** | Opens the Shopping Cart window; shows the current item count |
| 🌙 **Theme toggle** | Switches between Light and Dark mode |
| 🔍 **Search box** | Filters the catalog live by song title or artist name |

---

### Add Song Window

Fill in the fields and click **Save** to add a new song to the catalog.

| Field | Required | Notes |
|-------|----------|-------|
| Title | ✅ Yes | Song title |
| Artist | ✅ Yes | Performer name |
| Album | ❌ No | Album name |
| Price | ✅ Yes | Must be a positive number |

Click **Cancel** to close without saving.

---

### Edit Song Window

Pre-populated with the selected song's current data. Modify any field and click **Edit** to save changes, or **Cancel** to discard them.

---

### Shopping Cart Window

Displays all songs added to the cart with quantity controls.

| Control | Action |
|---------|--------|
| ➕ **+** button | Increases the quantity of that item by 1 |
| ➖ **-** button | Decreases the quantity of the selected item (minimum 1) |
| ❌ **Delete** button | Removes the item from the cart entirely |
| 💳 **Buy** button | Completes the purchase, shows a summary, saves a receipt file, and clears the cart |
| **Cancel** | Closes the cart window without purchasing |

After a successful purchase, a receipt file named `Receipt_DD-MM-YYYY_HH-mm.txt` is saved in the application directory.

---

## 🧾 Receipt Format

```
--- Music Shop Receipt ---
Date: 27-06-2025_14-30
--------------------------
Bohemian Rhapsody (A Night at the Opera) - 1 pc. - 1.99$
Hotel California (Hotel California) - 2 pc. - 3.98$
Total: 5.97$
```

---

## 🛠️ Technologies

| Technology | Version |
|------------|---------|
| [.NET](https://dotnet.microsoft.com/) | 10.0 |
| [C#](https://learn.microsoft.com/en-us/dotnet/csharp/) | 14.0 |
| [WPF (Windows Presentation Foundation)](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/) | .NET 10 |
| [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/) | 10.0.3 |
| [EF Core SQL Server Provider](https://learn.microsoft.com/en-us/ef/core/providers/sql-server/) | 10.0.3 |
| [SQL Server LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) | Bundled with VS |
| [WPF-UI (Fluent Design)](https://github.com/lepoco/wpfui) | 4.2.0 |

---

## 📁 Project Structure

```
MusicShop/
├── Data/
│   ├── MusicContext.cs        # EF Core DbContext (SQL Server LocalDB)
│   ├── Song.cs                # Song entity model
│   └── ShoppingCartItem.cs    # Cart item with INotifyPropertyChanged
├── MainWindow.xaml(.cs)       # Catalog view with CRUD + cart
├── AddSongWindow.xaml(.cs)    # Dialog to add a new song
├── EditSongWindow.xaml(.cs)   # Dialog to edit an existing song
├── ShoppingCartWindow.xaml(.cs) # Shopping cart and checkout
├── App.xaml(.cs)              # Application entry point
└── MusicShop.csproj           # Project file (net10.0-windows)
```

---

## 📝 Version

**v1.0** — Initial release
