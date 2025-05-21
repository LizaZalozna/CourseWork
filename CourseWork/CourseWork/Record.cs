using System;
namespace CourseWork
{
    public class LendingRecord
    {
        Book book;
        DateTime lendDate;
        DateTime returnDate;

        public Book book_
        {
            get { return book; }
        }

        public DateTime lendDate_
        {
            get { return lendDate; }
        }

        public DateTime returnDate_
        {
            get { return returnDate; }
            set
            {
                if (value >= lendDate) returnDate = value;
                else throw new Exception("Дата повернення не може бути раніше дати видачі книги");
            }
        }

        public LendingRecord(Book book, DateTime lendDate)
        {
            this.book = book;
            this.lendDate = lendDate;
        }
    }

    public class ReservationRecord
    {
        Book book;
        DateTime reservationDate;

        public Book book_
        {
            get { return book; }
        }

        public DateTime reservationDate_
        {
            get { return reservationDate; }
        }

        public ReservationRecord(Book book, DateTime reservationDate)
        {
            this.book = book;
            this.reservationDate = reservationDate;
        }
    }
}

