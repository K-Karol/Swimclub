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
	public partial class AllGrades : ContentPage
	{
		private ViewModels.GradeViewModel _viewModel;
		
		public AllGrades()
		{
			InitializeComponent();
			_viewModel = new ViewModels.GradeViewModel();
			BindingContext = _viewModel;
		}

		//protected override void OnAppearing()
		//{
		//	base.OnAppearing();
		//	_viewModel.OnAppearing();
		//}


	}
}