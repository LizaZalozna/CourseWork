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