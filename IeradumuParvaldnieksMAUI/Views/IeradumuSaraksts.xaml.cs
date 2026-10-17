using IeradumuParvaldnieksMAUI.ViewModels;
namespace IeradumuParvaldnieksMAUI.Views;
public partial class IeradumuSaraksts : ContentPage
{
	public IeradumuSaraksts()
	{
		InitializeComponent();
		BindingContext = new IeradumuSarakstsViewModel();
	}
}