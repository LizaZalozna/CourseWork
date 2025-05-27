using System;
namespace CourseWork
{
    public class LendingRecord
    {
        Book book;
        DateTime lendDate;
        DateTime? returnDate;
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
            get { return (DateTime)returnDate; }
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

        public LendingRecord(RecordDTO dto)
        {
            this.book = new Book(dto.Book);
            this.lendDate = dto.LendingRecordData.LendDate;
            this.returnDate = dto.LendingRecordData.ReturnDate;
        }

        public RecordDTO ToDTO()
        {
            return new RecordDTO
            {
                Book = book.ToDTO(),
                RecordType = "Lending",
                LendingRecordData = new LendingRecordData
                {
                    LendDate = lendDate,
                    ReturnDate=returnDate
                }
            };
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

        public ReservationRecord(RecordDTO dto)
        {
            this.book = new Book(dto.Book);
            this.reservationDate = dto.ReservationRecordData.ReservationDate;
        }

        public RecordDTO ToDTO()
        {
            return new RecordDTO
            {
                Book = book.ToDTO(),
                RecordType = "Reservation",
                ReservationRecordData = new ReservationRecordData
                {
                    ReservationDate = reservationDate
                }
            };
        }
    }
}