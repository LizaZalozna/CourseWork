using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Xamarin.Forms;

namespace CourseWork.Views
{	
	public partial class ViewLendedBooksPage : ContentPage
	{
        private readonly SimpleUser simpleUser;
        private readonly string libraryPath = "/Users/lizazalozna/Projects/CourseWork/library.xml";
        private ObservableCollection<BookDTO> books;

        public ViewLendedBooksPage(SimpleUser simpleUser)
		{
            InitializeComponent();
            this.simpleUser = simpleUser;
            books = new ObservableCollection<BookDTO>();
            BooksListView.ItemsSource = books;
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
                var user = Library.Instance.GetSimpleUserByLogin(simpleUser.login_);

                foreach (var lendedBook in user.lendedBooks_)
                {
                    books.Add(lendedBook.ToDTO());
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Помилка", $"Не вдалося завантажити книги: {ex.Message}", "OK");
            }
        }
    }
}