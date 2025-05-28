using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xamarin.Forms;

namespace CourseWork.Views
{
    public partial class AdminPage : ContentPage
    {
        private readonly string libraryPath = "/Users/lizazalozna/Projects/CourseWork/library.xml";

        public AdminPage()
        {
            InitializeComponent();
        }

        private async void OnManageBooksClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ManageBooksPage());
        }

        private async void OnManageLibrariansClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ManageLibrariansPage());
        }

        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Інформація", "Функція налаштувань системи в розробці", "OK");
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