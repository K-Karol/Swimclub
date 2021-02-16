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
	public partial class GradeDetailPage : ContentPage
	{
		private ViewModels.GradeDetailViewModel _viewModel;
		public GradeDetailPage(Models.Grade grade)
		{
			InitializeComponent();
			_viewModel = new ViewModels.GradeDetailViewModel();
			
			_viewModel.Grade = grade;
			BindingContext = _viewModel;
		}
	}
}