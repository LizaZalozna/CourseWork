using System;
namespace CourseWork
{
    public class Book
    {
        string fullNameOfAutor;
        string nameOfBook;
        BookGenre.LiteraryGenre genre;
        bool isAvailable;
        bool isReserved;
        SimpleUser reservedBy;

        public bool isAvailable_
        {
            get { return isAvailable; }
        }

        public bool isReserved_
        {
            get { return isReserved; }
        }

        public SimpleUser reservedBy_
        {
            get { return reservedBy; }
        }

        public Book(string fullName, string nameOfBook, BookGenre.LiteraryGenre genre)
        {
            this.fullNameOfAutor = fullName;
            this.nameOfBook = nameOfBook;
            this.genre = genre;
            this.isAvailable = true;
            this.isReserved = false;
        }

        public BookDTO ToDTO()
        {
            return new BookDTO()
            {
                FullNameOfAutor = fullNameOfAutor,
                NameOfBook = nameOfBook,
                Genre = genre,
                IsAvailable = isAvailable,
                IsReserved = isReserved,
                ReservedBy = reservedBy
            };
        }

        public Book(BookDTO dto)
        {
            this.fullNameOfAutor = dto.FullNameOfAutor;
            this.nameOfBook = dto.NameOfBook;
            this.genre = dto.Genre;
            this.isAvailable = dto.IsAvailable;
            this.isReserved = dto.IsReserved;
            this.reservedBy = dto.ReservedBy;
        }

        public bool Reserve(SimpleUser user)
        {
            if (isAvailable && !isReserved)
            {
                isReserved = true;
                reservedBy = user;
                return true;
            }
            return false;
        }

        public void CancelReservation()
        {
            isReserved = false;
            reservedBy = null;
        }

        public bool Lend(SimpleUser user)
        {
            if (isAvailable && reservedBy == user)
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