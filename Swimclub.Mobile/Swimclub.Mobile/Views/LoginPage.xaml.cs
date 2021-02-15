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
			activityIndicator.IsRunning = true;
			Task<bool> loginSuccessful = Task.Run(() => restService.LoginAsync(new Swimclub.Models.Login() { username = username, password = password }));
			loginSuccessful.ContinueWith(success => Device.BeginInvokeOnMainThread(
				async () => {
					activityIndicator.IsRunning = false;
					if (success.Result)
					{
						App.Current.MainPage = new AppShell();
						await DisplayAlert("Login Successfull", "You have logged in", "Hooray!");

					}
					else
					{
						await DisplayAlert("Login Failed", "Either the credentials are incorrect or the server is offline", "Oh no!");
					}

				}
				));
			
		}

	}
}