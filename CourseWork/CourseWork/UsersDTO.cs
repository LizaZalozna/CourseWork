using System;
using System.Collections.Generic;

namespace CourseWork
{
    public class UserDTO
    {
        public string FullName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public SimpleUserData? SimpleUserDetails { get; set; }
    }

    public class SimpleUserData
    {
        public int Reputation { get; set; }
        public List<BookDTO> ReservedBooks { get; set; } 
        public List<RecordDTO> Reservations { get; set; } 
        public List<BookDTO> LendedBooks { get; set; } 
        public List<RecordDTO> Lendings { get; set; } 
    }
}