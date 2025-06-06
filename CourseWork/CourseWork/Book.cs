using System;
namespace CourseWork
{
    public class Book
    {
        string fullNameOfAuthor;
        string nameOfBook;
        BookGenre.LiteraryGenre genre;
        bool isAvailable;
        bool isReserved;
        string reservedByLogin;

        public string fullNameOfAuthor_
        {
            get { return fullNameOfAuthor; }
        }

        public string nameOfBook_
        {
            get { return nameOfBook; }
        }

        public bool isAvailable_
        {
            get { return isAvailable; }
        }

        public string genre_
        {
            get { return genre.ToString(); }
        }

        public bool isReserved_
        {
            get { return isReserved; }
        }

        public string reservedByLogin_
        {
            get { return reservedByLogin; }
        }

        public Book(string fullName, string nameOfBook, BookGenre.LiteraryGenre genre)
        {
            this.fullNameOfAuthor = fullName;
            this.nameOfBook = nameOfBook;
            this.genre = genre;
            this.isAvailable = true;
            this.isReserved = false;
        }

        public BookDTO ToDTO()
        {
            return new BookDTO()
            {
                FullNameOfAuthor = fullNameOfAuthor,
                NameOfBook = nameOfBook,
                Genre = genre,
                IsAvailable = isAvailable,
                IsReserved = isReserved,
                ReservedByLogin = reservedByLogin
            };
        }

        public Book(BookDTO dto)
        {
            this.fullNameOfAuthor = dto.FullNameOfAuthor;
            this.nameOfBook = dto.NameOfBook;
            this.genre = dto.Genre;
            this.isAvailable = dto.IsAvailable;
            this.isReserved = dto.IsReserved;
            this.reservedByLogin = dto.ReservedByLogin;
        }

        public bool Reserve(SimpleUser user)
        {
            if (isAvailable && !isReserved)
            {
                isReserved = true;
                reservedByLogin = user.login_;
                return true;
            }
            return false;
        }

        public void CancelReservation()
        {
            isReserved = false;
            reservedByLogin = "";
        }

        public bool Lend(SimpleUser user)
        {
            if (isAvailable || reservedByLogin == user.login_)
            {
                isAvailable = false;
                CancelReservation();
                return true;
            }
            else return false;
        }

        public void Return()
        {
            isAvailable = true;
        }
    }
}