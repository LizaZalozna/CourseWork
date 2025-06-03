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
                    var dto = Serializer.LoadFromXml<LibraryDTO>(libraryPath);
                    Library.Initialize(dto);
                }

                var library = Library.Instance;
                var librarianUsers = library.ToDTO().Users
                    .Where(u => u.Role?.ToLower() == "librarian");

                librarians.Clear();
                foreach (var librarian in librarianUsers)
                {
                    librarians.Add(librarian);
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
                Library library = Library.Initialize(dto);
                if (dto.Users.Any(u => u.Login == UsernameEntry.Text))
                {
                    await DisplayAlert("Помилка", "Користувач з таким іменем вже існує", "OK");
                    return;
                }

                library.AddLibrarian(FullNameEntry.Text, UsernameEntry.Text, PasswordEntry.Text, admin);
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
                        var library = Library.Instance;
                        var librarianToRemove = library.ToDTO().Users.FirstOrDefault(u =>
                            u.Role?.ToLower() == "librarian" && u.Login == selectedLibrarian.Login);

                        if (librarianToRemove != null)
                        {
                            library.RemoveLibrarian(new Librarian(librarianToRemove));
                            Serializer.SaveToXml(library.ToDTO(), libraryPath);
                            LoadLibrarians();
                            await DisplayAlert("Успіх", "Бібліотекаря видалено", "OK");
                        }
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