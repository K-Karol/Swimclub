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

    public partial class StudentData : ContentPage
    {
        private ViewModels.StudentDataViewModel _viewModel;

        //private Models.Student student = null;
        public StudentData()
        {
            InitializeComponent();
            _viewModel = new ViewModels.StudentDataViewModel();
            BindingContext = _viewModel;
        }

    }
}