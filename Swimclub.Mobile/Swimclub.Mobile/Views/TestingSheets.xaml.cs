using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Swimclub.Mobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TestingSheets : ContentPage
	{
		private readonly Services.IRestService restService;

		private Swimclub.Models.Grade[] grades = new Swimclub.Models.Grade[] { };

		private Models.Grade selectedGrade = null;
		private Models.Student selectedStudent = null;
		private Models.StudentGradeTests sgt = null;
		public TestingSheets()
		{
			InitializeComponent();
			restService = DependencyService.Get<Services.IRestService>();
			Task.Run(() => loadData());
			
		}

		private void loadData()
		{
			refView.IsRefreshing = true;
			Models.AllGradesResponse resp;
			Task<Models.AllGradesResponse> task = Task.Run(() => restService.GetAllGradesAsync());
			task.ContinueWith(t => Device.BeginInvokeOnMainThread(
				async () =>
				{
					if (t.IsFaulted) await App.Current.MainPage.DisplayAlert("Connection error", "There was an error connecting to the server", "Try Again");
					resp = t.Result;
					if (!resp.Success)
					{
						refView.IsRefreshing = false;
						await App.Current.MainPage.DisplayAlert(resp.Error.Message, resp.Error.Detail, "Try Again");
						return;
					}
					grades = resp.Grades.values;


					refView.IsRefreshing = false;
					gradPicker.ItemsSource = grades;
				}
			  ));
		}

		private void loadSTG()
		{
			Models.StudentGradeTestsResponse resp;
			Task<Models.StudentGradeTestsResponse> task = Task.Run(() => restService.GetCurrentStudentGradeTestByID(new Models.StudentGradeTestRequest() { ID = selectedStudent.ID }));
			task.ContinueWith(t => Device.BeginInvokeOnMainThread(
				async () =>
				{
					if (t.IsFaulted) await DisplayAlert("Connection error", "There was an error connecting to the server", "Try Again");
					resp = t.Result;
					if (!resp.Success)
					{
						await DisplayAlert(resp.Error.Message, resp.Error.Detail, "Try Again");
						return;
					}

					sgt = resp.StudentGradeTests.itemValue ;

					testsColl.ItemsSource = sgt.TestAttempts;
				}
			  ));
		}

		private void saveButton_Clicked(object sender, EventArgs e)
		{
			Models.ModifyStudentGradeTestResponse resp;
			Task<Models.ModifyStudentGradeTestResponse> task = Task.Run(() => restService.ModifySGTest(sgt));
			task.ContinueWith(t => Device.BeginInvokeOnMainThread(
				async () =>
				{
					if (t.IsFaulted) await App.Current.MainPage.DisplayAlert("Connection error", "There was an error connecting to the server", "Try Again");
					resp = t.Result;
					if (!resp.Success)
					{
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


					await AppShell.Current.GoToAsync($"//AboutPage");
				}
			  ));
		}

		private void studentPickers_SelectedIndexChanged(object sender, EventArgs e)
		{
			testsColl.ItemsSource = null;
			Models.Student s = studentPickers.SelectedItem as Models.Student;
			if(s != null)
			{
				selectedStudent = s;
				Task.Run(() => loadSTG());
			}
		}

		private void gradPicker_SelectedIndexChanged(object sender, EventArgs e)
		{
			testsColl.ItemsSource = null;
			studentPickers.ItemsSource = null;
			studentPickers.IsEnabled = false;
			Models.Grade g = gradPicker.SelectedItem as Models.Grade;
			if(g != null) {
				selectedGrade = g;
				studentPickers.IsEnabled = true;
				studentPickers.ItemsSource = selectedGrade.Students;
			}
			else
			{
				selectedGrade = null;
				studentPickers.IsEnabled = false;
			}
		}

		private void refView_Refreshing(object sender, EventArgs e)
		{
			testsColl.ItemsSource = null;
			studentPickers.ItemsSource = null;
			studentPickers.IsEnabled = false;
			gradPicker.ItemsSource = null;
			Task.Run(() => loadData());
		}
	}

	public class StudentPickerConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Models.Student model = value as Models.Student;
			if (model == null) return " ";

			return $"{model.Forename} {model.Surname} [SE:{model.SwimEnglandNumber}]";

		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}