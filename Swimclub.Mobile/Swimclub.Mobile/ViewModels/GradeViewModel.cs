using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace Swimclub.Mobile.ViewModels
{
	class GradeViewModel : BaseViewModel
	{
		private Swimclub.Models.Grade[] grades = new Swimclub.Models.Grade[] { };


		private ObservableCollection<Swimclub.Models.Grade> gradesCollection = new ObservableCollection<Swimclub.Models.Grade>();
		public ObservableCollection<Swimclub.Models.Grade> GradesCollection { get { return gradesCollection; } }

		public Command LoadAll { get; }
		public Command<object> SelectGrade { get; }

		private string testData = "Hello";

		public string TestData
		{
			get
			{
				return testData;
			}
			set
			{
				SetProperty(ref testData, value);
			}
		}

		private bool busy = false;
		public bool isBusy
		{
			get { return busy; }
			set { SetProperty(ref busy, value); }
		}

		public GradeViewModel()
		{
			LoadAll = new Command(loadData);
			SelectGrade = new Command<object>(selectGrade);
		}


		private void loadData()
		{
			//some 'loading' here
			isBusy = true;
			gradesCollection.Clear();
			gradesCollection.Add(new Models.Grade() { ID = 1 });
			gradesCollection.Add(new Models.Grade() { ID = 2 });
			gradesCollection.Add(new Models.Grade() { ID = 3 });
			gradesCollection.Add(new Models.Grade() { ID = 4 });
			isBusy = false;
		}

		private void selectGrade(object obj)
		{
			Models.Grade g = (Models.Grade)obj;
			Shell.Current.Navigation.PushAsync(new Views.GradeDetailPage(g));
		}

	}

}
