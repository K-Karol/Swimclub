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
    [QueryProperty("StudentID", "identifier")] // pass in student name data from another cs file
    public partial class GradingPages : ContentPage
    {
        private string studentID;
        public string StudentID
		{
			set
			{
                studentID = Uri.UnescapeDataString(value);
			}
			get
			{
                return studentID;
			}
		}

        public GradingPages() //pass in a string of student name / unique identifier when page is created
        {
            InitializeComponent();
            SetStudentName = studentID; //set the name as SetStudentName
        }

        private string SetStudentName;
        public string SetName //display the student name in the lable
        {
            get { return SetStudentName; }
            set
            {
                SetStudentName = value;
                OnPropertyChanged(nameof(SetName));
            }
        }
        /*
        public class Stages
        {
            public string DateDone { get; set; } //display the date of the test
            public int Level { get; set; } // display the level of the test completed
            public string Task { get; set; } //display the tasks in the test
            public bool Score { get; set; } //display pass or fail in the test
        }*/
    }
}

// what i want to do here::
//gradingPages page
// make two pages one to display all past tests and one to add a new test
// this page is to display
// have a button that takes you to the 'add a new record' page
// have a searching function to display the select grades (1-7)
// display all of the sutdents past records for the student database
// diaply student name/uniqueID and their current grade at the page top

//newRecordPage
// a dropdown box to chose or an sytem to detect the level of the student
// a dynamic list to show all of the questions in the chosen grade
// a check box to show a pass or fail for the student
// the student's name displayed at the top of the page
// save the results to the student's database