using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace CourseWork.Views
{	
	public partial class LendBookPage : ContentPage
	{
        private readonly string libraryPath = "/Users/lizazalozna/Projects/CourseWork/library.xml";
        private ObservableCollection<BookDTO> books;
        private BookSearcher searcher;
        private ObservableCollection<UserDTO> simpleUsers;
        private Book selectedBook;
        private UserDTO selectedUser;

        public LendBookPage()
		{
			InitializeComponent();
            books = new ObservableCollection<BookDTO>();
            simpleUsers = new ObservableCollection<UserDTO>();
            searcher = new BookSearcher();
            UserPicker.SelectedIndexChanged += UserPicker_SelectedIndexChanged;
            LoadData();
        }

        private void UserPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedLogin = UserPicker.SelectedItem as string;
            if (!string.IsNullOrEmpty(selectedLogin))
            {
                selectedUser = simpleUsers.FirstOrDefault(u => u.Login == selectedLogin);
                LoadData();
            }
            else
            {
                selectedUser = null;
                books.Clear();
                BooksListView.ItemsSource = null;
            }
        }

        private void LoadData()
        {
            try
            {
                var dto = Serializer.LoadFromXml<LibraryDTO>(libraryPath);
                Library.Initialize(dto);

                books.Clear();
                foreach (BookDTO b in Library.Instance.ToDTO().Books)
                {
                    if (b.IsAvailable)
                    {
                        if (!b.IsReserved)
                        {
                            books.Add(b);
                        }
                        else if (selectedUser != null)
                        {
                            if (string.Equals(b.ReservedByLogin?.Trim(), selectedUser.Login.Trim(), StringComparison.OrdinalIgnoreCase))
                            {
                                books.Add(b);
                            }
                        }
                    }
                }
                BooksListView.ItemsSource = books;

                var simpleUsersUsers = dto.Users
                    .Where(u => u.Role?.ToLower() == "simpleuser");

                simpleUsers.Clear();
                foreach (var simpleUser in simpleUsersUsers)
                {
                    simpleUsers.Add(simpleUser);
                }
                var currentItems = UserPicker.ItemsSource as List<string>;
                var newItems = simpleUsers.Select(u => u.Login).ToList();

                if (!Enumerable.SequenceEqual(currentItems ?? new List<string>(), newItems))
                {
                    UserPicker.ItemsSource = newItems;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Помилка", $"Не вдалося завантажити книги: {ex.Message}", "OK");
            }
        }

        private void OnSearchClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchEntry.Text) || SearchTypePicker.SelectedIndex == -1)
            {
                DisplayAlert("Помилка", "Введіть запит і виберіть тип пошуку.", "OK");
                return;
            }

            string query = SearchEntry.Text.Trim();
            switch (SearchTypePicker.SelectedItem.ToString())
            {
                case "Назва":
                    searcher.SetStrategy(new TitleSearchStrategy());
                    break;
                case "Автор":
                    searcher.SetStrategy(new AuthorSearchStrategy());
                    break;
                case "Жанр":
                    searcher.SetStrategy(new GenreSearchStrategy());
                    break;
                default:
                    DisplayAlert("Помилка", "Невідомий тип пошуку.", "OK");
                    return;
            }
            var bookList = books.Select(dto => new Book(dto)).ToList();
            var results = searcher.Search(bookList, query);
            BooksListView.ItemsSource = results.Select(b => b.ToDTO()).ToList();
        }

        private void OnBookTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is BookDTO book)
            {
                selectedBook = Library.Instance.GetBookByTitleAndAuthor(book.NameOfBook, book.FullNameOfAuthor);
                DisplayAlert("Обрано книгу", $"Ви обрали: {book.NameOfBook}", "OK");
            }
            ((ListView)sender).SelectedItem = null;
        }

        private void OnLendBookClicked(object sender, EventArgs e)
        {
            if (selectedBook == null)
            {
                DisplayAlert("Увага", "Оберіть книгу для видачі", "OK");
                return;
            }

            if (UserPicker.SelectedIndex < 0)
            {
                DisplayAlert("Увага", "Оберіть користувача", "OK");
                return;
            }

            var library = Library.Instance;

            var simpleUser = library.GetSimpleUserByLogin(selectedUser.Login);

            try
            {
                library.LendBook(selectedBook, simpleUser, DateTime.Now);
                DisplayAlert("Успіх", $"Книгу \"{selectedBook.nameOfBook_}\" видано користувачу {simpleUser.login_}", "OK");
                LoadData();
                selectedBook = null;
            }
            catch (Exception ex)
            {
                DisplayAlert("Помилка", $"Не вдалося видати книгу: {ex.Message}", "OK");
            }
        }
    }
}