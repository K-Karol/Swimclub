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
    public partial class NewClass : ContentPage
    {
        private ViewModels.NewClassViewModel _viewModel;
        public NewClass()
        {
            _viewModel = new ViewModels.NewClassViewModel();
            InitializeComponent();
            BindingContext = _viewModel;
        }
    }
}