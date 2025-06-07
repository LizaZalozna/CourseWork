using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CourseWork.Views
{
    public partial class ReserveBookPage : ContentPage
    {
        private readonly string libraryPath = "/Users/lizazalozna/Projects/CourseWork/library.xml";
        private ObservableCollection<BookDTO> books;
        private BookSearcher searcher;
        private readonly SimpleUser simpleUser;

        public ReserveBookPage(SimpleUser simpleUser)
        {
            InitializeComponent();
            books = new ObservableCollection<BookDTO>();
            this.simpleUser = simpleUser;
            searcher = new BookSearcher();
            LoadBooks();
        }

        private void LoadBooks()
        {
            try
            {
                if (File.Exists(libraryPath))
                {
                    var dto = Serializer.LoadFromXml<LibraryDTO>(libraryPath);
                    Library.Initialize(dto);
                }
                books.Clear();
                foreach (BookDTO b in Library.Instance.ToDTO().Books)
                {
                    if (b.IsAvailable && !b.IsReserved)
                        books.Add(b);
                }
                BooksListView.ItemsSource = books;
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

        private async void OnBookTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is BookDTO selectedBook)
            {
                bool reserve = await DisplayAlert("Бронювання", $"Забронювати книгу \"{selectedBook.NameOfBook}\"?", "Так", "Ні");
                if (reserve)
                {
                    try
                    {
                        var library = Library.Instance;
                        var user = library.GetSimpleUserByLogin(simpleUser.login_);
                        var book = library.GetBookByTitleAndAuthor(selectedBook.NameOfBook, selectedBook.FullNameOfAuthor);
                        library.ReserveBook(user, book, DateTime.Now);
                        Serializer.SaveToXml(library.ToDTO(), libraryPath);

                        await DisplayAlert("Успіх", $"Книгу \"{selectedBook.NameOfBook}\" заброньовано!", "OK");
                        LoadBooks();

                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Помилка", $"Не вдалося забронювати книгу: {ex.Message}", "OK");
                    }
                }

                ((ListView)sender).SelectedItem = null;
            }
        }
    }
}