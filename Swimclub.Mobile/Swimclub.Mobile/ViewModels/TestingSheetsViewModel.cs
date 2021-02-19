using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Swimclub.Mobile.ViewModels
{
	public class TestingSheetsViewModel : BaseViewModel
	{

		private readonly Services.IRestService restService;
		private Swimclub.Models.Grade[] grades = new Swimclub.Models.Grade[] { };

		private ObservableCollection<Models.Grade> gradesCollection = new ObservableCollection<Models.Grade>();
		public ObservableCollection<Models.Grade> GradesCollection { get { return gradesCollection; } }

		private Models.Grade selectedGrade;
		public Models.Grade SelectedGrade
		{
			get { return selectedGrade; }
			set { SetProperty(ref selectedGrade, value); isGradeSelected = !(value == null); }
		}

		private Models.Student backup;

		private Models.Student selectedStudent;
		public Models.Student SelectedStudent
		{
			get { return selectedStudent; }
			set { SetProperty(ref selectedStudent, value); if (value != null) backup = new Models.Student() { ID = value.ID, CurrentGradeNumber = value.CurrentGradeNumber, DateOfBirth = value.DateOfBirth, Forename = value.Forename, MedicalDetails = value.MedicalDetails, Surname = value.Surname, SwimEnglandNumber = value.SwimEnglandNumber }; }
		}

		private Models.StudentGradeTests sgt;
		public Models.StudentGradeTests CurrentStudentGradeTest
		{
			get { return sgt; }
			set { SetProperty(ref sgt, value); }
		}

		public bool isgrade;
		public bool isGradeSelected
		{
			get { return isgrade; }
			set { SetProperty(ref isgrade, value); }
		}


		public Command LoadAll { get; }
		public Command Save { get; }
		private bool refreshing = false;
		public bool isRefreshing
		{
			get { return refreshing; }
			set { SetProperty(ref refreshing, value); }
		}

		public TestingSheetsViewModel()
		{
			restService = DependencyService.Get<Services.IRestService>();
			LoadAll = new Command(loadData);
			Save = new Command(save);
			Task.Run(() => loadDataAsync());
		}

		public Models.Student getBackup()
		{
			return backup;
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

		private void save()
		{
			isRefreshing = true;
			Models.ModifyStudentGradeTestResponse resp;
			Task<Models.ModifyStudentGradeTestResponse> task = Task.Run(() => restService.ModifySGTest(CurrentStudentGradeTest));
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

					if (resp.AdvancedGrade)
					{
						if (resp.MaxGrade)
						{
							await App.Current.MainPage.DisplayAlert("Finished last grade", $"{resp.Student.itemValue.Forename} has completed the last grade!", "WooHooo!");
						}
						else
						{
							int lastGrade = resp.Student.itemValue.CurrentGradeNumber - 1;
							
							await App.Current.MainPage.DisplayAlert("Grade completed", $"{resp.Student.itemValue.Forename} has completed the {lastGrade} grade and moved onto Grade {resp.Student.itemValue.CurrentGradeNumber}!", "WooHooo!");
						}
					}

					isRefreshing = false;

					await AppShell.Current.GoToAsync($"//AboutPage");
				}
			  ));
		}


	}


	
}
