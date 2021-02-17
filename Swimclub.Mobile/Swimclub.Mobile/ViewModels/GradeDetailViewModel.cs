using System;
using System.Collections.Generic;
using System.Text;

namespace Swimclub.Mobile.ViewModels
{
	class GradeDetailViewModel : BaseViewModel
	{
		private Models.Grade grade;

		public Models.Grade Grade
		{
			get { return grade; }
			set { SetProperty(ref grade, value); }
		}

		public int StudentsCount
		{
			get { return grade.Students.Length; }
		}

	}
}
//192.168.1.118