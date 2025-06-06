using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace CourseWork.Views
{	
	public partial class ViewReservedBooksPage : ContentPage
	{
        private readonly SimpleUser simpleUser;

        public ViewReservedBooksPage(SimpleUser simpleUser)
		{
			InitializeComponent();
			this.simpleUser = simpleUser;
		}
	}
}

