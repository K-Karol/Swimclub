using Swimclub.Mobile.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Swimclub.Mobile.Views
{
	public partial class ItemDetailPage : ContentPage
	{
		public ItemDetailPage()
		{
			InitializeComponent();
			BindingContext = new ItemDetailViewModel();
		}
	}
}