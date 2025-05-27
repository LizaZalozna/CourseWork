using System;
namespace CourseWork
{
    public class RecordDTO
    {
        public BookDTO Book { get; set; }
        public string RecordType { get; set; }

        public LendingRecordData? LendingRecordData { get; set; }
        public ReservationRecordData? ReservationRecordData { get; set; }
    }

    public class LendingRecordData
    {
        public DateTime LendDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }

    public class ReservationRecordData
    {
        public DateTime ReservationDate { get; set; }
    }
}