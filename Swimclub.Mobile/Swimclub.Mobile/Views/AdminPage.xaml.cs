﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Swimclub.Mobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AdminPage : ContentPage
	{
		public AdminPage()
		{
			InitializeComponent();
		}

		private void Modify_Student(object sender, EventArgs e)
    {
			Shell.Current.Navigation.PushAsync(new StudentData());
    }
		protected override void OnAppearing()
		{
			Services.IRestService s = DependencyService.Get<Services.IRestService>();
			if(s.Role != "Admin")
				App.Current.MainPage = new Views.LoginPage(true);
		}
	}
}