using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CourseWork
{
    [XmlRoot("Library")]
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
        [XmlArrayItem("User")]
        public List<UserDTO> Users { get; set; }
    }
}