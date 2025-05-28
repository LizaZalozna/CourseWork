using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace CourseWork.Views
{
    public partial class ManageLibrariansPage : ContentPage
    {
        private ObservableCollection<UserDTO> librarians;
        private string libraryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "library.xml");

        public ManageLibrariansPage()
        {
            InitializeComponent();
            librarians = new ObservableCollection<UserDTO>();
            LoadLibrarians();
            LibrariansListView.ItemsSource = librarians;
        }

        private void LoadLibrarians()
        {
            try
            {
                if (File.Exists(libraryPath))
                {
                    var library = Serializer.LoadFromXml<LibraryDTO>(libraryPath);
                    var librarianUsers = library.Users
                        .Where(u => u.Role?.ToLower() == "librarian");

                    librarians.Clear();
                    foreach (var librarian in librarianUsers)
                    {
                        librarians.Add(librarian);
                    }
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
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Помилка", $"Помилка завантаження бібліотекарів: {ex.Message}", "OK");
            }
        }

        private async void OnAddLibrarianClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FullNameEntry.Text) ||
                string.IsNullOrWhiteSpace(UsernameEntry.Text) ||
                string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                await DisplayAlert("Помилка", "Будь ласка, заповніть всі поля", "OK");
                return;
            }

            try
            {
                var library = File.Exists(libraryPath) 
                    ? Serializer.LoadFromXml<LibraryDTO>(libraryPath) 
                    : new LibraryDTO 
                    { 
                        Users = new System.Collections.Generic.List<UserDTO>(),
                        Books = new System.Collections.Generic.List<BookDTO>() 
                    };

                // Перевірка чи користувач вже існує
                if (library.Users.Any(u => u.Login == UsernameEntry.Text))
                {
                    await DisplayAlert("Помилка", "Користувач з таким іменем вже існує", "OK");
                    return;
                }

                // Створення нового бібліотекаря
                var newLibrarian = new UserDTO
                {
                    Login = UsernameEntry.Text,
                    Password = PasswordEntry.Text,
                    FullName = FullNameEntry.Text,
                    Role = "Librarian",
                    SimpleUserDetails = null
                };

                library.Users.Add(newLibrarian);
                Serializer.SaveToXml(library, libraryPath);

                // Очищення полів форми
                FullNameEntry.Text = string.Empty;
                UsernameEntry.Text = string.Empty;
                PasswordEntry.Text = string.Empty;

                LoadLibrarians();
                await DisplayAlert("Успіх", "Бібліотекаря додано успішно", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Помилка", $"Помилка додавання бібліотекаря: {ex.Message}", "OK");
            }
        }
    }
} 