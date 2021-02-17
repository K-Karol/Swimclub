using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Swimclub.Mobile.ViewModels
{
    class StudentDataViewModel : BaseViewModel
    {
        private readonly Services.IRestService restService;
        private Swimclub.Models.Student[] Students = new Swimclub.Models.Student[] { };

        public Command LoadData { get; }

        private StudentTemp selected;

        public StudentTemp SelectedStudent
        {
            get { return selected; }
            set { SetProperty(ref selected, value); }
        }

         ObservableCollection<StudentTemp> students = new ObservableCollection<StudentTemp>();

         public ObservableCollection<StudentTemp> StudentsCollection { get { return students; } }


         private bool loading;
         public bool isLoading
		 {
            get { return loading; }
            set { SetProperty(ref loading, value);  }
		 }

         public StudentDataViewModel() //consturctor
         {
            restService = DependencyService.Get<Services.IRestService>();
            LoadData = new Command(loadData);
            isLoading = false;
            Task.Run(() => loadDataAsync());
         }

         private async Task loadDataAsync()
         {
             loadData();
         }

         private void loadData()
         {
            isLoading = false;

             Services.AllStudentsReturn ret;
             Task<Services.AllStudentsReturn> task = Task.Run(() => restService.GetAllStudentsAsync());
             task.ContinueWith(t => Device.BeginInvokeOnMainThread(
                 async () =>
                 {
                     ret = t.Result;
                     if (!ret.Success)
                     {
                         return;
                     }
                     Students = ret.Students;

                     students.Clear();

                     students.Add(new StudentTemp() { Forename="New Student", CurrentGradeNumber=1, newStudent = true});

                     foreach (var s in Students)
                     {
                         StudentTemp st = new StudentTemp(s);
                         st.newStudent = false;
                         students.Add(st);
                     }

                     isLoading = true;
                 }
                 ));
         }

        private Models.Student student;
        public Models.Student Student
        {
            get { return student; }
            set { SetProperty(ref student, value); }
        }
    }

    public class StudentTemp : Models.Student
	{
        public bool newStudent;

        public StudentTemp()
		{

		}

        public StudentTemp(Swimclub.Models.Student _s)
        {
            this.Forename = _s.Forename;
            this.Surname = _s.Surname;
            this.SwimEnglandNumber = _s.SwimEnglandNumber;
            this.MedicalDetails = _s.MedicalDetails;
            this.DateOfBirth = _s.DateOfBirth;
            this.CurrentGradeNumber = _s.CurrentGradeNumber;
        }

        public static Models.Student ConvertToStudent(StudentTemp temp)
        {
            return new Models.Student()
            {
                Forename = temp.Forename,
                Surname = temp.Surname,
                SwimEnglandNumber = temp.SwimEnglandNumber,
                MedicalDetails = temp.MedicalDetails,
                DateOfBirth = temp.DateOfBirth,
                CurrentGradeNumber = temp.CurrentGradeNumber
            };
        }
    }
}
