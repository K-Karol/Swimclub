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

		

		private readonly Services.IRestService restService;
		public LoginPage()
		{
			InitializeComponent();
			restService = DependencyService.Get<Services.IRestService>();
		}

		private void Button_Clicked(object sender, EventArgs e)
		{
			string username = usernameEntry.Text;
			string password = passwordEntry.Text;
			Task<bool> loginSuccessful = restService.LoginAsync(new Swimclub.Models.Login() { username = username, password = password });
			loginSuccessful.Wait();
			if (loginSuccessful.Result)
			{
				App.Current.MainPage = new AppShell();
				DisplayAlert("Login status", "Successful", "Hooray!");

			}
			else
			{
				DisplayAlert("Login status", "Failed", "Oh no!");
			}
		}
	}
}