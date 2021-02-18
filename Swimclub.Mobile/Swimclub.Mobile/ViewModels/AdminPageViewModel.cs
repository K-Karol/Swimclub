using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Swimclub.Mobile.ViewModels
{
	public class AdminPageViewModel : BaseViewModel
	{
		public Command ModifyStudentDataPage { get; }
		public Command RegisterUserPage { get; }
		public Command DeleteUserPage { get; }

		public AdminPageViewModel()
		{
			ModifyStudentDataPage = new Command(Modify_Student);
			RegisterUserPage = new Command(Register_User);
			DeleteUserPage = new Command(Delete_User);
		}

		private void Modify_Student()
		{
			Shell.Current.Navigation.PushAsync(new Views.StudentData());
		}

		private void Register_User()
		{
			Shell.Current.Navigation.PushAsync(new Views.RegisterUserPage());
		}

		private void Delete_User()
		{
			Shell.Current.Navigation.PushAsync(new Views.DeleteUserPage());
		}

	}
}
