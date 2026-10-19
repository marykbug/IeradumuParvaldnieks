using IeradumuParvaldnieksKlases;
using IeradumuParvaldnieksMAUI.ViewModels;
using System.Collections.ObjectModel;

namespace IeradumuParvaldnieksMAUI.Views;

public partial class ArhivetieIeradumi : ContentPage
{
    public ArhivetieIeradumi(
        ObservableCollection<Ieradums> visiIeradumi,
        Func<Task> saglabatDatus)
    {
        InitializeComponent();

        BindingContext = new ArhivetieIeradumiViewModel(
            visiIeradumi,
            saglabatDatus
        );
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ArhivetieIeradumiViewModel viewModel)
        {
            viewModel.AtjaunotSarakstu();
        }
    }
}