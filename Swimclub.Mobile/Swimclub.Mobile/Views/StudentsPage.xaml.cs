using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Swimclub.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentsPage : ContentPage {
        public IList<Student> Ilist { get; set; }
        public StudentsPage()
        {
            Ilist = new List<Student>();
            Ilist.Add(new Student { StudentName = "Joe Bloggs", StudentGrade = "Grade: 1", StudentSwimEnglandNumber = "12345678" });
            Ilist.Add(new Student { StudentName = "Micheal Phelps", StudentGrade = "Grade: 1", StudentSwimEnglandNumber = "87654321" });
            Ilist.Add(new Student { StudentName = "Alove Floats", StudentGrade = "Grade: 2", StudentSwimEnglandNumber = "67284153" });
            Ilist.Add(new Student { StudentName = "Ivanha swim", StudentGrade = "Grade: 2", StudentSwimEnglandNumber = "06417324" });
            Ilist.Add(new Student { StudentName = "Channel Crosser", StudentGrade = "Grade: 3", StudentSwimEnglandNumber = "14285097" });
            Ilist.Add(new Student { StudentName = "Speedboat McGee", StudentGrade = "Grade: 3", StudentSwimEnglandNumber = "14285097" });

            InitializeComponent();

            Coll1.ItemsSource = Ilist;
        }

        private void StudentPageSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchResultName = Ilist.Where(c => c.StudentName.ToLower().Contains(StudentSearchBar.Text.ToLower()) || c.StudentSwimEnglandNumber.Contains(StudentSearchBar.Text));
            Coll1.ItemsSource = searchResultName;

            
        }
    }
    public class Student
    {
        public string StudentName { get; set; }
        public string StudentGrade { get; set; }
        public string StudentSwimEnglandNumber { get; set; }

    }
}