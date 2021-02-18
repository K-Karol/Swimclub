using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Swimclub.Mobile.ViewModels
{
	public class LoginViewModel : BaseViewModel
	{
		
		private readonly Services.IRestService restService;

		private Models.Login login;
		public Models.Login Login
		{
			get { return login; }
			set { SetProperty(ref login, value); }
		}

		private bool refreshing = false;
		public bool isRefreshing
		{
			get { return refreshing; }
			set { SetProperty(ref refreshing, value); }
		}

		private string ip;

		public string IP
		{
			get { return ip; }
			set { SetProperty(ref ip, value); }
		}

		public Command LogInUser { get; }
		public Command GoToSettings { get; }

		public LoginViewModel()
		{
			restService = DependencyService.Get<Services.IRestService>();
			IP = (string)Application.Current.Properties["API_URL"];
			LogInUser = new Command(loginUser);
			GoToSettings = new Command(goToSettings);
			login = new Models.Login();
		}

		private void loginUser()
		{
			if (isRefreshing) return;

			isRefreshing = true;
			Models.AuthResponse resp;
			Task<Models.AuthResponse> loginTask = Task.Run(() => restService.LoginAsync(Login));
			loginTask.ContinueWith(t => Device.BeginInvokeOnMainThread(
				async () => {
					isRefreshing = false;
					if (t.IsFaulted) { await App.Current.MainPage.DisplayAlert("Connection error", "There was an error connecting to the server", "Try Again"); return; }
					resp = t.Result;
					if (resp.Success)
					{
						AppShell appShell = new AppShell(restService.Role == "Admin");
						App.Current.MainPage = appShell;
					}
					else
					{
						await App.Current.MainPage.DisplayAlert(resp.Error.Message, resp.Error.Detail, "Try Again");
					}

				}
				));
		}

		private void goToSettings()
		{
			Application.Current.MainPage.Navigation.PushAsync(new Views.SettingsPage());
		}
	}
}
