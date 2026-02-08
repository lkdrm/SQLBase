namespace LibrarySQL
{
    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public BookStatus Status { get; set; }

        public int? BorrowerId { get; set; }

        public DateTime? BorrowedDate { get; set; }

        public User Borrower { get; set; }
    }
}
