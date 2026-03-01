using MusicShop.Data;
using Wpf.Ui.Controls;

namespace MusicShop;

/// <summary>
/// Interaction logic for AddSongs.xaml
/// </summary>
public partial class AddSongWindow : FluentWindow
{
    public AddSongWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Handles the click event for the Save button, validating input and saving a new song to the database.
    /// </summary>
    /// <param name="sender">The source of the event, typically the Save button that was clicked.</param>
    /// <param name="e">The event data associated with the click event.</param>
    private async void SaveButton_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TitleBox.Text))
        {
            await ShowMessageAsync("Please enter a song title.", "Validation Error");
            return;
        }

        if (string.IsNullOrWhiteSpace(ArtistBox.Text))
        {
            await ShowMessageAsync("Please enter the artist name.", "Validation Error");
            return;
        }

        if (!double.TryParse(PriceBox.Text, out var price) || price < 0)
        {
            await ShowMessageAsync("Please enter a valid price (must be a positive number).", "Invalid Price");
            return;
        }

        var newSong = new Song
        {
            Title = TitleBox.Text,
            Artist = ArtistBox.Text,
            Album = AlbumBox.Text,
            Price = price
        };

        using var db = new MusicContext();
        {
            db.Songs.Add(newSong);
            await db.SaveChangesAsync();
        }

        DialogResult = true;
        Close();
    }

    /// <summary>
    /// Handles the click event for the Cancel button, dismissing the dialog without applying any changes.
    /// </summary>
    /// <param name="sender">The source of the event, typically the Cancel button that was clicked.</param>
    /// <param name="e">An <see cref="EventArgs"/> instance containing data related to the click event.</param>
    private async void CancelButton_Click(object sender, EventArgs e)
    {
        DialogResult = false;
        Close();
    }

    /// <summary>
    /// Displays a message box asynchronously with the specified message and title.
    /// </summary>
    /// <param name="message">The message to display in the message box.</param>
    /// <param name="title">The title of the message box window.</param>
    /// <returns>A task that represents the asynchronous operation of showing the message box.</returns>
    private static async Task ShowMessageAsync(string message, string title)
    {
        var box = new MessageBox()
        {
            Title = title,
            Content = message
        };
        await box.ShowDialogAsync();
    }
}
