using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Swimclub.Mobile.ViewModels
{
    public class StudentDetailViewModel : BaseViewModel
    {
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
            if (data == null) { return ""; }
            if (data.Length == 0) { return ""; }
            foreach (var s in data)
            {
                temp += $"{s},";
            }
            temp = temp.Remove(temp.Length - 1, 1);
            return temp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string data = (string)value;
            if (data == "") { return null; }
            string[] temp = data.Split(',');
            return temp;
        }
    }
}
