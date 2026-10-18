using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using IeradumuParvaldnieksKlases;
using IeradumuParvaldnieksTestData;
using System.ComponentModel;

namespace IeradumuParvaldnieksMAUI.ViewModels
{
    public partial class IeradumuSarakstsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Ieradums> Ieradums { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        public IeradumuSarakstsViewModel()
        {
            Ieradums = new ObservableCollection<Ieradums>(IeradumsTestData.IzveidotIeradumus());
        }
        public void AtjaunotSarakstu()
        {
            Ieradums = new ObservableCollection<Ieradums>(Ieradums);

            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(nameof(Ieradums))
            );
        }
    }
}
