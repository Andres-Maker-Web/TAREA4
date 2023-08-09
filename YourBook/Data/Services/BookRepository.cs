using YourBook.Data.Repository;
using YourBook.Models;

namespace YourBook.Data.Services
{
    public class BookRepository : IBookRepository
    {
        private readonly YourBookDbContext _context;
        public BookRepository(YourBookDbContext yourBookDbContext)
        {
            _context= yourBookDbContext;
        }
        public void AddBook(Book book)
        {
            _context.Books.Add(book); 
            _context.SaveChanges();
        }

        public void DeleteBook(int id)
        {
            var bookTobeRemoved= _context.Books.Find(id);
            _context.Books.Remove(bookTobeRemoved);
            _context.SaveChanges();
        }

        public IEnumerable<Book> GetAll()
        {
            return _context.Books;
        }

        public Book GetSingleBook(int id)
        {
            var bookTobeRetrieved= _context.Books.FirstOrDefault(bk => bk.BookId == id);
            return bookTobeRetrieved;
        }

        public void UpdateBook(int id)
        {
            var bookTobeUpdated = _context.Books.Find(id);
            _context.Books.Update(bookTobeUpdated);
            _context.SaveChanges();
        }
    }
}
