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

        private void LoadLibrarians()
        {
            try
            {
                LibraryDTO dto;
                if (File.Exists(libraryPath))
                {
                    dto = Serializer.LoadFromXml<LibraryDTO>(libraryPath);
                }
                else
                {
                    dto = new LibraryDTO
                    {
                        Books = new List<BookDTO>(),
                        Users = new List<UserDTO>(),
                        Settings = new SettingsDTO()
                    };
                }

                if (!IsLibraryInitialized())
                {
                    Library.Initialize(dto);
                }

                var simpleUsersUsers = dto.Users
                        .Where(u => u.Role?.ToLower() == "simpleuser");

                simpleUsers.Clear();
                foreach (var simpleUser in simpleUsersUsers)
                {
                    simpleUsers.Add(simpleUser);
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Помилка", $"Помилка завантаження книг: {ex.Message}", "OK");
            }
        }

        private bool IsLibraryInitialized()
        {
            try
            {
                var _ = Library.Instance;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void LoadSimpleUsers()
        {
            try
            {
                if (File.Exists(libraryPath))
                {
                    var dto = Serializer.LoadFromXml<LibraryDTO>(libraryPath);
                    Library.Initialize(dto);
                }

                var library = Library.Instance;
                var simpleUsersUsers = library.ToDTO().Users
                    .Where(u => u.Role?.ToLower() == "simpleuser");

                simpleUsers.Clear();
                foreach (var simpleUser in simpleUsersUsers)
                {
                    simpleUsers.Add(simpleUser);
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
                Library library = Library.Initialize(dto);
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
                    $"Видалити звичайного користувача \"{selectedSimpleUser.FullName}\"?", "Так", "Скасувати");

                if (confirm)
                {
                    try
                    {
                        var library = Library.Instance;
                        var simpleUserToRemove = library.ToDTO().Users.FirstOrDefault(u =>
                            u.Role?.ToLower() == "simpleuser" && u.Login == selectedSimpleUser.Login);

                        if (simpleUserToRemove != null)
                        {
                            library.RemoveSimpleUser(new SimpleUser(simpleUserToRemove));
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