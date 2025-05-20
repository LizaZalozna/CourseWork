using System;
namespace CourseWork
{
	public class Book
	{
        string fullNameOfAutor;
        string nameOfBook;
        BookGenre.LiteraryGenre genre;

        public Book(string fullName, string nameOfBook, BookGenre.LiteraryGenre genre)
        {
            this.fullNameOfAutor = fullName;
            this.nameOfBook = nameOfBook;
            this.genre = genre;
        }
    }
}

