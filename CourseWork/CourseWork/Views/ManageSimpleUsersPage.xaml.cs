using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using Xamarin.Forms;

namespace CourseWork.Views
{	
	public partial class ManageSimpleUsersPage : ContentPage
	{
        private ObservableCollection<UserDTO> simpleUsers;
        private readonly string libraryPath = "/Users/lizazalozna/Projects/CourseWork/library.xml";
        private readonly Librarian librarian;

        public ManageSimpleUsersPage(Librarian librarian)
        {
            simpleUsers = new ObservableCollection<UserDTO>();
            this.librarian = librarian;
            InitializeComponent();    
            LoadSimpleUsers(); 
            SimpleUsersListView.ItemsSource = simpleUsers;
        }

        private void LoadSimpleUsers()
        {
            try
            {
                if (File.Exists(libraryPath))
                {
                    var library = Serializer.LoadFromXml<LibraryDTO>(libraryPath);

                    if (library.Users == null)
                    {
                        library.Users = new List<UserDTO>();
                    }

                    var simpleUsersUsers = library.Users
                        .Where(u => u.Role?.ToLower() == "simpleuser");

                    simpleUsers.Clear();
                    foreach (var simpleUser in simpleUsersUsers)
                    {
                        simpleUsers.Add(simpleUser);
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Помилка", $"Помилка завантаження бібліотекарів: {ex.Message}", "OK");
            }
        }

        private async void OnAddSimpleUsersClicked(object sender, EventArgs e)
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

                library.AddSimpleUser(FullNameEntry.Text, UsernameEntry.Text, PasswordEntry.Text, librarian);
                Serializer.SaveToXml(library.ToDTO(), libraryPath);

                FullNameEntry.Text = string.Empty;
                UsernameEntry.Text = string.Empty;
                PasswordEntry.Text = string.Empty;

                LoadSimpleUsers();
                await DisplayAlert("Успіх", "Звичайного користувача додано успішно", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Помилка", $"Помилка додавання користувача: {ex.Message}", "OK");
            }
        }

        private async void SimpleUsersListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is UserDTO selectedSimpleUser)
            {
                bool confirm = await DisplayAlert("Підтвердження",
                    $"Видалити бібліотекаря \"{selectedSimpleUser.FullName}\"?", "Так", "Скасувати");

                if (confirm)
                {
                    try
                    {
                        var dto = Serializer.LoadFromXml<LibraryDTO>(libraryPath);
                        dto.Users.RemoveAll(u => u.Login == selectedSimpleUser.Login && u.Role?.ToLower() == "simpleuser");

                        Serializer.SaveToXml(dto, libraryPath);
                        LoadSimpleUsers();

                        await DisplayAlert("Успіх", "Звичайного користувача видалено", "OK");
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