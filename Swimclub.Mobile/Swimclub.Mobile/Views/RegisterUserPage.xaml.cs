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
	public partial class RegisterUserPage : ContentPage
	{
		private ViewModels.RegisterUserViewModel _viewModel;
		public RegisterUserPage()
		{
			_viewModel = new ViewModels.RegisterUserViewModel();
			InitializeComponent();
			BindingContext = _viewModel;
		}
	}
}