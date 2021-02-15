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
    public partial class StudentTestingPage : ContentPage
    {
        private Models.Student student;

        private String[] tasks = {"Task_1", "Task_2", "Task_3", "Task_4"};
        private int taskPos = 0;

        public StudentTestingPage(Models.Student lastClickedstudent)
        {
            InitializeComponent();
            student = lastClickedstudent;
            StudentNameLable.Text = student.Forename;
        }

    }
}