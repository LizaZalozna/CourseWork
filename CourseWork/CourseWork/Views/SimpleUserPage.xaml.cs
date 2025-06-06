﻿using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace CourseWork.Views
{	
	public partial class SimpleUserPage : ContentPage
	{
        private readonly SimpleUser simpleUser;

		public SimpleUserPage(SimpleUser simpleUser)
		{
			InitializeComponent ();
            this.simpleUser = simpleUser;
            WelcomeLabel.Text = $"Вітаємо, Звичайний користувач {simpleUser.fullName_}!";
        }

        private async void OnReserveBookClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ReserveBookPage(simpleUser));
        }

        private async void OnViewReservedBooksClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ViewReservedBooksPage(simpleUser));
        }

        private async void OnViewLendedBooksClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ViewLendedBooksPage(simpleUser));
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

