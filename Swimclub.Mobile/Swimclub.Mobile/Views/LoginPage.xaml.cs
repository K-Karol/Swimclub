using Swimclub.Mobile.ViewModels;
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
	public partial class LoginPage : ContentPage
	{
		public LoginPage()
		{
			InitializeComponent();
			this.BindingContext = new LoginViewModel();
			// of size 5 x 2 with preinput values, change this la
			tempDatabase = new string[5, 2] { { "username", "password" }, { "JohnLennon", "Submarine" }, { "Karol_Was_Here", "ISw3ar" }, { "hello", "World" }, { "purple", "orange" } };
		}

		//example database replace with real one once its constructed
		string[,] tempDatabase; // 2D array

		/// <summary>
		/// When the login button is pressed, the function compares the username and password to the database, if they match the user is sent to the main page, else a message is displayed 
		/// saying that the user they are trying to enter doesn't exist, the password entry is whiped after every button push
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LoginButton_Clicked(object sender, EventArgs e)
        {
			bool found = false;
			//using simple search as a small amount of total users is expected
			for (int i = 0; i < (tempDatabase.Length) / 2 - 1; i++) // /2 as only need to search one row of data at a time
			{
				if (tempDatabase[i, 0] == UsernameInput.Text)
				{
					if (tempDatabase[i, 1] == PasswordInput.Text)
					{
						found = true;
						Shell.Current.GoToAsync($"//{nameof(MainPage)}");
					}
				}
			}
			//if the system can't fins the user in the database then...
			if (found == false)
            {
				PasswordInput.Text = string.Empty; //emptys the password string 
				DisplayAlert("Incorrect", "This user doesn't exist in the database", "exit");
            }
		}

		//change this later to contact an admin to help then sign in and such
        private void passwordForgoten_Clicked(object sender, EventArgs e)
        {
			DisplayAlert("Password Remainder", "An admin wiil notify you of your paassword at their earliest possible convienience" , "exit");
		}
    }
}