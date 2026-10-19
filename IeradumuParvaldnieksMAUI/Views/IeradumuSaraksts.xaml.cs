using IeradumuParvaldnieksKlases;
using IeradumuParvaldnieksMAUI.ViewModels;
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
            e.CurrentSelection[0] is Ieradums ieradums &&
            BindingContext is IeradumuSarakstsViewModel viewModel)
        {
            await Navigation.PushAsync(
                new IeradumsDetalas(
                    ieradums,
                    viewModel.SaglabatDatusAsync
                )
            );

            ((CollectionView)sender).SelectedItem = null;
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is IeradumuSarakstsViewModel viewModel)
        {
            await viewModel.IeladetDatusAsync();
            viewModel.AtjaunotSarakstu();
        }
    }
}