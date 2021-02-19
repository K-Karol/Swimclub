using Swimclub.Mobile.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Swimclub.Mobile.ViewModels
{
    public class SwimCLassViewModel:BaseViewModel
    {
        private readonly Services.IRestService restService;

        private Swimclub.Models.Class[] Classes = new Swimclub.Models.Class[] { };
        ObservableCollection<ClassesTemp> classes = new ObservableCollection<ClassesTemp>();
        public ObservableCollection<ClassesTemp> ClassesCollection { get { return classes; } }

        public Command AddSwimCLassCommand { get; }


        public Command LoadData { get; }
        public Command SelectClass { get; }

        private ClassesTemp selectedClass;
        public ClassesTemp SelectedClass
		{
			get { return selectedClass; }
			set { SetProperty(ref selectedClass, value); }
		}



        private async void OnAddClass(object obj)
        { await Shell.Current.Navigation.PushAsync(new NewClass()) ; }

        private bool refreshing = false;
        public bool isRefreshing
        {
            get { return refreshing; }
            set { SetProperty(ref refreshing, value); }
        }


        public SwimCLassViewModel()
        {
            restService = DependencyService.Get<Services.IRestService>();
            AddSwimCLassCommand = new Command(OnAddClass);
            LoadData = new Command(loadData);
            SelectClass = new Command(selectClass);
            Task.Run(() => loadDataAsync());
        }

        private async Task loadDataAsync()
        {
            isRefreshing = true;
            loadData();
        }

        private void selectClass()
		{
            Shell.Current.Navigation.PushAsync(new Views.SwimClassDetailView(SelectedClass));
        }


        private void loadData()
        {
            isRefreshing = true;
            Models.AllClassResponse resp;
            Task<Models.AllClassResponse> task = Task.Run(() => restService.GetAllClasses());
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

                    if (resp.Classes == null || resp.Classes.length == 0 || resp.Classes.values == null) { isRefreshing = false; return; }

                    Classes = resp.Classes.values;


                    classes.Clear();



                    foreach (var c in Classes)
                    {
                        ClassesTemp ct = new ClassesTemp(c);
                        classes.Add(ct);
                    }
                    isRefreshing = false;
                }
              ));
        }


    }

    public class ClassesTemp : Models.Class
    {
        public bool[] AttendanceArray
		{
			get { return getAARR(); }
		}

        public ClassesTemp()
        {

        }

        private bool[] getAARR()
		{
            List<bool> tempAttendance = new List<bool>();
            foreach (var s in Students)
            {
                tempAttendance.Add(Attendance[s.ID]);
            }
            return tempAttendance.ToArray();
        }

        private void setAARR(bool[] temp)
		{
            Dictionary<int, bool> tempAtt = new Dictionary<int, bool>();
            for (int i = 0; i < Students.Length; i++)
            {
                Models.Student sMod = Students[i];
                tempAtt.Add(sMod.ID, temp[i]);

            }
            Attendance = tempAtt;
        }

        public ClassesTemp(Swimclub.Models.Class _s)
        {
            this.ID = _s.ID;
            this.Pool = _s.Pool;
            this.coach = _s.coach;
            this.ClassGrade = _s.ClassGrade;
            this.TimeOfClass = _s.TimeOfClass;
            this.Attendance = _s.Attendance;
            this.Students = _s.Students;
        }

        public static Models.Class ConvertToClass(ClassesTemp temp)
        {
            return new Models.Class()
            {
                ID = temp.ID,
                Pool = temp.Pool,
                coach = temp.coach,
                ClassGrade = temp.ClassGrade,
                TimeOfClass = temp.TimeOfClass,
                Attendance = temp.Attendance,
                Students = temp.Students
            };
        }
    }

}
