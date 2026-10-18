using IeradumuParvaldnieksKlases;
using IeradumuParvaldnieksMAUI.ViewModels;
namespace IeradumuParvaldnieksMAUI.Views;

public partial class IeradumsDetalas : ContentPage
{
	public IeradumsDetalas(Ieradums ieradums)
	{
		InitializeComponent();
        BindingContext = new IeradumsDetalasViewModel(ieradums);
    }
}