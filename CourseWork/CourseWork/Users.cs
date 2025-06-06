using System;
using System.Collections.Generic;
using System.Linq;

namespace CourseWork
{
    public abstract class User
    {
        protected string fullName;
        protected string login;
        protected string password;

        public string login_
        {
            get { return login; }
        }

        public string fullName_
        {
            get { return fullName; }
        }

        protected User(UserDTO dto)
        {
            fullName = dto.FullName;
            login = dto.Login;
            password = dto.Password;
        }

        public abstract UserDTO ToDTO();

        protected User(string fullName, string login, string password)
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
            return new UserDTO
            {
                FullName = fullName,
                Login = login,
                Password = password,
                Role = "Admin"
            };
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
            reservedBooks = new List<Book>();
            reservations = new List<ReservationRecord>();
            lendedBooks = new List<Book>();
            lendings = new List<LendingRecord>();
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
                    ReservedBooks = reservedBooks.Select(book => book.ToDTO()).ToList(),
                    Reservations = reservations.Select(record => record.ToDTO()).ToList(),
                    LendedBooks = lendedBooks.Select(book => book.ToDTO()).ToList(),
                    Lendings = lendings.Select(record => record.ToDTO()).ToList()
                }
            };
        }

        public SimpleUser(UserDTO dto) : base(dto)
        {
            var details = dto.SimpleUserDetails;
            this.reputation = details.Reputation;
            this.reservedBooks = details.ReservedBooks.Select(bookdto => new Book(bookdto)).ToList();
            this.reservations = details.Reservations.Select(recorddto => RecordFactory.CreateRecord(recorddto) as ReservationRecord).ToList();
            this.lendedBooks = details.LendedBooks.Select(bookdto => new Book(bookdto)).ToList();
            this.lendings = details.Lendings.Select(recorddto => RecordFactory.CreateRecord(recorddto) as LendingRecord).ToList();
        }
    }

    public class Librarian : User
    {
        public Librarian(string fullName, string login, string password) : base(fullName, login, password) { }

        public override UserDTO ToDTO()
        {
            return new UserDTO
            {
                FullName = fullName,
                Login = login,
                Password = password,
                Role = "Librarian"
            };
        }

        public Librarian(UserDTO dto) : base(dto) { }
    }
}