using MusicShop.Data;
using System.ComponentModel;
using System.IO;
using System.Text;
using Wpf.Ui.Controls;

namespace MusicShop;

/// <summary>
/// Interaction logic for CartWindow.xaml
/// </summary>
public partial class ShoppingCartWindow : FluentWindow, INotifyPropertyChanged
{
    /// <summary>
    /// Gets the collection of items currently in the shopping cart, allowing for data binding and manipulation of the cart's contents.
    /// </summary>
    private readonly List<ShoppingCartItem> _cartItems = [];

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Gets the total sum of all item prices in the cart.
    /// </summary>
    public double Sum
    {
        get => _cartItems.Sum(p => p.TotalPrice);
    }

    /// <summary>
    /// Initializes a new instance of the ShoppingCartWindow class with the specified shopping cart items.
    /// </summary>
    /// <param name="cartItems">The collection of items to display in the shopping cart.</param>
    public ShoppingCartWindow(List<ShoppingCartItem> cartItems)
    {
        InitializeComponent();
        _cartItems = cartItems;
        DataContext = this;
        Loaded += async (s, e) => await LoadSongsAsync();
    }

    /// <summary>
    /// Asynchronously loads the list of songs from the database and binds it to the SongsGrid control.
    /// </summary>
    private async Task LoadSongsAsync()
    {
        ItemsToBuy.ItemsSource = _cartItems;
    }

    /// <summary>
    /// Handles the click event for the Cancel button, dismissing the dialog without applying any changes.
    /// </summary>
    /// <param name="sender">The source of the event, typically the Cancel button that was clicked.</param>
    /// <param name="e">An <see cref="EventArgs"/> instance containing data related to the click event.</param>
    private async void MoreButton_Click(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            if (button.DataContext is ShoppingCartItem cartItem)
            {
                cartItem.Quantity++;
                OnPropertyChanged("Sum");
            }
        }
    }

    /// <summary>
    /// Handles the click event for the Cancel button, dismissing the dialog without applying any changes.
    /// </summary>
    /// <param name="sender">The source of the event, typically the Cancel button that was clicked.</param>
    /// <param name="e">An <see cref="EventArgs"/> instance containing data related to the click event.</param>
    private async void LessButton_Click(object sender, EventArgs e)
    {
        if (ItemsToBuy.SelectedItem is ShoppingCartItem cartItem)
        {
            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
                OnPropertyChanged("Sum");
            }
        }
    }

    /// <summary>
    /// Handles the click event for the Cancel button, dismissing the dialog without applying any changes.
    /// </summary>
    /// <param name="sender">The source of the event, typically the Cancel button that was clicked.</param>
    /// <param name="e">An <see cref="EventArgs"/> instance containing data related to the click event.</param>
    private async void DeleteItemButton_Click(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            if (button.DataContext is ShoppingCartItem cartItem)
            {
                _cartItems.Remove(cartItem);
                ItemsToBuy.Items.Refresh();
                OnPropertyChanged("Sum");
            }
        }
    }

    /// <summary>
    /// Handles the click event for the edit button, updating the song information in the database and closing the
    /// dialog upon completion.
    /// </summary>
    /// <param name="sender">The source of the event, typically the edit button that was clicked.</param>
    /// <param name="e">The event data associated with the click event.</param>
    private async void BuyButton_Click(object sender, EventArgs e)
    {
        if (_cartItems.Count > 0)
        {
            DialogResult = true;
            await ShowMessageAsync($"You have purchased:\n{_cartItems.Count}\n\nPrice: ${Sum:F2}", "Purchase Successful");
            CreateReceipt();
            _cartItems.Clear();
            ItemsToBuy.Items.Refresh();
            Close();
        }
        else
        {
            await ShowMessageAsync("No Song Selected", "Purchase");
        }
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
    /// Generates a receipt for the current shopping cart and saves it as a text file with a timestamped filename.
    /// </summary>
    private void CreateReceipt()
    {
        var dateTime = DateTime.Now;
        StringBuilder receipt = new();
        receipt.AppendLine("--- Music Shop Receipt ---");
        receipt.AppendLine($"Date: {dateTime.ToString("dd-MM-yyyy_HH-mm")}");
        receipt.AppendLine("--------------------------");

        foreach (var item in _cartItems)
        {
            receipt.AppendLine($"{item.Product.Title} ({item.Product.Album}) - {item.Quantity} pc. - {item.TotalPrice}$");
        }
        receipt.AppendLine($"Total: {Sum}$");

        File.WriteAllText($"Receipt_{dateTime:dd-MM-yyyy_HH-mm}.txt", receipt.ToString());
    }

    /// <summary>
    /// Raises the PropertyChanged event to notify listeners that a property value has changed.
    /// </summary>
    /// <param name="name">The name of the property that changed. Cannot be null or empty.</param>
    protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
