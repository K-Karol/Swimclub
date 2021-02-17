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
			MainPage = new Views.LoginPage(false);
			
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
