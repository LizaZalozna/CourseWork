using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xamarin.Forms;

namespace CourseWork.Views
{
    public partial class AdminPage : ContentPage
    {
        private readonly Admin admin;

        public AdminPage(Admin admin)
        {
            InitializeComponent();
            this.admin = admin;
        }

        private async void OnManageBooksClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ManageBooksPage());
        }

        private async void OnManageLibrariansClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ManageLibrariansPage(admin));
        }

        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ManageSettingsPage());
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