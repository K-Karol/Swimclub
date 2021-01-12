using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Swimclub.Mobile.ViewModels
{


    class MainViewModel : BaseViewModel
    {
        public Command SaveInput { get; }
        public Command ClearInput { get; }

        MainViewModel()
        {

        }

    }

}
