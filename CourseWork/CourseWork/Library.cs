using System;
using System.Collections.Generic;
using System.Linq;

namespace CourseWork
{
    public class Library
    {
        static Library instance;
        List<Book> books;
        List<User> users;

        private Library() { }

        public Library(LibraryDTO dto)
        {
            books = dto.Books.Select(book => new Book(book)).ToList();
            users = dto.Users.Select(user => UserFactory.CreateUser(user)).ToList();
        }

        public LibraryDTO ToDTO()
        {
            return new LibraryDTO
            {
                Books = books.Select(book => book.ToDTO()).ToList(),
                Users = users.Select(user => user.ToDTO()).ToList()
            };
        }

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

        public void CheckUnclaimedReservations()
        {
            foreach (var user in users)
            {
                if (user is SimpleUser)
                {
                    foreach (var res in ((SimpleUser)user).reservations_)
                    {
                        if ((DateTime.Now - res.reservationDate_).TotalDays > 5)
                        {
                            ((SimpleUser)user).ChangeReputation(-5);
                            ((SimpleUser)user).reservations_.Remove(res);
                            ((SimpleUser)user).reservedBooks_.Remove(res.book_);
                            res.book_.CancelReservation();
                        }
                    }
                }
            }
        }
    }
}

