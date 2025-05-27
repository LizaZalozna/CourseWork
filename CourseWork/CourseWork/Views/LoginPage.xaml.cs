using System;
using Xamarin.Forms;

namespace CourseWork.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmailEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                await DisplayAlert("Помилка", "Будь ласка, заповніть всі поля", "OK");
                return;
            }
            try
            {
                await Application.Current.MainPage.DisplayAlert("Завантаження", "Виконується вхід...", "OK");
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Помилка", "Помилка при вході: " + ex.Message, "OK");
            }
        }
    }
} 