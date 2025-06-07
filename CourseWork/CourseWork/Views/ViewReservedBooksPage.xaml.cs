using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace CourseWork.Views
{
    public partial class ViewReservedBooksPage : ContentPage
    {
        private readonly SimpleUser simpleUser;
        private readonly string libraryPath = "/Users/lizazalozna/Projects/CourseWork/library.xml";
        private ObservableCollection<BookDTO> books;

        public ViewReservedBooksPage(SimpleUser simpleUser)
        {
            InitializeComponent();
            books = new ObservableCollection<BookDTO>();
            this.simpleUser = simpleUser;
            LoadBooks();
            BooksListView.ItemsSource = books;
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

                var user = Library.Instance.GetSimpleUserByLogin(simpleUser.login_);

                foreach (var reservedBook in user.reservedBooks_)
                {
                    books.Add(reservedBook.ToDTO());
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Помилка", $"Не вдалося завантажити книги: {ex.Message}", "OK");
            }
        }

        private async void BooksListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is BookDTO selectedBook)
            {
                bool confirm = await DisplayAlert("Скасувати резервування",
                    $"Ви дійсно хочете скасувати резервування книги \"{selectedBook.NameOfBook}\"?", "Так", "Ні");

                if (confirm)
                {
                    try
                    {
                        var library = Library.Instance;
                        var user = library.GetSimpleUserByLogin(simpleUser.login_);

                        var reservedBook = library.GetBookByTitleAndAuthor(selectedBook.NameOfBook, selectedBook.FullNameOfAuthor);

                        library.CancelReservation(user, reservedBook);
                        Serializer.SaveToXml(library.ToDTO(), libraryPath);
                        LoadBooks();
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Помилка", $"Не вдалося скасувати резервування: {ex.Message}", "OK");
                    }
                }
            ((ListView)sender).SelectedItem = null;
            }
        }
    }
}