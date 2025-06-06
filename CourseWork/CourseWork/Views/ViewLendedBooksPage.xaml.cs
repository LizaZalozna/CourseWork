using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace CourseWork.Views
{	
	public partial class ViewLendedBooksPage : ContentPage
	{
        private readonly SimpleUser simpleUser;

        public ViewLendedBooksPage(SimpleUser simpleUser)
		{
			InitializeComponent ();
			this.simpleUser = simpleUser;
            BooksListView.ItemsSource = simpleUser.lendedBooks_;
            LendingListView.ItemsSource = simpleUser.lendings_;
        }
    }
}