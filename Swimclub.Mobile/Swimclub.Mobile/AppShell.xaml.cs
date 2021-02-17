using Swimclub.Mobile.ViewModels;
using Swimclub.Mobile.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Swimclub.Mobile
{
	public partial class AppShell : Xamarin.Forms.Shell
	{
		private AppShellViewModel _viewModel;
		public AppShell(bool admin)
		{
			_viewModel = new AppShellViewModel();
			_viewModel.isAdmin = admin;
			InitializeComponent();
			BindingContext = _viewModel;
			//Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
			//Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
		}
		//private async void OnMenuItemClicked(object sender, EventArgs e)
		//{
		//	await Shell.Current.GoToAsync("//LoginPage");
		//}
	}
}
