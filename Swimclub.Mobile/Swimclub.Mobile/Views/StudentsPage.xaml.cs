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