using IeradumuParvaldnieksKlases;
using IeradumuParvaldnieksMAUI.ViewModels;
using System.Linq;
namespace IeradumuParvaldnieksMAUI.Views;
public partial class IeradumuSaraksts : ContentPage
{
	public IeradumuSaraksts()
	{
		InitializeComponent();
		BindingContext = new IeradumuSarakstsViewModel();

    }
    private async void IeradumsSelectionChanged(
        object sender,
        SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count > 0 &&
            e.CurrentSelection[0] is Ieradums ieradums)
        {
            await Navigation.PushAsync(
                new IeradumsDetalas(ieradums));

            ((CollectionView)sender).SelectedItem = null;
        }
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is IeradumuSarakstsViewModel viewModel)
        {
            viewModel.AtjaunotSarakstu();
        }
    }
}