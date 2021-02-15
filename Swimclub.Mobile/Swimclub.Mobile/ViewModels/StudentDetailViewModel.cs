using System;
using System.Collections.Generic;
using System.Text;

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
}
