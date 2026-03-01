namespace MusicShop.Data;

/// <summary>
/// Represents a musical composition with associated metadata such as title, artist, album, and price.
/// </summary>
public class Song
{
    /// <summary>
    /// Gets or sets the unique identifier for the song.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Gets or sets the title of the song.
    /// </summary>
    public string Title { get; set; }
    
    /// <summary>
    /// Gets or sets the artist who performed the song.
    /// </summary>
    public string Artist { get; set; }
    
    /// <summary>
    /// Gets or sets the album name that contains the song.
    /// </summary>
    public string Album { get; set; }
    
    /// <summary>
    /// Gets or sets the price of the song in the music shop.
    /// </summary>
    public double Price { get; set; }
}
