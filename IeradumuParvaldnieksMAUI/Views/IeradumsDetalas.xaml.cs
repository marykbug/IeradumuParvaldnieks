using IeradumuParvaldnieksKlases;
using IeradumuParvaldnieksMAUI.Services;
using IeradumuParvaldnieksMAUI.ViewModels;

namespace IeradumuParvaldnieksMAUI.Views;

public partial class IeradumsDetalas : ContentPage
{
    public IeradumsDetalas(
        Ieradums ieradums,
        APIIeradumuService apiService)
    {
        InitializeComponent();

        BindingContext = new IeradumsDetalasViewModel(
            ieradums,
            apiService
        );
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is IeradumsDetalasViewModel viewModel)
        {
            await viewModel.AtjaunotDatusAsync();
        }
    }
}