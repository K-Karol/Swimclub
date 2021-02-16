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
		public LoginPage()
		{
			InitializeComponent();
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
			Task<int> loginSuccessful = Task.Run(() => restService.LoginAsync(new Swimclub.Models.Login() { username = username, password = password }));
			loginSuccessful.ContinueWith(statusCode => Device.BeginInvokeOnMainThread(
				async () => {
					activityIndicator.IsRunning = false;
					running = false;
					int temp = loginSuccessful.Result;
					if (temp == 200)
					{
						AppShell appShell = new AppShell();
						if (restService.Role == "Admin")
						{
							ShellSection adminPage = new ShellSection() { Title = "Admin Page" };
							adminPage.Items.Add(new ShellContent() { Content = new AdminPage() });
							appShell.Items.Add(adminPage);
						}

						App.Current.MainPage = appShell;
						await DisplayAlert("Login Successfull", $"You have logged in with the role of a {restService.Role}", "Hooray!");

					}
					else if(temp == 401)
					{
						await DisplayAlert("Login Failed", "Either the credentials are incorrect or the user does not exist", "Oh no!");
					}
					else
					{
						await DisplayAlert("Login Failed", "Either the server is offline or you are disconnected", "Oh no!");
					}

				}
				));
			
		}

	}
}