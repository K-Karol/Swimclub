using Swimclub.Mobile.ViewModels;
using Swimclub.Mobile.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Swimclub.Mobile
{
	public partial class AppShell : Xamarin.Forms.Shell
	{
		public AppShell()
		{
			InitializeComponent();
			Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
			Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
			Routing.RegisterRoute("//GradingPages", typeof(GradingPages)); //this is needed for the griding page to be displayed
		}

		private async void OnMenuItemClicked(object sender, EventArgs e)
		{
			await Shell.Current.GoToAsync("//LoginPage");
		}
	}
}
