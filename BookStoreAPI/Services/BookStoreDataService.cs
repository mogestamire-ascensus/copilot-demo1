using BookStore.Shared.Entities;
using BookStoreAPI.Data;

namespace BookStoreAPI.Services
{
    /// <summary>
    /// Service for managing book data in the bookstore database.
    /// </summary>
    public class BookStoreDataService
    {
        private readonly BookStoreDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookStoreDataService"/> class.
        /// </summary>
        /// <param name="dbContext">The database context to be used for data operations.</param>
        public BookStoreDataService(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves all books from the database.
        /// </summary>
        /// <returns>An enumerable collection of books.</returns>
        public IEnumerable<Book> GetBooks()
        {
            return _dbContext.Books.ToList();
        }

        /// <summary>
        /// Retrieves a book by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the book.</param>
        /// <returns>The book with the specified identifier, or null if not found.</returns>
        public Book GetBookById(int id)
        {
            return _dbContext.Books.FirstOrDefault(b => b.Id == id) ?? throw new Exception("Book not found");
        }

        /// <summary>
        /// Searches for books by a search term.
        /// </summary>
        /// <param name="searchTerm">The search term to use for finding books.</param>
        /// <returns>An enumerable collection of books that match the search term.</returns>
        public IEnumerable<Book> SearchBooks(string searchTerm)
        {
            return _dbContext.Books.Where(b => b.Title.Contains(searchTerm) || b.Author.Contains(searchTerm)).ToList();
        }

        /// <summary>
        /// Adds a new book to the database.
        /// </summary>
        /// <param name="book">The book to add.</param>
        public void AddBook(Book book)
        {
            _dbContext.Books.Add(book);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Updates an existing book in the database.
        /// </summary>
        /// <param name="book">The book with updated data.</param>
        /// <exception cref="Exception">Thrown when the book is not found.</exception>
        public void UpdateBook(Book book)
        {
            var existingBook = _dbContext.Books.Find(book.Id);
            if (existingBook == null)
            {
                throw new Exception("Book not found");
            }

            _dbContext.Entry(existingBook).CurrentValues.SetValues(book);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Deletes a book from the database.
        /// </summary>
        /// <param name="id">The identifier of the book to delete.</param>
        /// <exception cref="Exception">Thrown when the book is not found.</exception>
        public void DeleteBook(int id)
        {
            var book = _dbContext.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                throw new Exception("Book not found");
            }

            _dbContext.Books.Remove(book);
            _dbContext.SaveChanges();
        }
    }
}
