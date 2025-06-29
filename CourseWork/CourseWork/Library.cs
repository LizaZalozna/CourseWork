﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace CourseWork
{
    public class Library
    {
        static Library instance;
        List<Book> books;
        List<User> users;
        Settings settings;

        private Library()
        {
            books = new List<Book>();
            users = new List<User>();
            settings = new Settings(0,0,0,0,0,0);
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

        private Library(LibraryDTO dto)
        {
            books = dto.Books.Select(book => new Book(book)).ToList();
            users = dto.Users.Select(user => UserFactory.CreateUser(user)).ToList();
            settings = new Settings(dto.Settings);
        }

        public LibraryDTO ToDTO()
        {
            return new LibraryDTO
            {
                Books = books.Select(book => book.ToDTO()).ToList(),
                Users = users.Select(user => user.ToDTO()).ToList(),
                Settings = settings.ToDTO()
            };
        }

        public static Library Initialize(LibraryDTO dto)
        {
            if (instance == null)
                instance = new Library(dto);
            return instance;
        }

        public void AddBook(Book book)
        {
            if (!books.Contains(book))
            {
                books.Add(book);
            }
        }

        public void RemoveBook(Book book)
        {
            books.RemoveAll(b => b.nameOfBook_ == book.nameOfBook_ && b.fullNameOfAuthor_ == book.fullNameOfAuthor_);
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

        public void RemoveSimpleUser(SimpleUser simpleUser)
        {
            users.RemoveAll(u => u.login_ == simpleUser.login_);
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

        public void RemoveLibrarian(Librarian librarian)
        {
            users.RemoveAll(u => u.login_ == librarian.login_);
        }

        public void ChangeSettings(int rR, int rT, int mR, int retR, int retT, int mL)
        {
            settings = new Settings(rR, rT, mR, retR, retT, mL);
        }

        public void ReserveBook(SimpleUser user, Book book, DateTime date)
        {
            if (user.reputation_ > 50)
            {
                if (user.reservedBooks_.Count < settings.maxReserved_)
                {
                    if (book.Reserve(user))
                    {
                        user.AddReservedBook(book);
                        user.AddReservation(new ReservationRecord(book, date));
                    }
                }
            }
            else
            {
                Console.WriteLine("Недостатня репутація для резервування книги.");
            }
        }

        public void CancelReservation(SimpleUser user, Book book)
        {
            user.RemoveReservation(user.reservations_.FirstOrDefault(b =>
                   b.book_ == book));
            user.RemoveReservedBook(book);
            book.CancelReservation();
        }

        public bool LendBook(Book book, SimpleUser user, DateTime date)
        {
            bool isReserved = book.isReserved_;
            Book book1 = book;
            if (user.reputation_ > 0 && book.Lend(user) && user.lendedBooks_.Count < settings.maxLended_)
            {
                user.AddLendedBook(book);
                user.AddLending(new LendingRecord(book, date));
                if (isReserved)
                {
                    user.ChangeReputation(settings.reservedReputation_);
                    CancelReservation(user, book1);
                }
                return true;
            }
            return false;
        }

        public void ReturnBook(Book book, SimpleUser user, DateTime date)
        {
            LendingRecord record = user.lendings_.FirstOrDefault(r =>
            r.book_.nameOfBook_ == book.nameOfBook_ &&
            r.book_.fullNameOfAuthor_ == book.fullNameOfAuthor_);
            var bookToRemove = user.lendedBooks_.FirstOrDefault(b =>
            b.nameOfBook_ == book.nameOfBook_ && b.fullNameOfAuthor_ == book.fullNameOfAuthor_);
            if (record == null)
            {
                throw new InvalidOperationException("Ця книга не була видана цьому користувачу.");
            }
            user.RemoveLendedBook(bookToRemove);
            user.RemoveLending(record);
            book.Return();
            if ((date - record.lendDate_).TotalDays > settings.returnTime_)
            {
                user.ChangeReputation(-(settings.returnReputation_));
            }
            else
            {
                user.ChangeReputation(settings.returnReputation_);
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
                        if ((DateTime.Now - res.reservationDate_).TotalDays > settings.reservedTime_)
                        {
                            var simpleUser = (SimpleUser)user;
                            simpleUser.ChangeReputation(-(settings.reservedReputation_));
                            CancelReservation(simpleUser, res.book_);
                        }
                    }
                }
            }
        }

        public Book GetBookByTitleAndAuthor(string title, string author)
        {
            return books.FirstOrDefault(b => b.nameOfBook_ == title && b.fullNameOfAuthor_ == author);
        }

        public SimpleUser GetSimpleUserByLogin(string login)
        {
            return users.OfType<SimpleUser>().FirstOrDefault(u => u.login_ == login);
        }
    }
}