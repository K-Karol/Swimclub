using System;
using System.Collections.Generic;
using System.Text;

namespace Swimclub.Mobile.ViewModels
{
	class SwimClassDetailViewModel : BaseViewModel
	{
		private readonly Services.IRestService restService;

		private ViewModels.ClassesTemp _class;
		public ViewModels.ClassesTemp _Class
		{
			get { return _class; }
			set { SetProperty(ref _class, value); }
		}
	}
}
