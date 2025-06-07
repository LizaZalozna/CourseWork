using System;
using System.Xml.Serialization;

namespace CourseWork
{
    [XmlType("Record")]
    public class RecordDTO
    {
        [XmlElement("Book")]
        public BookDTO Book { get; set; }

        [XmlElement("RecordType")]
        public string RecordType { get; set; }

        [XmlElement("LendingRecordData")]
        public LendingRecordData? LendingRecordData { get; set; }

        [XmlElement("ReservationRecordData")]
        public ReservationRecordData? ReservationRecordData { get; set; }

        public string RecordDateString
        {
            get
            {
                if (RecordType == "Lending" && LendingRecordData != null)
                {
                    return $"Видано: {LendingRecordData.LendDate:dd.MM.yyyy} - Повернути: {LendingRecordData.ReturnDate:dd.MM.yyyy}";
                }
                else if (RecordType == "Reservation" && ReservationRecordData != null)
                {
                    return $"Зарезервовано: {ReservationRecordData.ReservationDate:dd.MM.yyyy}";
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }

    [XmlType("LendingRecordData")]
    public class LendingRecordData
    {
        [XmlElement("LendDate")]
        public DateTime LendDate { get; set; }

        [XmlElement("ReturnDate")]
        public DateTime? ReturnDate { get; set; }
    }

    [XmlType("ReservationRecordData")]
    public class ReservationRecordData
    {
        [XmlElement("ReservationDate")]
        public DateTime ReservationDate { get; set; }
    }
}