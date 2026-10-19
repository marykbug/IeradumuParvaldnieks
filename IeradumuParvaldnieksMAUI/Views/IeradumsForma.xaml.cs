using IeradumuParvaldnieksKlases;
using IeradumuParvaldnieksMAUI.ViewModels;
using System.Collections.ObjectModel;
namespace IeradumuParvaldnieksMAUI.Views;

public partial class IeradumsForma : ContentPage
{
    public IeradumsForma(
    ObservableCollection<Ieradums> ieradumi,
    string lietotajsID,
    Func<Task> saglabatDatus)
    {
        InitializeComponent();

        BindingContext = new IeradumsFormaViewModel(
            ieradumi,
            lietotajsID,
            saglabatDatus
        );
    }
    public IeradumsForma(
    Ieradums ieradums,
    Func<Task> saglabatDatus)
    {
        InitializeComponent();

        BindingContext = new IeradumsFormaViewModel(
            ieradums,
            saglabatDatus
        );
    }
}