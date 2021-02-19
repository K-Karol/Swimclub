using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Swimclub.Mobile.ViewModels
{
    class NewClassViewModel : BaseViewModel
    {

		private bool refreshing = false;
		public bool isRefreshing
		{
			get { return refreshing; }
			set { SetProperty(ref refreshing, value); }
		}

		private readonly Services.IRestService restService;

		private Swimclub.Models.Grade[] grades = new Swimclub.Models.Grade[] { };


		private ObservableCollection<Models.Grade> gradesCollection = new ObservableCollection<Models.Grade>();
		public ObservableCollection<Models.Grade> GradesCollection { get { return gradesCollection; } }

		private Models.Grade selcGrade;
		public Models.Grade SelectedGrade
		{
			get { return selcGrade; }
			set { SetProperty(ref selcGrade, value); }
		}

		private Models.Class _class;

		public Models.Class _Class
		{
			get { return _class; }
			set { SetProperty(ref _class, value); }
		}

		public Command SubmitClass { get; }

		public NewClassViewModel()
		{
			restService = DependencyService.Get<Services.IRestService>();
			SubmitClass = new Command(submitClass);
			_Class = new Models.Class() { coach = restService.CurrentUser };
			Task.Run(() => loadDataAsync());
		}

		private async Task loadDataAsync()
		{
			isRefreshing = true;
			loadData();
		}

		private void loadData()
		{
			isRefreshing = true;
			Models.AllGradesResponse resp;
			Task<Models.AllGradesResponse> task = Task.Run(() => restService.GetAllGradesAsync());
			task.ContinueWith(t => Device.BeginInvokeOnMainThread(
				async () =>
				{
					if (t.IsFaulted) await App.Current.MainPage.DisplayAlert("Connection error", "There was an error connecting to the server", "Try Again");
					resp = t.Result;
					if (!resp.Success)
					{
						isRefreshing = false;
						await App.Current.MainPage.DisplayAlert(resp.Error.Message, resp.Error.Detail, "Try Again");
						return;
					}
					grades = resp.Grades.values;


					gradesCollection.Clear();



					foreach (var g in grades)
					{
						gradesCollection.Add(g);
					}
					isRefreshing = false;
				}
			  ));
		}


		private void submitClass()
		{
			if (isRefreshing) return;
			isRefreshing = true;

			Models.AddClassResponse resp;
			_class.Students = SelectedGrade.Students;
			Dictionary<int, bool> temp = new Dictionary<int, bool>();
			foreach(var s in _class.Students)
			{
				temp.Add(s.ID, false);
			}
			_class.Attendance = temp;
			Task<Models.AddClassResponse> task = Task.Run(() => restService.CreateNewClass(_Class));
			task.ContinueWith(t => Device.BeginInvokeOnMainThread(
				async () =>
				{
					if (t.IsFaulted) await App.Current.MainPage.DisplayAlert("Connection error", "There was an error connecting to the server", "Try Again");
					resp = t.Result;
					if (!resp.Success)
					{
						isRefreshing = false;
						await App.Current.MainPage.DisplayAlert(resp.Error.Message, resp.Error.Detail, "Try Again");
						return;
					}
					isRefreshing = false;
					await App.Current.MainPage.DisplayAlert("Success", "Class added successfully", "Ok");
					await AppShell.Current.Navigation.PopAsync();
				}
			  ));
		}

	}
}
