using System;
using System.Collections.Generic;

namespace CourseWork
{
    public class Library
    {
        static Library instance;
        List<Book> books;
        List<User> users;

        private Library() { }

        public static Library Instance
        {
            get
            {
                if (instance == null)
                    instance = new Library();
                return instance;
            }
        }

        public void AddBook(Book book)
        {
            if (!books.Contains(book))
            {
                books.Add(book);
            }
        }

        public bool AddSimpleUser(string fullName, string login, string password, Librarian librarian)
        {
            try
            {
                var user = (SimpleUser)UserFactory.CreateUser("simpleuser", fullName, login, password, librarian);
                users.Add(user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddLibrarian(string fullName, string login, string password, Admin admin)
        {
            try
            {
                var librarian = (Librarian)UserFactory.CreateUser("librarian", fullName, login, password, admin);
                users.Add(librarian);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

