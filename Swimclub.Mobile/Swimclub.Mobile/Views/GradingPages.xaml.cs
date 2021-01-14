﻿using Swimclub.Mobile.ViewModels;
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
    [QueryProperty("SetStudentName", "identifier")] // pass in student name data from another cs file
    public partial class GradingPages : ContentPage
    {
        public GradingPages() //constructor allways happens first
        {
            InitializeComponent();
            BindingContext = new GradingPagesViewModel();

        }

        private string setStudentName;
        public string SetStudentName //save the student name in setStudentName
        {
			set
			{
                setStudentName = Uri.UnescapeDataString(value);
                var bind = (GradingPagesViewModel)BindingContext;
                if(bind != null)
				{
                    bind.ViewStudentName = setStudentName;
				}
            }
			get
			{
                return setStudentName;
			}
		}

        private void NewTest_Clicked(object sender, EventArgs e)
        {

        }

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

/* attempt two
public class tempDatabase //arrays to store the testText and other arrays containg the data
{
    public string[][] Grade;

    public string[][] Score;
} */

/* Attempt One 
public class Stages
{
    //virtaul as all the diffrent grades will use this as a base, but they all contain diffrent data
    public virtual string DateDone { get; set; } //display the date of the test
    public virtual int Level { get; set; } // display the level of the test completed
    public virtual string Task { get; set; } //display the tasks in the test
    public virtual bool Score { get; set; } //display pass or fail for each task in the test
}*/