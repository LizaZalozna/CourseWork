using System;
using System.Collections.Generic;
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
                LibraryDTO dto = File.Exists(libraryPath)
                ? Serializer.LoadFromXml<LibraryDTO>(libraryPath)
                : new LibraryDTO { Users = new List<UserDTO>(), Books = new List<BookDTO>(), Settings = new SettingsDTO() };
                Library library = new Library(dto);
                if (dto.Books.Any(b => b.NameOfBook == NameOfBookEntry.Text))
                {
                    await DisplayAlert("Помилка", "Книга з такою назвою вже існує", "OK");
                    return;
                }
                library.AddBook(new Book(FullNameEntry.Text, NameOfBookEntry.Text, (BookGenre.LiteraryGenre)Enum.Parse(
                    typeof(BookGenre.LiteraryGenre), GenrePicker.SelectedItem.ToString())));
                Serializer.SaveToXml(library.ToDTO(), libraryPath);
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

        private async void BooksListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is BookDTO selectedBook)
            {
                bool confirm = await DisplayAlert("Підтвердження",
                    $"Видалити книгу \"{selectedBook.NameOfBook}\"?", "Так", "Скасувати");

                if (confirm)
                {
                    try
                    {
                        var dto = Serializer.LoadFromXml<LibraryDTO>(libraryPath);
                        dto.Books.RemoveAll(b => b.NameOfBook == selectedBook.NameOfBook && b.FullNameOfAutor == selectedBook.FullNameOfAutor);

                        Serializer.SaveToXml(dto, libraryPath);
                        LoadBooks();

                        await DisplayAlert("Успіх", $"Книгу \"{selectedBook.NameOfBook}\" видалено", "OK");
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