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
		private bool running = false;
		private readonly Services.IRestService restService;
		public LoginPage(bool breach)
		{
			InitializeComponent();
			if (breach) DisplayAlert("Security Breach", "You've accessed the page not as an admin. You have been logged to enforce the security policy", "OK???");
			restService = DependencyService.Get<Services.IRestService>();
		}

		private void Button_Clicked(object sender, EventArgs e)
		{
			if (running)
				return;

			running = true;
			string username = usernameEntry.Text;
			string password = passwordEntry.Text;
			activityIndicator.IsRunning = true;
			Models.AuthResponse resp;
			Task<Models.AuthResponse> loginTask = Task.Run(() => restService.LoginAsync(new Swimclub.Models.Login() { username = username, password = password }));
			loginTask.ContinueWith(t => Device.BeginInvokeOnMainThread(
				async () => {
					activityIndicator.IsRunning = false;
					running = false;

					resp = t.Result;
					if (resp.Success)
					{
						AppShell appShell = new AppShell(restService.Role == "Admin");
						App.Current.MainPage = appShell;
					}
					else
					{
						await DisplayAlert(resp.Error.Message, resp.Error.Detail, "Try Again");
					}

				}
				));
			
		}

	}
}