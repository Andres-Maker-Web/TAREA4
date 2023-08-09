using YourBook.Models;

namespace YourBook.Data.Repository
{
    public interface IBookRepository
    {
        public void AddBook(Book book);
        public void UpdateBook(int id);
        public void DeleteBook(int id);
        public IEnumerable<Book> GetAll();
        public Book GetSingleBook(int id);
    }
}
