using System;
namespace CourseWork
{
    public static class RecordFactory
    {
        public static object CreateRecord(RecordDTO dto)
        {
            return dto.RecordType switch
            {
                "Lending" => new LendingRecord(dto),
                "Reservation" => new ReservationRecord(dto),
                _ => throw new ArgumentException("Unknown record type"),
            };
        }
    }
}