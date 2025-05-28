using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xamarin.Forms;

namespace CourseWork.Views
{
    public partial class LoginPage : ContentPage
    {
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
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "library.xml");
                LibraryDTO library = Serializer.LoadFromXml<LibraryDTO>(filePath);
                var user = library.Users.FirstOrDefault(u =>
                    u.Login == LoginEntry.Text && u.Password == PasswordEntry.Text);

                if (user != null)
                {
                    string role = user.Role;
                    await DisplayAlert("Успіх", "Авторизація успішна!", "OK");
                    if (role.ToLower() == "admin")
                    {
                        Application.Current.MainPage = new NavigationPage(new AdminPage());
                    }
                    else Application.Current.MainPage = new NavigationPage(new MainPage());
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