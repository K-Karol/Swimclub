using Swimclub.Mobile.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Swimclub.Mobile.ViewModels
{
    class SwimCLassViewModel:BaseViewModel
    {
        public Command AddSwimCLassCommand { get; }

        public SwimCLassViewModel()
        { AddSwimCLassCommand = new Command(OnAddClass); }

        private async void OnAddClass(object obj)
        { await Shell.Current.GoToAsync(nameof(NewClass)); }
    }
}
