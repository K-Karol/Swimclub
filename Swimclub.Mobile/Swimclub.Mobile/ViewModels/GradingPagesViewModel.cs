using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace Swimclub.Mobile.ViewModels
{
    public class GradingPagesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string viewStudentName;

        public string ViewStudentName //save the student name in setStudentName
        {
            set
            {
                if (viewStudentName != value)
                {
                    viewStudentName = value;
                    OnPropertyChanged(nameof(ViewStudentName));
                }
            }
            get
            {
                return viewStudentName;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
