using System;
namespace CourseWork
{
    public static class UserFactory
    {
        public static User CreateUser(string type, string fullName, string login, string password, User createdBy = null)
        {
            switch (type.ToLower())
            {
                case "admin":
                    return new Admin(fullName, login, password);
                case "librarian":
                    if (createdBy is Admin)
                        return new Librarian(fullName, login, password);
                    else
                        throw new UnauthorizedAccessException("Тільки адміністратор може створювати бібліотекарів");
                case "simpleuser":
                    if (createdBy is Librarian)
                        return new SimpleUser(fullName, login, password);
                    else
                        throw new UnauthorizedAccessException("Тільки бібліотекар може створювати звичайних користувачів");
                default:
                    throw new ArgumentException("Unknown user type");
            }
        }

        public static User CreateUser(UserDTO dto)
        {
            return dto.Role switch
            {
                "Admin" => new Admin(dto),
                "Librarian" => new Librarian(dto),
                "SimpleUser" => new SimpleUser(dto)
            };
        }
    }
}