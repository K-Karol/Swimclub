using Swimclub.Mobile.Services;
using Swimclub.Mobile.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Swimclub.Mobile
{
	public partial class App : Application
	{

		public App()
		{
			InitializeComponent();

			//DependencyService.Register<MockDataStore>();
			DependencyService.Register<Services.RestService>();
			DependencyService.Register<Services.ConfigurationService>();
			IRestService restService = DependencyService.Get<IRestService>();
			IConfigurationService configService = DependencyService.Get<IConfigurationService>();
			if (!Application.Current.Properties.ContainsKey("API_URL"))
			{
				restService.ApiUrl = configService.ConfigValues["apiIP"];
				Application.Current.Properties["API_URL"] = configService.ConfigValues["apiIP"];
				Application.Current.SavePropertiesAsync();
			}
			else
			{
				restService.ApiUrl = (string)Application.Current.Properties["API_URL"];
			}

			MainPage = new NavigationPage(new Views.LoginPage(false));
			
        }

		protected override void OnStart()
		{
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
