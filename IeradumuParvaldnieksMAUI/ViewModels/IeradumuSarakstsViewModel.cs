using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using IeradumuParvaldnieksKlases;
using IeradumuParvaldnieksTestData;

namespace IeradumuParvaldnieksMAUI.ViewModels
{
    public class IeradumuSarakstsViewModel
    {
        public ObservableCollection<Ieradums> Ieradums { get; set; }
        public IeradumuSarakstsViewModel()
        {
            Ieradums = new ObservableCollection<Ieradums>(IeradumsTestData.IzveidotIeradumus());
        }
    }
}
