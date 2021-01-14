using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Swimclub.Mobile.CustomWidgets
{
	public class StudentListButton : Button
	{
		public static readonly BindableProperty BindStudentNameProperty = BindableProperty.Create(
														 propertyName: "BindStudentName",
														 returnType: typeof(string),
														 declaringType: typeof(StudentListButton),
														 defaultValue: "",
														 defaultBindingMode: BindingMode.TwoWay,
														 propertyChanged: HandleTextPropertyChanged);
		public string BindStudentName {
			get
			{
				return (string)base.GetValue(BindStudentNameProperty);
			}

			set
			{
				if(this.BindStudentName != value)
				{
					base.SetValue(BindStudentNameProperty, value);
				}
			}
		
		}


		private static void HandleTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			
		}

	}
}
