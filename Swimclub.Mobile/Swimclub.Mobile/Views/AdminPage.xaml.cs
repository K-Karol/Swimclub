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
	public partial class AdminPage : ContentPage
	{
		private ViewModels.AdminPageViewModel _viewModel;
		public AdminPage()
		{
			_viewModel = new ViewModels.AdminPageViewModel();
			InitializeComponent();

			BindingContext = _viewModel;
		}

		
		protected override void OnAppearing()
		{
			Services.IRestService s = DependencyService.Get<Services.IRestService>();
			if(s.Role != "Admin")
				App.Current.MainPage = new Views.LoginPage(true);
		}
	}
}