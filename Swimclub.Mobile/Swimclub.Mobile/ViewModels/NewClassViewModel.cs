using System;
using System.Collections.Generic;
using System.Text;

namespace Swimclub.Mobile.ViewModels
{
    class NewClassViewModel : BaseViewModel
    {
		private Models.Class _class;

		public Models.Class _Class
		{
			get { return _class; }
			set { SetProperty(ref _class, value); }
		}

		
	}
}
