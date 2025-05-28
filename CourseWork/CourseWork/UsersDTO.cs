using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CourseWork
{
    [XmlType("UserDTO")]
    public class UserDTO
    {
        [XmlElement("FullName")]
        public string FullName { get; set; }

        [XmlElement("Login")]
        public string Login { get; set; }

        [XmlElement("Password")]
        public string Password { get; set; }

        [XmlElement("Role")]
        public string Role { get; set; }

        [XmlElement("SimpleUserDetails")]
        public SimpleUserData? SimpleUserDetails { get; set; }
    }

    [XmlType("SimpleUserData")]
    public class SimpleUserData
    {
        [XmlElement("Reputation")]
        public int Reputation { get; set; }

        [XmlArray("ReservedBooks")]
        [XmlArrayItem("Book")]
        public List<BookDTO> ReservedBooks { get; set; }

        [XmlArray("Reservations")]
        [XmlArrayItem("Record")]
        public List<RecordDTO> Reservations { get; set; }

        [XmlArray("LendedBooks")]
        [XmlArrayItem("Book")]
        public List<BookDTO> LendedBooks { get; set; }

        [XmlArray("Lendings")]
        [XmlArrayItem("Record")]
        public List<RecordDTO> Lendings { get; set; }

        public SimpleUserData()
        {
            ReservedBooks = new List<BookDTO>();
            Reservations = new List<RecordDTO>();
            LendedBooks = new List<BookDTO>();
            Lendings = new List<RecordDTO>();
        }
    }
}