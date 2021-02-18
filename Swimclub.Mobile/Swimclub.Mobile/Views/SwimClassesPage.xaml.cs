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
    public partial class SwimClassesPage : ContentPage
    {
        public SwimClassesPage()
        {
            InitializeComponent();
            //BindingContext = new SwimCLassViewModel();
        }

        private void CreateAClass(object sender, EventArgs e)
        {
            Shell.Current.Navigation.PushAsync(new NewClass());
        }
    }
}