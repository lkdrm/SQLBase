using System.ComponentModel;

namespace MusicShop.Data;

/// <summary>
/// Represents an item in a shopping cart, including the associated product and quantity.
/// </summary>
public class ShoppingCartItem : INotifyPropertyChanged
{
    /// <summary>
    /// Gets or sets the quantity of items associated with this instance.
    /// </summary>
    private int _quantity;

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Gets or sets the song associated with this instance.
    /// </summary>
    public Song Product { get; set; }

    /// <summary>
    /// Gets or sets the quantity of items associated with this instance.
    /// </summary>
    public int Quantity
    {
        get => _quantity;
        set
        {
            _quantity = value;
            OnPropertyChanged("Quantity");
            OnPropertyChanged("TotalPrice");
        }
    }

    /// <summary>
    /// Gets the total price for the product based on its unit price and quantity.
    /// </summary>
    public double TotalPrice => Product.Price * Quantity;

    /// <summary>
    /// Raises the PropertyChanged event to notify listeners that a property value has changed.
    /// </summary>
    /// <param name="name">The name of the property that changed. Cannot be null or empty.</param>
    protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
