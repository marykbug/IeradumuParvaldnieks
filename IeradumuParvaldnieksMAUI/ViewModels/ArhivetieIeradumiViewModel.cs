using IeradumuParvaldnieksKlases;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace IeradumuParvaldnieksMAUI.ViewModels
{
    public partial class ArhivetieIeradumiViewModel : INotifyPropertyChanged
    {
        private readonly ObservableCollection<Ieradums> visiIeradumi;
        private readonly Func<Task> saglabatDatus;

        public ObservableCollection<Ieradums> ArhivetieIeradumi { get; set; }

        public Command<Ieradums> AtjaunotCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ArhivetieIeradumiViewModel(
            ObservableCollection<Ieradums> visiIeradumi,
            Func<Task> saglabatDatus)
        {
            this.visiIeradumi = visiIeradumi;
            this.saglabatDatus = saglabatDatus;

            ArhivetieIeradumi =
                new ObservableCollection<Ieradums>();

            AtjaunotCommand =
                new Command<Ieradums>(Atjaunot);

            AtjaunotSarakstu();
        }

        public void AtjaunotSarakstu()
        {
            ArhivetieIeradumi.Clear();

            foreach (Ieradums ieradums in visiIeradumi)
            {
                if (ieradums.Status == IeradumuStatus.Arhivets)
                {
                    ArhivetieIeradumi.Add(ieradums);
                }
            }

            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(nameof(ArhivetieIeradumi))
            );
        }

        private async void Atjaunot(Ieradums ieradums)
        {
            ieradums.AtjaunotNoArhiva();

            await saglabatDatus();

            AtjaunotSarakstu();
        }
    }
}