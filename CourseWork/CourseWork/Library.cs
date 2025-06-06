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
        Settings settings;
        private Library()
        {
            books = new List<Book>();
            users = new List<User>();
            settings = new Settings(0,0,0,0);
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

        public static Library Instance
        {
            get
            {
                if (instance == null)
                    instance = new Library();
                return instance;
            }
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

        public void ChangeSettings(int rR, int rT, int retR, int retT)
        {
            settings = new Settings(rR, rT, retR, retT);
        }

        public void ReserveBook(SimpleUser user, Book book, DateTime date)
        {
            if (user.reputation_ > 50)
            {
                if (book.Reserve(user))
                {
                    user.reservedBooks_.Add(book);
                    user.reservations_.Add(new ReservationRecord(book, date));
                }
            }
            else
            {
                Console.WriteLine("Недостатня репутація для резервування книги.");
            }
        }

        public bool LendBook(Book book, SimpleUser user, DateTime date)
        {
            if (user.reputation_ > 0 && book.Lend(user))
            {
                user.lendedBooks_.Add(book);
                user.lendings_.Add(new LendingRecord(book, date));
                if (book.isReserved_)
                {
                    user.ChangeReputation(settings.reservedReputation_);
                    user.reservedBooks_.Remove(book);
                    user.reservations_.RemoveAll(r => r.book_ == book);
                }
                return true;
            }
            return false;
        }

        public void ReturnBook(Book book, SimpleUser user, DateTime date)
        {
            book.Return();
            LendingRecord record = user.lendings_.Find(r => r.book_ == book);
            if (record != null)
            {
                record.returnDate_ = date;
                if ((record.returnDate_ - record.lendDate_).TotalDays > settings.returnTime_)
                {
                    user.ChangeReputation(-(settings.returnReputation_));
                }
                else user.ChangeReputation(settings.returnReputation_);
                user.lendedBooks_.Remove(record.book_);
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
                            ((SimpleUser)user).ChangeReputation(-(settings.reservedReputation_));
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