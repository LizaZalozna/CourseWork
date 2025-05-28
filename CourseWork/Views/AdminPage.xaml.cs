using System;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace CourseWork.Views
{
    public partial class AdminPage : ContentPage
    {
        private string libraryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "library.xml");

        public AdminPage()
        {
            InitializeComponent();
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            try
            {
                if (File.Exists(libraryPath))
                {
                    var library = Serializer.LoadFromXml<LibraryDTO>(libraryPath);

                    int usersCount = library.Users.Count;
                    int booksCount = library.Books?.Count ?? 0;
                    int librariansCount = library.Users.Count(u => u.Role?.ToLower() == "librarian");

                    UsersCountLabel.Text = $"Кількість користувачів: {usersCount}";
                    BooksCountLabel.Text = $"Кількість книг: {booksCount}";
                    LibrariansCountLabel.Text = $"Кількість бібліотекарів: {librariansCount}";
                }
                else
                {
                    // Створюємо новий файл з базовою структурою
                    var library = new LibraryDTO 
                    { 
                        Users = new System.Collections.Generic.List<UserDTO>(),
                        Books = new System.Collections.Generic.List<BookDTO>() 
                    };
                    Serializer.SaveToXml(library, libraryPath);
                    
                    UsersCountLabel.Text = "Кількість користувачів: 0";
                    BooksCountLabel.Text = "Кількість книг: 0";
                    LibrariansCountLabel.Text = "Кількість бібліотекарів: 0";
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Помилка", $"Помилка завантаження статистики: {ex.Message}", "OK");
            }
        }

        private async void OnManageUsersClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ManageLibrariansPage());
        }

        private async void OnManageBooksClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Інформація", "Функція управління книгами в розробці", "OK");
        }

        private async void OnManageLibrariansClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ManageLibrariansPage());
        }

        private async void OnViewReportsClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Інформація", "Функція перегляду звітів в розробці", "OK");
        }

        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Інформація", "Функція налаштувань системи в розробці", "OK");
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Вихід", "Ви впевнені, що хочете вийти?", "Так", "Ні");
            if (answer)
            {
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
        }
    }
} 