using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xamarin.Forms;

namespace CourseWork.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly string libraryPath = "/Users/lizazalozna/Projects/CourseWork/library.xml";

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                ShowError("Будь ласка, заповніть всі поля");
                return;
            }

            try
            {
                LibraryDTO dto = File.Exists(libraryPath)
                ? Serializer.LoadFromXml<LibraryDTO>(libraryPath)
                : new LibraryDTO { Users = new List<UserDTO>(), Books = new List<BookDTO>(), Settings = new SettingsDTO() };
                Library library = Library.Initialize(dto);
                library.CheckUnclaimedReservations();
                Serializer.SaveToXml(library.ToDTO(), libraryPath);
                var user = library.ToDTO().Users.FirstOrDefault(u =>
                    u.Login == LoginEntry.Text && u.Password == PasswordEntry.Text);
                if (user != null)
                {
                    string role = user.Role;
                    await DisplayAlert("Успіх", "Авторизація успішна!", "OK");
                    if (role.ToLower() == "admin")
                    {
                        Application.Current.MainPage = new NavigationPage(new AdminPage(new Admin(user)));
                    }
                    else if (role.ToLower() == "librarian")
                    {
                        Application.Current.MainPage = new NavigationPage(new LibrarianPage(new Librarian(user)));
                    }
                    else if (role.ToLower() == "simpleuser")
                    {
                        Application.Current.MainPage = new NavigationPage(new SimpleUserPage(new SimpleUser(user)));
                    }
                }
                else
                {
                    ShowError("Неправильний логін або пароль");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Помилка при авторизації: {ex.Message}");
            }
        }

        private async void ShowError(string message)
        {
            await DisplayAlert("Помилка", message, "OK");
        }
    }
}