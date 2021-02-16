using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Swimclub.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentsPage : ContentPage {
        private ViewModels.StudentViewModel _viewModel;

        public StudentsPage()
        {
           
            InitializeComponent();

/*
        private void studentList_GoToNewTest(object sender, EventArgs e)
        {
            //check if the last clicked is not null
            if (lastClickedstudent != null)
            {
                if (restService.Role == "Coach")
                { //if not coach display message
                    DisplayAlert("PermitionsCheck", "You do not have the correct premition for this action", "exit");
                }
                else
                { //if coach go to testing page
                    Shell.Current.Navigation.PushAsync(new StudentTestingPage(lastClickedstudent));
                }
            }
            else
            {
                DisplayAlert("Student Check", "You do not select a student for this action, please click a student before trying to proceed", "exit");
            }
        }

        private void StudentPageSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            //var searchResultName = Ilist.Where(c => c.StudentName.ToLower().Contains(StudentSearchBar.Text.ToLower()) || c.StudentSwimEnglandNumber.Contains(StudentSearchBar.Text));
            //studentList.ItemsSource = searchResultName;
        }

        private async Task loadData()
		{
            Students = await restService.GetAllStudentsAsync();
            students.Clear();
            
            foreach (var s in Students)
            {
                StudentCell cell = new StudentCell(s);
                cell.textColour = Application.Current.Resources["OnSurface"] as Color? ?? Color.Black;
                students.Add(cell);
            }
            studentList.IsRefreshing = false;
        }

		private async void studentList_Refreshing(object sender, EventArgs e)
		{
            studentList.IsRefreshing = true;
            await loadData();
        }


        private void studentList_ItemTapped(object sender, ItemTappedEventArgs e)
		{
            StudentCell student = e.Item as StudentCell;
            Swimclub.Models.Student s = student as Swimclub.Models.Student;
            lastClickedstudent = s;

            DisplayAlert("Data",
                String.Format(
                    "Name: {0} {1}" +
                    "\nCurrentGrade: {2}" +
                    "\nDate of Birth: {3}" +
                    "\nMedical Details" +
                    "\tAllergies: {4}" +
                    "\tImmunizations {5}" +
                    "\tIllnesses {6}+" +
                    "\tDisabilities {7}"
                    ,
                    s.Forename, s.Surname, s.CurrentGradeNumber, s.DateOfBirth.ToString("dd/MM/yyyy"), "WIP",
                    "WIP", "WIP", "WIP", "WIP"
                    ), "ok");
        }

		private void studentList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
            StudentCell currentCell = e.SelectedItem as StudentCell;
            currentCell.textColour = Application.Current.Resources["OnSecondary"] as Color? ?? Color.Black;
            List<StudentCell> temp = students.Except<StudentCell>(new List<StudentCell>() { currentCell }).ToList();
            foreach(var a in temp)
			{
                a.textColour = Application.Current.Resources["OnSurface"] as Color? ?? Color.Black;
            }
        }

    }
*/
            _viewModel = new ViewModels.StudentViewModel();
            BindingContext = _viewModel;
        }

		//private void studentList_ItemTapped(object sender, ItemTappedEventArgs e)
		//{
  //          StudentCell student = e.Item as StudentCell;
  //          Swimclub.Models.Student s = student as Swimclub.Models.Student;
  //          DisplayAlert("Data",
  //              String.Format(
  //                  "Name: {0} {1}" +
  //                  "\nCurrentGrade: {2}" +
  //                  "\nDate of Birth: {3}" +
  //                  "\nMedical Details" +
  //                  "\tAllergies: {4}" +
  //                  "\tImmunizations {5}" +
  //                  "\tIllnesses {6}+" +
  //                  "\tDisabilities {7}"
  //                  ,
  //                  s.Forename, s.Surname, s.CurrentGradeNumber, s.DateOfBirth.ToString("dd/MM/yyyy"), "WIP",
  //                  "WIP", "WIP", "WIP", "WIP"
  //                  ), "ok");
  //      }

		//private void studentList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		//{
  //          StudentCell currentCell = e.SelectedItem as StudentCell;
  //          currentCell.textColour = Application.Current.Resources["OnSecondary"] as Color? ?? Color.Black;
  //          List<StudentCell> temp = students.Except<StudentCell>(new List<StudentCell>() { currentCell }).ToList();
  //          foreach(var a in temp)
		//	{
  //              a.textColour = Application.Current.Resources["OnSurface"] as Color? ?? Color.Black;
  //          }
		//}
	}
}




//Swimclub.Models.Student[] students = await restService.GetAllStudentsAsync();