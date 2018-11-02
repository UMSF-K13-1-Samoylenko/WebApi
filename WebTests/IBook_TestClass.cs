﻿// <copyright file="IBook_TestClass.cs" company="My company">
//     Copyright (c) My company". All rights reserved.
// </copyright>

namespace WebTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using WebApi.BookService;
    using WebLib;
    using WebLib.Models;

    /// <summary>
    /// IBook test class
    /// </summary>
    [TestClass]
    public class IBook_TestClass
    {
        /// <summary>
        /// IDataProvider object
        /// </summary>
        private static readonly IDataProvider DataProvider;

        /// <summary>
        /// IBook object
        /// </summary>
        private IBook booksObject;

        /// <summary>
        /// Initializes static members of the <see cref="IBook_TestClass"/> class
        /// </summary>
        static IBook_TestClass()
        {
            DataProvider = new DataProviderList();
        }

        /// <summary>
        /// Test initialize method
        /// </summary>
        [TestInitialize]
        public void TestClassInitialize()
        {
            this.booksObject = new LibraryList(DataProvider);
        }

        /// <summary>
        /// Add book without author method
        /// </summary>
        [TestMethod]
        public void AddBook()
        {
            // Arrange
            Book expected = new Book("Test book title");

            // Act
            int bookId = this.booksObject.AddBook(expected);
            Book actual = this.booksObject.GetBookById(bookId);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Adding book author and get book authors test
        /// </summary>
        [TestMethod]
        public void AddBookAuthor_GetBookAuthors()
        {
            // Arrange
            Book testBook = new Book("Test book");
            int testBookId = this.booksObject.AddBook(testBook);
            List<int> authorsToAddIds = new List<int>() { 1, 2 };
            ILibrary library = this.booksObject as ILibrary;
            List<Author> expectedAuthors = (from author in library.GetAllAuthors()
                                    where authorsToAddIds.Contains(author.Id)
                                    select author).ToList();
            
            // Act
            foreach (int authorIdToAdd in authorsToAddIds)
            {
                this.booksObject.AddBookAuthor(testBookId, authorIdToAdd);
            }

            List<Author> actualAuthors = this.booksObject.GetBookAuthors(testBookId);

            // Assert
            CollectionAssert.AreEqual(expectedAuthors, actualAuthors);
        }

        /// <summary>
        /// Add book genre and get book genres test
        /// </summary>
        [TestMethod]
        public void AddBookGenre_GetBookGenres()
        {
            // Arrange
            Book testBook = new Book("Test book");
            int testBookId = this.booksObject.AddBook(testBook);
            List<int> genresToAddIds = new List<int>() { 0, 1 };
            List<Genre> expectedGenres = (from genre in DataProvider.GetGenres()
                                            where genresToAddIds.Contains(genre.Id)
                                            select genre).ToList();

            // Act
            foreach (int genreId in genresToAddIds)
            {
                this.booksObject.AddBookGenre(testBookId, genreId);
            }

            List<Genre> actualGenres = this.booksObject.GetBookGenres(testBookId);

            // Assert
            CollectionAssert.AreEqual(expectedGenres, actualGenres);
        }

        /// <summary>
        /// Getting all books method
        /// </summary>
        [TestMethod]
        public void GetAllBooks()
        {
            // Act
            List<Book> actual = this.booksObject.GetAllBooks().ToList();

            // Assert
            Assert.IsNotNull(actual);
        }

        /// <summary>
        /// Getting book by id test
        /// </summary>
        [TestMethod]
        public void GetBookById()
        {
            // Arrange
            Book expected = new Book("Second book");
            int expectedBookId = this.booksObject.AddBook(expected);

            // Act
            Book actual = this.booksObject.GetBookById(expectedBookId);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Getting books in genre test
        /// </summary>
        [TestMethod]
        public void GetBooksInGenre()
        {
            // Arrange
            ILibrary library = this.booksObject as ILibrary;
            List<Genre> genres = library.GetAllGenres().ToList();
            int genreIdToAdd = genres[0].Id;
            List<Book> expectedBooks = new List<Book>()
            {
                new Book("Test book A"),
                 new Book("Test book B")
            };
            List<int> expectedBooksIds = new List<int>();
            foreach (Book book in expectedBooks)
            {
                expectedBooksIds.Add(this.booksObject.AddBook(book));
            }

            foreach (int bookId in expectedBooksIds)
            {
                this.booksObject.AddBookGenre(bookId, genreIdToAdd);
            }

            // Act
            List<Book> actualBooks = this.booksObject.GetBooksInGenre(genreIdToAdd).ToList();

            // Assert
            CollectionAssert.AreEqual(expectedBooks, actualBooks);
        }

        /// <summary>
        /// Updating book test
        /// </summary>
        [TestMethod]
        public void UpdateBook()
        {
            // Arrange
            Book bookForUpdate = new Book("Test book");
            Book expected = new Book("New book");
            int idForUpdate = this.booksObject.AddBook(bookForUpdate);

            // Act
            this.booksObject.UpdateBook(idForUpdate, expected);

            // Assert
            Assert.AreEqual(expected, this.booksObject.GetBookById(idForUpdate));
        }

        /// <summary>
        /// Update book genre test
        /// </summary>
        [TestMethod]
        public void UpdateBookGenre()
        {
            // Arrange
            ILibrary library = this.booksObject as ILibrary;
            List<Genre> allGenres = library.GetAllGenres().ToList();
            int oldBookGenre = allGenres[0].Id;
            int newBookGenre = allGenres[1].Id;
            Book book = new Book("Test");
            int bookId = this.booksObject.AddBook(book);
            this.booksObject.AddBookGenre(bookId, oldBookGenre);
            List<Genre> bookGenres = this.booksObject.GetBookGenres(bookId);
            Assert.AreEqual(1, bookGenres.Count); // only 1 genre added
            Assert.AreEqual(oldBookGenre, bookGenres[0].Id); // only old book genre added

            // Act
            this.booksObject.UpdateBookGenre(bookId, oldBookGenre, newBookGenre);
            bookGenres = this.booksObject.GetBookGenres(bookId);

            // Assert
            Assert.AreEqual(1, bookGenres.Count); // count of genres is still 1
            Assert.AreEqual(newBookGenre, bookGenres[0].Id); // only new genre exist for this book
        }

        /// <summary>
        /// Update book author test
        /// </summary>
        [TestMethod]
        public void UpdateBookAuthor()
        {
            // Arrange
            ILibrary library = this.booksObject as ILibrary;
            List<Author> allAuthors = library.GetAllAuthors().ToList();
            int oldBookAuthor = allAuthors[0].Id;
            int newBookAuthor = allAuthors[1].Id;
            Book book = new Book("Test");
            int bookId = this.booksObject.AddBook(book);
            this.booksObject.AddBookAuthor(bookId, oldBookAuthor);
            List<Author> bookAuthors = this.booksObject.GetBookAuthors(bookId);

            // Act
            this.booksObject.UpdateBookAuthor(bookId, oldBookAuthor, newBookAuthor);
            bookAuthors = this.booksObject.GetBookAuthors(bookId);

            // Assert
            Assert.AreEqual(1, bookAuthors.Count); // count of author is still 1
            Assert.AreEqual(newBookAuthor, bookAuthors[0].Id); // only new author exist for this book
        }

        /// <summary>
        /// Deleting book by correct id method
        /// </summary>
        [TestMethod]
        public void DeleteBook()
        {
            // Arrange
            Book book = new Book("Test");
            int idForDelete = this.booksObject.AddBook(book);

            // Act
            this.booksObject.DeleteBook(idForDelete);

            // Assert
            Assert.AreEqual(null, this.booksObject.GetBookById(idForDelete));
        }

        /// <summary>
        /// Delete book author test
        /// </summary>
        [TestMethod]
        public void DeleteBookAuthor()
        {
            // Arrange
            int bookAuthorId = 0;
            Book book = new Book("Test");
            int bookId = this.booksObject.AddBook(book);
            this.booksObject.AddBookAuthor(bookId, bookAuthorId);
            List<Author> bookAuthors = this.booksObject.GetBookAuthors(bookId);

            // Act
            this.booksObject.DeleteBookAuthor(bookId, bookAuthorId);
            bookAuthors = this.booksObject.GetBookAuthors(bookId);

            // Assert
            Assert.AreEqual(0, bookAuthors.Count);
        }

        /// <summary>
        /// Delte book genre test
        /// </summary>
        [TestMethod]
        public void DeleteBookGenre()
        {
            // Arrange
            int bookGenreId = 0;
            Book book = new Book("Test");
            int bookId = this.booksObject.AddBook(book);
            this.booksObject.AddBookGenre(bookId, bookGenreId);
            List<Genre> bookGenres = this.booksObject.GetBookGenres(bookId);

            // Act
            this.booksObject.DeleteBookGenre(bookId, bookGenreId);
            bookGenres = this.booksObject.GetBookGenres(bookId);

            // Assert
            Assert.AreEqual(0, bookGenres.Count);
        }
    }
}
