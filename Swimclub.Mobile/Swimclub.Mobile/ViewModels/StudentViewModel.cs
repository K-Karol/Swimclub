using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace Swimclub.Mobile.ViewModels
{
	public class StudentViewModel : BaseViewModel
	{
		private readonly Services.IRestService restService;
		private Swimclub.Models.Student[] Students = new Swimclub.Models.Student[] { };


        public Command LoadData { get; }
        public Command<object> SelectStudent { get; }
		public Command SearchStudents { get; }

		private string searchString;
		public string SearchString
		{
			get { return searchString; }
			set { SetProperty(ref searchString, value); search(); }
		}

        ObservableCollection<StudentCell> students = new ObservableCollection<StudentCell>();
		public ObservableCollection<StudentCell> StudentsCollection { get { return students; } }

		private bool refresh;
		public bool isRefreshing
		{
			get { return refresh; }
			set { SetProperty(ref refresh, value); }
		}

		public StudentViewModel()
		{
			restService = DependencyService.Get<Services.IRestService>();
            LoadData = new Command(loadData);
            SelectStudent = new Command<object>(selectStudent);
			SearchStudents = new Command(search);
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
            Models.AllStudentsResponse resp;
            Task<Models.AllStudentsResponse> task = Task.Run(() => restService.GetAllStudentsAsync());
            task.ContinueWith(t => Device.BeginInvokeOnMainThread(
                async () =>
                {
					resp = t.Result;
                    if (!resp.Success)
                    {
                        isRefreshing = false;
                        await App.Current.MainPage.DisplayAlert(resp.Error.Message, resp.Error.Detail, "Try again");
                        return;
                    }
                    Students = resp.Students.values;

                    students.Clear();



                    foreach (var s in Students)
                    {
						StudentCell cell = new StudentCell(s);
						cell.textColour = Application.Current.Resources["OnSurface"] as Color? ?? Color.Black;
						students.Add(cell);
                    }
                    isRefreshing = false;
					search();

				}
              ));
			
        }

        private void selectStudent(object obj)
        {
			StudentCell currentCell = obj as StudentCell;
			currentCell.textColour = Application.Current.Resources["OnSecondary"] as Color? ?? Color.Black;
			List<StudentCell> temp = students.Except<StudentCell>(new List<StudentCell>() { currentCell }).ToList();
			foreach (var a in temp)
			{
				a.textColour = Application.Current.Resources["OnSurface"] as Color? ?? Color.Black;
			}

			Shell.Current.Navigation.PushAsync(new Views.StudentDetailPage(StudentCell.ConvertToStudent(currentCell)));
		}

		private void search()
		{
			if (searchString == null || searchString == "")
			{
				students.Clear();
				foreach (var s in Students)
				{
					StudentCell cell = new StudentCell(s);
					cell.textColour = Application.Current.Resources["OnSurface"] as Color? ?? Color.Black;
					students.Add(cell);
				}
			}
			else
			{
				string searchStringTemp = searchString.ToLower();
				List<Models.Student> searchResults = Students.Where<Models.Student>(s => $"{s.Forename.ToLower()} {s.Surname.ToLower()}".StartsWith(searchStringTemp) || s.Forename.ToLower().StartsWith(searchStringTemp) || s.Surname.ToLower().StartsWith(searchStringTemp) || s.SwimEnglandNumber.StartsWith(searchStringTemp)).ToList();
				students.Clear();
				foreach (var s in searchResults)
				{
					StudentCell cell = new StudentCell(s);
					cell.textColour = Application.Current.Resources["OnSurface"] as Color? ?? Color.Black;
					students.Add(cell);
				}
			}
		}

	}




	public class StudentCell : Swimclub.Models.Student, INotifyPropertyChanged
	{
		private Color textcol;
		public Color textColour
		{
			get { return textcol; }
			set
			{
				if (textcol != value)
				{
					textcol = value;
					if (PropertyChanged != null)
					{
						PropertyChanged(this, new PropertyChangedEventArgs("textColour"));
					}
				}
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
		//public void OnPropertyChanged()
		//{
		//    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
		//}
		public StudentCell(Swimclub.Models.Student _s)
		{
			this.Forename = _s.Forename;
			this.Surname = _s.Surname;
			this.SwimEnglandNumber = _s.SwimEnglandNumber;
			this.MedicalDetails = _s.MedicalDetails;
			this.DateOfBirth = _s.DateOfBirth;
			this.CurrentGradeNumber = _s.CurrentGradeNumber;
		}

		public static Models.Student ConvertToStudent(StudentCell cell)
		{
			return new Models.Student()
			{
				Forename = cell.Forename,
				Surname = cell.Surname,
				SwimEnglandNumber = cell.SwimEnglandNumber,
				MedicalDetails = cell.MedicalDetails,
				DateOfBirth = cell.DateOfBirth,
				CurrentGradeNumber = cell.CurrentGradeNumber
			};
		}


	}
}
