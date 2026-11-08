using IeradumuParvaldnieksMAUI.Services;
using IeradumuParvaldnieksMAUI.ViewModels;

namespace IeradumuParvaldnieksMAUI.Views;

public partial class ArhivetieIeradumi : ContentPage
{
    public ArhivetieIeradumi(
        APIIeradumuService apiService)
    {
        InitializeComponent();

        BindingContext = new ArhivetieIeradumiViewModel(
            apiService
        );
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ArhivetieIeradumiViewModel viewModel)
        {
            await viewModel.IeladetDatusAsync();
        }
    }
}