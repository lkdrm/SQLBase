using Microsoft.EntityFrameworkCore;
using MusicShop.Data;
using System.Windows;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace MusicShop;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : FluentWindow
{
    public MainWindow()
    {
        InitializeComponent();
        Loaded += async (s, e) => await LoadSongsAsync();
    }

    /// <summary>
    /// Asynchronously loads the list of songs from the database and binds it to the SongsGrid control.
    /// </summary>
    /// <returns></returns>
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
    private async void BuySong_click(object sender, RoutedEventArgs e)
    {
        if (SongsGrid.SelectedItem is Song selectedSong)
        {
            await ShowMessageAsync($"You have purchased:\n{selectedSong.Artist} - {selectedSong.Title}\n\nPrice: ${selectedSong.Price:F2}", "Purchase Successful");
        }
        else
        {
            await ShowMessageAsync("Please select a song from the list to purchase.", "No Song Selected");
        }
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
}