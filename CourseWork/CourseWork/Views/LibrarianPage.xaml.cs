using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace CourseWork.Views
{
    public partial class LibrarianPage : ContentPage
    {
        private readonly Librarian librarian;

        public LibrarianPage(Librarian librarian)
        {
            InitializeComponent();
            this.librarian = librarian;
            WelcomeLabel.Text = $"Вітаємо, Бібліотекар {librarian.fullName_}!";
        }

        private async void OnManageSimpleUsersClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ManageSimpleUsersPage(librarian));
        }

        private async void OnLendBookClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LendBookPage());
        }

        private async void OnReturnBookClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Неможливо", "Функція на етапі розробки", "OK");
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Вихід", "Ви впевнені, що хочете вийти?", "Так", "Ні");
            if (answer)
            {
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
        }
    }
}

