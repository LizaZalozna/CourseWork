using System;
using System.Collections.Generic;

namespace CourseWork
{
    public class User
    {
        protected string fullName;
        protected string login;
        protected string password;

        public User(UserDTO dto)
        {
            fullName = dto.FullName;
            login = dto.Login;
            password = dto.Password;
        }

        public virtual UserDTO ToDTO()
        {
            return new UserDTO
            {
                FullName = fullName,
                Login = login,
                Password = password,
                Role = "User"
            };
        }

        public User(string fullName, string login, string password)
        {
            this.fullName = fullName;
            this.login = login;
            this.password = password;
        }
    }

    public class Admin : User
    {
        public Admin(string fullName, string login, string password) : base(fullName, login, password) { }

        public override UserDTO ToDTO()
        {
            var dto = base.ToDTO();
            dto.Role = "Admin";
            return dto;
        }

        public Admin(UserDTO dto) : base(dto) { }

        public void AddBook(Book book)
        {
            Library.Instance.AddBook(book);
        }
    }

    public class SimpleUser : User
    {
        int reputation;
        List<Book> reservedBooks;
        List<ReservationRecord> reservations;
        List<Book> lendedBooks;
        List<LendingRecord> lendings;

        public int reputation_
        {
            get { return reputation; }
        }

        public void ChangeReputation(int n) => reputation += n;

        public List<Book> reservedBooks_
        {
            get { return reservedBooks; }
        }

        public List<ReservationRecord> reservations_
        {
            get { return reservations; }
        }

        public List<Book> lendedBooks_
        {
            get { return lendedBooks; }
        }

        public List<LendingRecord> lendings_
        {
            get { return lendings; }
        }

        public SimpleUser(string fullName, string login, string password) : base(fullName, login, password)
        {
            reputation = 100;
        }

        public override UserDTO ToDTO()
        {
            return new UserDTO
            {
                FullName = fullName,
                Login = login,
                Password = password,
                Role = "SimpleUser",
                SimpleUserDetails = new SimpleUserData
                {
                    Reputation = reputation,
                    ReservedBooks = reservedBooks,
                    Reservations = reservations,
                    LendedBooks = lendedBooks,
                    Lendings = lendings
                }
            };
        }

        public SimpleUser(UserDTO dto) : base(dto)
        {
            var details = dto.SimpleUserDetails ?? new SimpleUserData();
            this.reputation = details.Reputation;
            this.reservedBooks = details.ReservedBooks;
            this.reservations = details.Reservations;
            this.lendedBooks = details.LendedBooks;
            this.lendings = details.Lendings;
        }

        public void ReserveBook(Book book, DateTime date)
        {
            if (reputation_ > 50)
            {
                if (book.Reserve(this))
                {
                    reservedBooks.Add(book);
                    reservations.Add(new ReservationRecord(book, date));
                }
            }
            else
            {
                Console.WriteLine("Недостатня репутація для резервування книги.");
            }
        }
    }

    public class Librarian : User
    {
        public Librarian(string fullName, string login, string password) : base(fullName, login, password) { }

        public override UserDTO ToDTO()
        {
            var dto = base.ToDTO();
            dto.Role = "Librarian";
            return dto;
        }

        public Librarian(UserDTO dto) : base(dto) { }

        public bool LendBook(Book book, SimpleUser user, DateTime date)
        {
            if (user.reputation_ > 0 && book.Lend(user))
            {
                user.lendedBooks_.Add(book);
                user.lendings_.Add(new LendingRecord(book, date));
                if (book.isReserved_)
                {
                    user.ChangeReputation(5);
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
                if ((record.returnDate_ - record.lendDate_).TotalDays > 30)
                {
                    user.ChangeReputation(-10);
                }
                else user.ChangeReputation(10);
                user.lendedBooks_.Remove(record.book_);
            }
        }
    }
}