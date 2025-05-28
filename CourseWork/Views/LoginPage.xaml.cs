using System;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace CourseWork.Views
{
    public partial class LoginPage : ContentPage
    {
        private string libraryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "library.xml");

        public LoginPage()
        {
            InitializeComponent();
            InitializeLibraryFile();
        }

        private void InitializeLibraryFile()
        {
            try
            {
                if (!File.Exists(libraryPath))
                {
                    // Створюємо новий файл з базовою структурою і адміністратором за замовчуванням
                    var library = new LibraryDTO 
                    { 
                        Users = new System.Collections.Generic.List<UserDTO>
                        {
                            new UserDTO
                            {
                                FullName = "Адміністратор",
                                Login = "admin",
                                Password = "admin",
                                Role = "Admin",
                                SimpleUserDetails = null
                            }
                        },
                        Books = new System.Collections.Generic.List<BookDTO>() 
                    };
                    Serializer.SaveToXml(library, libraryPath);
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Помилка", $"Помилка ініціалізації бази даних: {ex.Message}", "OK");
            }
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
                if (!File.Exists(libraryPath))
                {
                    ShowError("Файл з даними користувачів не знайдено");
                    return;
                }

                var library = Serializer.LoadFromXml<LibraryDTO>(libraryPath);
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
                    else
                    {
                        Application.Current.MainPage = new NavigationPage(new MainPage());
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