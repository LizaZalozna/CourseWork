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
        public List<Book> ReservedBooks { get; set; } 
        public List<ReservationRecord> Reservations { get; set; } 
        public List<Book> LendedBooks { get; set; } 
        public List<LendingRecord> Lendings { get; set; } 
    }

}

