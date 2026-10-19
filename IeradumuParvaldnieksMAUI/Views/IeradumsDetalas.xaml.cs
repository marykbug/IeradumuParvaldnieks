using IeradumuParvaldnieksKlases;
using IeradumuParvaldnieksMAUI.ViewModels;
namespace IeradumuParvaldnieksMAUI.Views;

public partial class IeradumsDetalas : ContentPage
{
    public IeradumsDetalas(
    Ieradums ieradums,
    Func<Task> saglabatDatus)
    {
        InitializeComponent();

        BindingContext = new IeradumsDetalasViewModel(
            ieradums,
            saglabatDatus
        );
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is IeradumsDetalasViewModel viewModel)
        {
            viewModel.AtjaunotDatus();
        }
    }
}