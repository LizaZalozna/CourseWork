using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CourseWork
{
    [XmlRoot("LibraryDTO")]
    public class LibraryDTO
    {
        public LibraryDTO()
        {
            Books = new List<BookDTO>();
            Users = new List<UserDTO>();
        }

        [XmlArray("Books")]
        [XmlArrayItem("Book")]
        public List<BookDTO> Books { get; set; }

        [XmlArray("Users")]
        [XmlArrayItem("UserDTO")]
        public List<UserDTO> Users { get; set; }

        [XmlArrayItem("SettingsDTO")]
        public SettingsDTO Settings { get; set; }
    }
}