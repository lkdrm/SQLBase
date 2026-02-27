using Microsoft.EntityFrameworkCore;
using MusicShop.Data;
using System.Windows;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace MusicShop
{
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

        private async Task LoadSongsAsync()
        {
            using var db = new MusicContext();
            await db.Database.EnsureCreatedAsync();

            var songs = await db.Songs.ToListAsync();

            SongsGrid.ItemsSource = songs;
        }

        private async void AddSongs_Click(object sender, RoutedEventArgs e)
        {
            using var db = new MusicContext();
            if (!await db.Songs.AnyAsync())
            {
                db.Songs.Add(new Song { Title = "Bohemian Rhapsody", Artist = "Queen", Album = "A Night at the Opera", Price = 1.99 });
                db.Songs.Add(new Song { Title = "Shape of You", Artist = "Ed Sheeran", Album = "Divide", Price = 1.29 });
                db.Songs.Add(new Song { Title = "Ой у лузі червона калина", Artist = "Бумбокс", Album = "Single", Price = 0.99 });

                await db.SaveChangesAsync();

                await LoadSongsAsync();
                await ShowMessageAsync("Songs successfully added", "Song add");
            }
            else
            {
                await ShowMessageAsync("Is in the data base", "DataBase");
            }
        }

        private async void BuySong_click(object sender, RoutedEventArgs e)
        {
            if (SongsGrid.SelectedItem is Song selectedSong)
            {
                await ShowMessageAsync($"U purchase: {selectedSong.Artist} - {selectedSong.Title}\n You paid: {selectedSong.Price}$", "Buy");
            }
            else
            {
                await ShowMessageAsync("Pick song from the list", "Buy");
            }
        }

        private async void ThemeButton_Click(object sender, RoutedEventArgs e)
        {
            var currentTheme = ApplicationThemeManager.GetAppTheme();
            ApplicationThemeManager.Apply(currentTheme == ApplicationTheme.Dark ? ApplicationTheme.Light : ApplicationTheme.Dark);
        }

        private async void SearchBox_TextChanged(object sender, RoutedEventArgs e)
        {
            string searchText = SearchBox.Text.ToLower().Trim();

            using var db = new MusicContext();
            var allsongs = await db.Songs.ToListAsync();

            var filtertedSongs = allsongs.Where(s => s.Title.ToLower().Contains(searchText) || s.Artist.ToLower().Contains(searchText));

            SongsGrid.ItemsSource = filtertedSongs;
        }

        private static async Task ShowMessageAsync(string message, string title)
        {
            var box = new Wpf.Ui.Controls.MessageBox()
            {
                Title = title,
                Content = message
            };
            await box.ShowDialogAsync();
        }
    }
}