using MusicShop.Data;
using Wpf.Ui.Controls;

namespace MusicShop;

/// <summary>
/// Interaction logic for EditSongWindow.xaml
/// </summary>
public partial class EditSongWindow : FluentWindow
{
    /// <summary>
    /// Gets or sets the currently selected song.
    /// </summary>
    public Song SelectedSong { get; set; }

    /// <summary>
    /// Initializes a new instance of the EditSongWindow class for editing the specified song.
    /// </summary>
    /// <param name="selectedSong">The song to be edited. This parameter cannot be null.</param>
    public EditSongWindow(Song selectedSong)
    {
        InitializeComponent();
        SelectedSong = selectedSong;
        LoadSongDetails();
    }

    /// <summary>
    /// Handles the click event for the edit button, updating the song information in the database and closing the
    /// dialog upon completion.
    /// </summary>
    /// <param name="sender">The source of the event, typically the edit button that was clicked.</param>
    /// <param name="e">The event data associated with the click event.</param>
    private async void EditButton_Click(object sender, EventArgs e)
    {
        using var db = new MusicContext();
        await UpdateSongInformation(db);

        await db.SaveChangesAsync();

        DialogResult = true;
        Close();
    }

    /// <summary>
    /// Updates the information for the selected song in the database asynchronously.
    /// </summary>
    /// <param name="db">The database context used to access and modify song records.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    private async Task UpdateSongInformation(MusicContext db)
    {
        var songToUpdate = await db.Songs.FindAsync(SelectedSong.Id);

        songToUpdate.Title = TitleBox.Text;
        songToUpdate.Artist = ArtistBox.Text;
        songToUpdate.Album = AlbumBox.Text;
        songToUpdate.Price = double.Parse(PriceBox.Text);
    }

    /// <summary>
    /// Populates the UI fields with the details of the currently selected song.
    /// </summary>
    private void LoadSongDetails()
    {
        TitleBox.Text = SelectedSong.Title;
        ArtistBox.Text = SelectedSong.Artist;
        AlbumBox.Text = SelectedSong.Album;
        PriceBox.Text = SelectedSong.Price.ToString();
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
}
