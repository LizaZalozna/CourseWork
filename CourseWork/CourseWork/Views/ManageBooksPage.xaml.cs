using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace CourseWork.Views
{
    public partial class ManageBooksPage : ContentPage
    {
        private ObservableCollection<BookDTO> books;
        private readonly string libraryPath = "/Users/lizazalozna/Projects/CourseWork/library.xml";

        public ManageBooksPage()
        {
            InitializeComponent();
            books = new ObservableCollection<BookDTO>();
            LoadBooks();
            GenrePicker.ItemsSource = Enum.GetNames(typeof(BookGenre.LiteraryGenre));
            BooksListView.ItemsSource = books;
        }

        private void LoadBooks()
        {
            try
            {
                if (File.Exists(libraryPath))
                {
                    var library = Serializer.LoadFromXml<LibraryDTO>(libraryPath);
                    if (library.Books != null)
                    {
                        books.Clear();
                        foreach (var book in library.Books)
                        {
                            books.Add(book);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Помилка", $"Помилка завантаження книг: {ex.Message}", "OK");
            }
        }

        private async void OnAddBookClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FullNameEntry.Text) ||
                string.IsNullOrWhiteSpace(NameOfBookEntry.Text) ||
                GenrePicker.SelectedItem == null) 
            {
                await DisplayAlert("Помилка", "Будь ласка, заповніть всі поля", "OK");
                return;
            }

            try
            {
                var library = File.Exists(libraryPath)
                    ? Serializer.LoadFromXml<LibraryDTO>(libraryPath)
                    : new LibraryDTO { Books = new System.Collections.Generic.List<BookDTO>() };

                if (library.Books.Any(b => b.NameOfBook == NameOfBookEntry.Text))
                {
                    await DisplayAlert("Помилка", "Книга з такою назвою вже існує", "OK");
                    return;
                }
                var newBook = new BookDTO
                {
                    FullNameOfAutor = FullNameEntry.Text,
                    NameOfBook = NameOfBookEntry.Text,
                    Genre = (BookGenre.LiteraryGenre)Enum.Parse(
                    typeof(BookGenre.LiteraryGenre), GenrePicker.SelectedItem.ToString()),
                    IsAvailable = true,
                    IsReserved = false,
                    ReservedByLogin = null
                };

                library.Books.Add(newBook);
                Serializer.SaveToXml(library, libraryPath);
                FullNameEntry.Text = string.Empty;
                NameOfBookEntry.Text = string.Empty;
                GenrePicker.SelectedItem = null;

                LoadBooks();
                await DisplayAlert("Успіх", "Книгу додано успішно", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Помилка", $"Помилка додавання книги: {ex.Message}", "OK");
            }
        }
    }
}