using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Swimclub.Mobile.ViewModels
{
	class GradeViewModel : BaseViewModel
	{
		private readonly Services.IRestService restService;
		private Swimclub.Models.Grade[] grades = new Swimclub.Models.Grade[] { };


		private ObservableCollection<GradeCell> gradesCollection = new ObservableCollection<GradeCell>();
		public ObservableCollection<GradeCell> GradesCollection { get { return gradesCollection; } }

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

		private bool refreshing = false;
		public bool isRefreshing
		{
			get { return refreshing; }
			set { SetProperty(ref refreshing, value); }
		}

		public GradeViewModel()
		{
			restService = DependencyService.Get<Services.IRestService>();
			LoadAll = new Command(loadData);
			SelectGrade = new Command<object>(selectGrade);
			Task.Run(() => loadDataAsync());
		}

		private async Task loadDataAsync()
		{
			isRefreshing = true;
			loadData();
		}

		private void loadData()
		{
			isRefreshing = true;
			Services.AllGradesReturn ret;
			Task<Services.AllGradesReturn> task = Task.Run(() => restService.GetAllGradesAsync());
			task.ContinueWith(t => Device.BeginInvokeOnMainThread(
				async () =>
				{
					ret = t.Result;
					if (!ret.Success)
					{
						isRefreshing = false;
						//await DisplayAlert("Error", "There was an error fetching the data", "oopsieDasiy");
						return;
					}
					grades = ret.Grades;
					

					gradesCollection.Clear();



					foreach (var g in grades)
					{
						GradeCell newG = new GradeCell(g);
						newG.textColour = Application.Current.Resources["OnSurface"] as Color? ?? Color.Black;
						gradesCollection.Add(newG);
					}
					isRefreshing = false;
				}
			  ));
		}

		private void selectGrade(object obj)
		{
			
			GradeCell g = (GradeCell)obj;

			g.textColour = Application.Current.Resources["OnSecondary"] as Color? ?? Color.Black;
			List<GradeCell> temp = gradesCollection.Except<GradeCell>(new List<GradeCell>() { g }).ToList();
			foreach (var a in temp)
			{
				a.textColour = Application.Current.Resources["OnSurface"] as Color? ?? Color.Black;
			}

			Shell.Current.Navigation.PushAsync(new Views.GradeDetailPage(GradeCell.ConvertToGrade(g)));
		}

	}


	public class GradeCell : Models.Grade, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public string StudentsLength
		{
			get { return $"{this.Students.Length}"; }
		}

		private Color textcol;
		public Color textColour
		{
			get { return textcol; }
			set
			{
				if (textcol != value)
				{
					textcol = value;
					if (PropertyChanged != null)
					{
						PropertyChanged(this, new PropertyChangedEventArgs("textColour"));
					}
				}
			}
		}

		public GradeCell(Swimclub.Models.Grade _g)
		{
			this.ID = _g.ID;
			this.Number = _g.Number;
			this.Students = _g.Students;
			this.Tests = _g.Tests;
		}

		public static Models.Grade ConvertToGrade(GradeCell cell)
		{
			return new Models.Grade()
			{
				ID = cell.ID,
				Number = cell.Number,
				Students = cell.Students,
				Tests = cell.Tests,
			};
		}

	}

}
