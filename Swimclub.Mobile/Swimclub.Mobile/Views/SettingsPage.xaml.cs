using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Swimclub.Mobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
		private ViewModels.SettingsViewModel _viewModel;
		public SettingsPage()
		{
			_viewModel = new ViewModels.SettingsViewModel();
			InitializeComponent();
			BindingContext = _viewModel;
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			_viewModel.IP = (string)Application.Current.Properties["API_URL"];
		}
	}
}