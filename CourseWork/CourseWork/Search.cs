using System;
using System.Collections.Generic;
using System.Linq;

namespace CourseWork
{
    public interface IBookSearchStrategy
    {
        List<Book> Search(List<Book> books, string query);
    }

    public class TitleSearchStrategy : IBookSearchStrategy
    {
        public List<Book> Search(List<Book> books, string query)
        {
            return books.Where(b => b.nameOfBook_.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }

    public class AuthorSearchStrategy : IBookSearchStrategy
    {
        public List<Book> Search(List<Book> books, string query)
        {
            return books.Where(b => b.fullNameOfAuthor_.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }

    public class GenreSearchStrategy : IBookSearchStrategy
    {
        public List<Book> Search(List<Book> books, string query)
        {
            if (Enum.TryParse<BookGenre.LiteraryGenre>(query, out var genre))
            {
                return books.Where(b => b.genre_ == genre.ToString()).ToList();
            }
            return new List<Book>();
        }
    }

    public class BookSearcher
    {
        private IBookSearchStrategy strategy;

        public void SetStrategy(IBookSearchStrategy newStrategy)
        {
            strategy = newStrategy;
        }

        public List<Book> Search(List<Book> books, string query)
        {
            return strategy?.Search(books, query) ?? new List<Book>();
        }
    }
}