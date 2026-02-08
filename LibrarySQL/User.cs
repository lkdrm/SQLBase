namespace LibrarySQL
{
    public class User
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public List<Book> BorrowedBooks { get; set; } = [];
    }
}
