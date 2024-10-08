using System;
using System.Collections.Generic;
using System.Linq;
using BookStore.Shared.Entities;
using BookStoreAPI.Data;
using BookStoreAPI.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace BookStoreAPI.Tests.Services
{
    [TestFixture]
    public class BookStoreDataServiceTests
    {
        private BookStoreDbContext _dbContext;
        private BookStoreDataService _dataService;

        [SetUp]
        public void Setup()
        {
            // Initialize the in-memory database
            var options = new DbContextOptionsBuilder<BookStoreDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _dbContext = new BookStoreDbContext(options);
            _dataService = new BookStoreDataService(_dbContext);

            // Seed the database with test data
            SeedTestData();
        }

        [TearDown]
        public void TearDown()
        {
            // Dispose the in-memory database
            _dbContext.Dispose();
        }

        [Test]
        public void GetBookById_ExistingId_ReturnsBook()
        {
            // Arrange
            int existingId = 1;

            // Act
            Book book = _dataService.GetBookById(existingId);

            // Assert
            Assert.That(book, Is.Not.Null);
            Assert.That(book.Id, Is.EqualTo(existingId));
        }

        [Test]
        public void GetBookById_NonExistingId_ThrowsException()
        {
            // Arrange
            int nonExistingId = 100;

            // Act & Assert
            Assert.Throws<Exception>(() => _dataService.GetBookById(nonExistingId));
        }

        private void SeedTestData()
        {
            // Create test books
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Book 1", Author = "Author 1" },
                new Book { Id = 2, Title = "Book 2", Author = "Author 2" },
                new Book { Id = 3, Title = "Book 3", Author = "Author 3" }
            };

            // Add test books to the in-memory database
            _dbContext.Books.AddRange(books);
            _dbContext.SaveChanges();
        }

        [Test]
        public void UpdateBook_ExistingBook_UpdatesBook()
        {
            // Arrange
            int existingId = 1;
            Book updatedBook = new Book { Id = existingId, Title = "Updated Book", Author = "Updated Author" };

            // Act
            _dataService.UpdateBook(updatedBook);
            Book retrievedBook = _dataService.GetBookById(existingId);

            // Assert
            Assert.That(retrievedBook, Is.Not.Null);
            Assert.That(retrievedBook.Id, Is.EqualTo(existingId));
            Assert.That(retrievedBook.Title, Is.EqualTo(updatedBook.Title));
            Assert.That(retrievedBook.Author, Is.EqualTo(updatedBook.Author));
        }

        [Test]
        public void UpdateBook_NonExistingBook_ThrowsException()
        {
            // Arrange
            int nonExistingId = 100;
            Book updatedBook = new Book { Id = nonExistingId, Title = "Updated Book", Author = "Updated Author" };

            // Act & Assert
            Assert.Throws<Exception>(() => _dataService.UpdateBook(updatedBook));
        }
    }
}
