using System;
using System.Collections.Generic;
using System.Text;

namespace Swimclub.Mobile.ViewModels
{
	public class AppShellViewModel : BaseViewModel
	{
		private bool admin = false;
		public bool isAdmin
		{
			get { return admin; }
			set { SetProperty(ref admin, value); }
		}
	}
}
