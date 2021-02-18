using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Swimclub.Mobile.ViewModels
{
	class GradeDetailViewModel : BaseViewModel
	{


		private Swimclub.Models.Student[] Students = new Swimclub.Models.Student[] { };
		ObservableCollection<StudentTemp> students = new ObservableCollection<StudentTemp>();

		public ObservableCollection<StudentTemp> StudentsCollection { get { return students; } }





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

		public Models.Student[] StudentList { get { return grade.Students; } }

	}
}
//192.168.1.118