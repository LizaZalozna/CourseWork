using System;

namespace CourseWork
{
    public class User
    {
        protected string fullName;
        protected string login;
        private string password;

        public User(string fullName, string login, string password)
        {
            this.fullName = fullName;
            this.login = login;
            this.password = password;
        }
    }

    public class Admin : User
    {
        public Admin(string fullName, string login, string password) : base(fullName, login, password) { }
    }

    public class SimpleUser : User
    {
        int reputation;

        public int reputation_
        {
            get { return reputation; }
        }

        public void ChangeReputation(int n) => reputation += n;

        public SimpleUser(string fullName, string login, string password) : base(fullName, login, password)
        {
            reputation = 100;
        }
    }

    public class Librarian : User
    {
        public Librarian(string fullName, string login, string password) : base(fullName, login, password) { }
    }
}