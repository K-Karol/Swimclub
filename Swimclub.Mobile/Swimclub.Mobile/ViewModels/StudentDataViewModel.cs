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

        private Models.Student selected;

        public Models.Student SelectedStudent
        {
            get { return selected; }
            set { SetProperty(ref selected, value); }
        }

         ObservableCollection<Models.Student> students = new ObservableCollection<Models.Student>();

         public ObservableCollection<Models.Student> StudentsCollection { get { return students; } }


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

                     students.Add(new Models.Student() { Forename="New Student", CurrentGradeNumber=1});

                     foreach (var s in Students)
                     {
                         students.Add(s);
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

    public class StringArrayToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string temp = "";
            string[] data = (string[])value;
            if(data.Length == 0) { return "";  }
            foreach (var s in data)
            {
                temp += $"{s},";
            }
            temp = temp.Remove(temp.Length  -  1,  1);
            return temp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string data = (string)value;
            string[] temp = data.Split(',');
            return temp;
        }
    }
}
