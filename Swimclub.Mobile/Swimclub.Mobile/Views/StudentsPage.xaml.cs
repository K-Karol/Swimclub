using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Swimclub.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentsPage : ContentPage {
        //public IList<Student> Ilist { get; set; }
        private readonly Services.IRestService restService;
        private Swimclub.Models.Student[] Students = new Swimclub.Models.Student[] { };


        ObservableCollection<Swimclub.Models.Student> students = new ObservableCollection<Swimclub.Models.Student>();
        public ObservableCollection<Swimclub.Models.Student> StudentsView { get { return students; } }

        public StudentsPage()
        {
            restService = DependencyService.Get<Services.IRestService>();
            InitializeComponent();
            studentList.ItemsSource = students;
            studentList.IsRefreshing = true;
            Task.Run(loadData);
        }

        private void StudentPageSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            //var searchResultName = Ilist.Where(c => c.StudentName.ToLower().Contains(StudentSearchBar.Text.ToLower()) || c.StudentSwimEnglandNumber.Contains(StudentSearchBar.Text));
            //studentList.ItemsSource = searchResultName;
        }

        private async Task loadData()
		{
            Students = await restService.GetAllStudentsAsync();
            foreach (var s in Students)
            {
                students.Add(s);
            }
            studentList.IsRefreshing = false;
        }

    }
}


//Swimclub.Models.Student[] students = await restService.GetAllStudentsAsync();