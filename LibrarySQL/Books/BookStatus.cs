namespace LibrarySQL.Books;

/// <summary>
/// Specifies the status of a book in the library system.
/// </summary>
public enum BookStatus
{
    /// <summary>
    /// Gets or sets a value indicating whether the resource is available for use.
    /// </summary>
    Available,

    /// <summary>
    /// Represents the borrowed status or entity within the domain.
    /// </summary>
    Borrowed,

    /// <summary>
    /// Specifies that items are processed or traversed in their natural or defined order.
    /// </summary>
    InOrder,

    /// <summary>
    /// Represents a state indicating that an operation or process is currently waiting to proceed.
    /// </summary>
    Waiting
}
