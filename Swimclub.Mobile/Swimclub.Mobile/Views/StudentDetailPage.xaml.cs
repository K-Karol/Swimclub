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
	public partial class StudentDetailPage : ContentPage
	{
		private ViewModels.StudentDetailViewModel _viewModel;
		public StudentDetailPage(Models.Student student)
		{
			InitializeComponent();
			_viewModel = new ViewModels.StudentDetailViewModel();
			_viewModel.Student = student;
			BindingContext = _viewModel;
		}
	}
}