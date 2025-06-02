using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace CourseWork.Views
{
    public partial class ManageLibrariansPage : ContentPage
    {
        private ObservableCollection<UserDTO> librarians;
        private readonly string libraryPath = "/Users/lizazalozna/Projects/CourseWork/library.xml";
        private readonly Admin admin;

        public ManageLibrariansPage(Admin admin)
        {
            librarians = new ObservableCollection<UserDTO>();
            this.admin = admin;
            InitializeComponent();
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
                LibraryDTO dto = File.Exists(libraryPath)
                ? Serializer.LoadFromXml<LibraryDTO>(libraryPath)
                : new LibraryDTO { Users = new List<UserDTO>(), Books = new List<BookDTO>(), Settings = new SettingsDTO() };
                Library library = new Library(dto);
                if (dto.Users.Any(u => u.Login == UsernameEntry.Text))
                {
                    await DisplayAlert("Помилка", "Користувач з таким іменем вже існує", "OK");
                    return;
                }
                
                library.AddLibrarian(FullNameEntry.Text,UsernameEntry.Text,PasswordEntry.Text, admin);
                Serializer.SaveToXml(library.ToDTO(), libraryPath);
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

        private async void LibrariansListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is UserDTO selectedLibrarian)
            {
                bool confirm = await DisplayAlert("Підтвердження",
                    $"Видалити бібліотекаря \"{selectedLibrarian.FullName}\"?", "Так", "Скасувати");

                if (confirm)
                {
                    try
                    {
                        var dto = Serializer.LoadFromXml<LibraryDTO>(libraryPath);
                        dto.Users.RemoveAll(u => u.Login == selectedLibrarian.Login && u.Role?.ToLower() == "librarian");

                        Serializer.SaveToXml(dto, libraryPath);
                        LoadLibrarians();

                        await DisplayAlert("Успіх", "Бібліотекаря видалено", "OK");
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Помилка", $"Не вдалося видалити: {ex.Message}", "OK");
                    }
                }
                ((ListView)sender).SelectedItem = null;
            }
        }
    }
}