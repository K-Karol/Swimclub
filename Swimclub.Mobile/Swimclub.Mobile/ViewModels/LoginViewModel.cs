using Swimclub.Mobile.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Swimclub.Mobile.ViewModels
{
	public class LoginViewModel : BaseViewModel
	{
		public Command LoginCommand { get; }

		public LoginViewModel()
		{
			LoginCommand = new Command(OnLoginClicked);

			tempDatabase = new string[5][];
		}

		//example database replace with real one once its constructed
		string[][] tempDatabase;

		private async void OnLoginClicked(object obj)
		{
			for(int i = 0; i<tempDatabase.Length-1; i++)
            {
                if(i == 0)
                {

                }
            }


			// Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
			await Shell.Current.GoToAsync($"//{nameof(AboutPage)}"); //change this to main page later
		}

	}
}
