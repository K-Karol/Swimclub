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
	public partial class LoginPage : ContentPage
	{
		private ViewModels.LoginViewModel _viewModel;
		public LoginPage(bool breach)
		{
			_viewModel = new ViewModels.LoginViewModel();
			InitializeComponent();
			if (breach) DisplayAlert("Security Breach", "You've accessed the page not as an admin. You have been logged to enforce the security policy", "Affirmitive");
			BindingContext = _viewModel;
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			_viewModel.IP = (string)Application.Current.Properties["API_URL"];
		}

	}
}