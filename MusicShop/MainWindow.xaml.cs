using Microsoft.EntityFrameworkCore;
using MusicShop.Data;
using System.ComponentModel;
using System.Windows;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace MusicShop;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : FluentWindow, INotifyPropertyChanged
{
    /// <summary>
    /// Gets the collection of items currently in the shopping cart, allowing for data binding and manipulation of the cart's contents.
    /// </summary>
    private readonly List<ShoppingCartItem> shoppingCart = [];

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Gets or sets the count of items currently in the shopping cart, providing a property for data binding to display the number of items in the cart.
    /// </summary>
    private int _cartItemsCount = 0;

    /// <summary>
    /// Gets or sets the total number of items currently in the shopping cart.
    /// </summary>
    public int CartItemsCount
    {
        get => _cartItemsCount;
        set
        {
            _cartItemsCount = value;
            OnPropertyChanged("CartItemsCount");
        }
    }

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        Loaded += async (s, e) => await LoadSongsAsync();
    }

    /// <summary>
    /// Asynchronously loads the list of songs from the database and binds it to the SongsGrid control.
    /// </summary>
    private async Task LoadSongsAsync()
    {
        using var db = new MusicContext();
        await db.Database.EnsureCreatedAsync();

        var songs = await db.Songs.ToListAsync();

        SongsGrid.ItemsSource = songs;
    }

    /// <summary>
    /// Handles the click event to add a new song by displaying a dialog for song entry and updating the song list upon
    /// confirmation.
    /// </summary>
    /// <param name="sender">The source of the event, typically the button that was clicked.</param>
    /// <param name="e">The event data associated with the click event.</param>
    private async void AddSong_Click(object sender, RoutedEventArgs e)
    {
        var songWindow = new AddSongWindow();
        songWindow.Owner = this;

        if (songWindow.ShowDialog() == true)
        {
            await LoadSongsAsync();
            await ShowMessageAsync("The song has been successfully added to the catalog.", "Song Added");
        }
    }

    /// <summary>
    /// Handles the click event for purchasing a song by displaying a confirmation message with the selected song's
    /// details and price.
    /// </summary>
    /// <param name="sender">The source of the event, typically the button or UI element that was clicked.</param>
    /// <param name="e">The event data associated with the click event.</param>
    private async void BuySong_Click(object sender, RoutedEventArgs e)
    {
        if (SongsGrid.SelectedItem is Song selectedSong)
        {
            var existingSong = shoppingCart.FirstOrDefault(item => item.Product.Id == selectedSong.Id);
            if (existingSong != null)
            {
                existingSong.Quantity++;
                CartItemsCount = shoppingCart.Sum(item => item.Quantity);
            }
            else
            {
                var newSong = new ShoppingCartItem() { Product = selectedSong, Quantity = 1 };
                shoppingCart.Add(newSong);
                CartItemsCount++;
            }
        }
        else
        {
            await ShowMessageAsync("Please select a song from the list to purchase.", "No Song Selected");
        }
    }

    /// <summary>
    /// Handles the click event for the cart items button, displaying the shopping cart window and updating the cart
    /// item count.
    /// </summary>
    /// <param name="sender">The source of the event, typically the button that was clicked.</param>
    /// <param name="e">The event data associated with the click event.</param>
    private async void CartItemsButton_Click(object sender, RoutedEventArgs e)
    {
        var shoppingCartWindow = new ShoppingCartWindow(shoppingCart);
        shoppingCartWindow.ShowDialog();
        shoppingCartWindow.Owner = this;
        CartItemsCount = shoppingCart.Sum(item => item.Quantity);
    }

    /// <summary>
    /// Handles the click event for the theme toggle button, switching the application theme between light and dark
    /// modes.
    /// </summary>
    /// <param name="sender">The source of the event, typically the button that was clicked.</param>
    /// <param name="e">The event data associated with the click event.</param>
    private async void ThemeButton_Click(object sender, RoutedEventArgs e)
    {
        var currentTheme = ApplicationThemeManager.GetAppTheme();
        ApplicationThemeManager.Apply(currentTheme == ApplicationTheme.Dark ? ApplicationTheme.Light : ApplicationTheme.Dark);
    }

    /// <summary>
    /// Handles the click event to delete the selected song from the music library after user confirmation.
    /// </summary>
    /// <param name="sender">The source of the event, typically the button or UI element that was clicked.</param>
    /// <param name="e">The event data associated with the click event.</param>
    private async void DeleteSong_Click(object sender, RoutedEventArgs e)
    {
        if (SongsGrid.SelectedItem is Song selectedSong)
        {
            var result = await ShowMessageBoxResultAsync($"Are you sure you want to delete '{selectedSong.Title}' by {selectedSong.Artist}?", "Confirm Deletion");

            if (result == Wpf.Ui.Controls.MessageBoxResult.Primary)
            {
                using var db = new MusicContext();
                var songToDelete = await db.Songs.FindAsync(selectedSong.Id);

                if (songToDelete != null)
                {
                    db.Songs.Remove(songToDelete);
                    await db.SaveChangesAsync();
                    await LoadSongsAsync();
                    await ShowMessageAsync("The song has been successfully deleted.", "Song Deleted");
                }
            }
        }
        else
        {
            await ShowMessageAsync("Please select a song from the list to delete.", "No Song Selected");
        }
    }

    /// <summary>
    /// Handles the click event for editing the currently selected song in the songs grid.
    /// </summary>
    /// <param name="sender">The source of the event, typically the button that was clicked.</param>
    /// <param name="e">The event data associated with the click event.</param>
    private async void EditSong_Click(object sender, RoutedEventArgs e)
    {
        if (SongsGrid.SelectedItem == null)
        {
            await ShowMessageAsync("Chose song for update", "Song update");
        }
        else
        {
            EditSongWindow editSongWindow = new((Song)SongsGrid.SelectedItem);

            editSongWindow.Owner = this;
            if (editSongWindow.ShowDialog() == true)
            {
                await LoadSongsAsync();
                await ShowMessageAsync("The song has been successfully updated.", "Song Updated");
            }
        }
    }

    /// <summary>
    /// Handles the TextChanged event for the search box, updating the displayed list of songs to match the user's
    /// search input.
    /// </summary>
    /// <param name="sender">The source of the event, typically the search box control that triggered the event.</param>
    /// <param name="e">The event data associated with the TextChanged event.</param>
    private async void SearchBox_TextChanged(object sender, RoutedEventArgs e)
    {
        string searchText = SearchBox.Text.ToLower().Trim();

        using var db = new MusicContext();
        var allsongs = await db.Songs.ToListAsync();

        var filtertedSongs = allsongs.Where(s => s.Title.ToLower().Contains(searchText) || s.Artist.ToLower().Contains(searchText));

        SongsGrid.ItemsSource = filtertedSongs;
    }

    /// <summary>
    /// Displays a message box dialog asynchronously with the specified message and title.
    /// </summary>
    /// <param name="message">The text to display in the body of the message box dialog.</param>
    /// <param name="title">The title to display in the message box window.</param>
    /// <returns>A task that represents the asynchronous operation of showing the message box dialog.</returns>
    private static async Task ShowMessageAsync(string message, string title)
    {
        var box = new Wpf.Ui.Controls.MessageBox()
        {
            Title = title,
            Content = message
        };
        await box.ShowDialogAsync();
    }

    /// <summary>
    /// Displays a message box with the specified content and title, allowing the user to choose between deleting or
    /// canceling the action.
    /// </summary>
    /// <param name="message">The message to display in the message box. Provides context for the user's decision.</param>
    /// <param name="title">The title of the message box, shown at the top of the dialog to indicate its purpose.</param>
    /// <returns>A <see cref="Wpf.Ui.Controls.MessageBoxResult"/> value indicating the user's selection, either 'Delete' or
    /// 'Cancel'.</returns>
    private static async Task<Wpf.Ui.Controls.MessageBoxResult> ShowMessageBoxResultAsync(string message, string title)
    {
        var box = new Wpf.Ui.Controls.MessageBox()
        {
            Title = title,
            Content = message,
            PrimaryButtonText = "Delete",
            SecondaryButtonText = "Cancel"
        };
        return box.ShowDialogAsync().Result;
    }

    /// <summary>
    /// Raises the PropertyChanged event to notify listeners that a property value has changed.
    /// </summary>
    /// <param name="name">The name of the property that changed. Cannot be null or empty.</param>
    protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}