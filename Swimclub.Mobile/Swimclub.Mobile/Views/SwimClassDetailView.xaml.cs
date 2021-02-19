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
	public partial class SwimClassDetailView : ContentPage
	{
		private ViewModels.SwimClassDetailViewModel _viewModel;
		public SwimClassDetailView(ViewModels.ClassesTemp p_class)
		{
			_viewModel = new ViewModels.SwimClassDetailViewModel();
			InitializeComponent();
			_viewModel._Class = p_class;
			BindingContext = _viewModel;
		}
	}
}