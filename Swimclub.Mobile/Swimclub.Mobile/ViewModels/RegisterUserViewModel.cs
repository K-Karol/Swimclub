using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Swimclub.Mobile.ViewModels
{
	public class RegisterUserViewModel : BaseViewModel
	{
		private readonly Services.IRestService restService;

		private Models.Register registerModel;
		public Models.Register RegisterModel
		{
			get { return registerModel; }
			set { SetProperty(ref registerModel, value); }
		}
		private ObservableCollection<string> regErrorsCollection;
		public ObservableCollection<string> RegistrationErrors
		{
			get { return regErrorsCollection; }
			set { SetProperty(ref regErrorsCollection, value); }
		}

		private bool refreshing = false;
		public bool isRefreshing
		{
			get { return refreshing; }
			set { SetProperty(ref refreshing, value); }
		}

		private Color errorCol;
		public Color ErrorColour
		{
			get { return errorCol; }
			set { SetProperty(ref errorCol, value); }
		}

		public Command RegisterUser { get; }

		public RegisterUserViewModel()
		{
			restService = DependencyService.Get<Services.IRestService>();
			registerModel = new Models.Register();
			regErrorsCollection = new ObservableCollection<string>();
			RegisterUser = new Command(regUser);
			errorCol = Color.Red;
			ErrorColour = Color.Red;
			
		}



		public void regUser()
		{
			isRefreshing = true;
			regErrorsCollection.Clear();
			ErrorColour = Color.Red;
			if (registerModel.Forename == null) regErrorsCollection.Add("Forename required");
			if (registerModel.Surname == null) regErrorsCollection.Add("Surname required");
			if (registerModel.Username == null) regErrorsCollection.Add("Username required");
			if (registerModel.Password == null) regErrorsCollection.Add("Password required");
			if (registerModel.Role == null) regErrorsCollection.Add("Role required");
			if (registerModel.Forename == null || registerModel.Surname == null || registerModel.Username == null || registerModel.Password == null || registerModel.Role == null) { isRefreshing = false; return; }

			Models.RegistrationResponse resp;
			Task<Models.RegistrationResponse> task = Task.Run(() => restService.RegisterUser(RegisterModel));
			task.ContinueWith(t => Device.BeginInvokeOnMainThread(
				async () =>
				{
					if (t.IsFaulted) await App.Current.MainPage.DisplayAlert("Connection error", "There was an error connecting to the server", "Try Again");
					resp = t.Result;
					if (!resp.Success)
					{
						isRefreshing = false;
						//if (ret.Errors == null) regErrorsCollection.Add("There was a problem registering the user. Please try again");
						//else
						//{
						//	foreach(var i in ret.Errors)
						//	{
						//		regErrorsCollection.Add(i);
						//	}
						//}

						if((int)resp.Error.Code == (int)Models.ServerResponse.ErrorCodes.PASSWORD_INVALID)
						{
							foreach(var i in resp.PasswordValidationErrors.values)
							{
								regErrorsCollection.Add(i);
							}
						}

						await App.Current.MainPage.DisplayAlert(resp.Error.Message, resp.Error.Detail, "Try Again");
						isRefreshing = false;
						return;
					}
					else
					{
						ErrorColour = Color.Green;
						regErrorsCollection.Add("User registered");
						isRefreshing = false;
						return;
					}
				}
				));
		}
	}
}
