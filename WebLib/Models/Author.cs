﻿// <copyright file="Author.cs" company="My company">
//     Copyright (c) My company". All rights reserved.
// </copyright>

namespace WebApi.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Author model class
    /// </summary>
    public class Author
    {
        /// <summary>
        /// Authors static counter for make each author unique
        /// </summary>
        private static int authorIDCount;

        /// <summary>
        /// Initializes static members of the <see cref="Author"/> class
        /// </summary>
        static Author()
        {
            authorIDCount = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Author"/> class
        /// </summary>
        /// <param name="surname">Surname string</param>
        /// <param name="name">Name string</param>
        /// <param name="patronymic">Patronymic string</param>
        public Author(string surname, string name, string patronymic)
        {
            this.Surname = surname;
            this.Name = name;
            this.Patronymic = patronymic;
            this.AuthorID = authorIDCount++;
        }

        /// <summary>
        /// Gets author id
        /// </summary>
        public int AuthorID
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets surname of author
        /// </summary>
        [Required(ErrorMessage = "Every author has surname name!")]
        public string Surname
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets name of author
        /// </summary>
        [Required(ErrorMessage = "Every author has name!")]
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets patronymic of author
        /// </summary>
        public string Patronymic
        {
            get;
            private set;
        }

        /// <summary>
        /// Cloning author for leave id correct
        /// </summary>
        /// <param name="author">Author with copy info</param>
        public void Clone(Author author)
        {
            this.Surname = author.Surname;
            this.Name = author.Name;
            this.Patronymic = author.Patronymic;
        }

        /// <summary>
        /// Method for compare two Authors by its full name
        /// </summary>
        /// <param name="author">Author for compare</param>
        /// <returns>True if full names of authors is equals</returns>
        public override bool Equals(object author)
        {
            bool result = false;
            if (author is Author)
            {
                Author authorToCheckEquals = author as Author;
                if (this.Surname == authorToCheckEquals.Surname && this.Name == authorToCheckEquals.Name && this.Patronymic == authorToCheckEquals.Patronymic)
                {
                    result = true;
                }
            }

            return result;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Surname, Name, Patronymic);
        }
    }
}
