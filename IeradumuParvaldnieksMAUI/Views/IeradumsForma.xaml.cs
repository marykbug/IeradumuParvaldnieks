using IeradumuParvaldnieksKlases;
using IeradumuParvaldnieksMAUI.Services;
using IeradumuParvaldnieksMAUI.ViewModels;

namespace IeradumuParvaldnieksMAUI.Views;

public partial class IeradumsForma : ContentPage
{
    // Jauna ieraduma izveide.
    public IeradumsForma(
        APIIeradumuService apiService,
        string lietotajsID)
    {
        InitializeComponent();

        BindingContext = new IeradumsFormaViewModel(
            apiService,
            lietotajsID
        );
    }

    // Esoša ieraduma labošana.
    public IeradumsForma(
        Ieradums ieradums,
        APIIeradumuService apiService)
    {
        InitializeComponent();

        BindingContext = new IeradumsFormaViewModel(
            ieradums,
            apiService
        );
    }
}