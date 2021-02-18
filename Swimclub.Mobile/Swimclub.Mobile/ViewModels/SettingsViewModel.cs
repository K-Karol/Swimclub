using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Swimclub.Mobile.ViewModels
{
	public class SettingsViewModel : BaseViewModel
	{
		private Regex ipRX = new Regex(@"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");
		Services.IRestService restService;
		private string ip;
		public string IP
		{
			get { return ip; }
			set { SetProperty(ref ip, value); }
		}

		public Command Save { get;}
		 
		public SettingsViewModel()
		{
			restService = DependencyService.Get<Services.IRestService>();
			ip = (string)Application.Current.Properties["API_URL"];
			Save = new Command(save);
		}

		private async void save()
		{
			MatchCollection matches = ipRX.Matches(IP);
			if(matches.Count >= 1)
			{
				Application.Current.Properties["API_URL"] = matches[0].ToString();
				await Application.Current.SavePropertiesAsync();
				restService.ApiUrl = (string)Application.Current.Properties["API_URL"];
			}
			else
			{
				await Application.Current.MainPage.DisplayAlert("Apply settings error", "The IP is incorrect", "Try Again");
			}
		}
	}
}
