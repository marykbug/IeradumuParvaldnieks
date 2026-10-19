using IeradumuParvaldnieksKlases;
using IeradumuParvaldnieksMAUI.Views;
using System.ComponentModel;
namespace IeradumuParvaldnieksMAUI.ViewModels
{
    public partial class IeradumsDetalasViewModel : INotifyPropertyChanged
    {
        public Ieradums Ieradums { get; set; }
        private readonly Func<Task> saglabatDatus;
        public Command AtzimetKaIzpildituCommand { get; }
        public Command LabotCommand { get; }
        public IeradumsDetalasViewModel(
            Ieradums ieradums,
            Func<Task> saglabatDatus)
        {
            Ieradums = ieradums;
            this.saglabatDatus = saglabatDatus;

            AtzimetKaIzpildituCommand = new Command(AtzimetKaIzpilditu);
            ArhivetCommand = new Command(Arhivet);
            LabotCommand = new Command(Labot);
        }
        public string IzpilditsSodien
        {
            get { return Ieradums.IzpilditsSodien; }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        private async void AtzimetKaIzpilditu()
        {
            if (Ieradums.IrIzpilditsSodien())
            {
                await Shell.Current.DisplayAlertAsync(
                    "Paziņojums",
                    "Ieradums šodien jau ir izpildīts.",
                    "Labi"
                );
                return;
            }
            Ieradums.AtzimetKaIzpilditu(DateOnly.FromDateTime(DateTime.Today));
            // Saglabā izmaiņas lokālajā failā.
            await saglabatDatus();

            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(nameof(IzpilditsSodien))
            );
        }
        public Command ArhivetCommand { get; }
        private async void Arhivet()
        {
            if (Ieradums.Status == IeradumuStatus.Arhivets)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Paziņojums",
                    "Ieradums jau ir arhivēts.",
                    "Labi"
                );
                return;
            }
            Ieradums.Arhivet();
            // Saglabā izmaiņas lokālajā failā.
            await saglabatDatus();
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(nameof(Ieradums))
            );
        }
        private async void Labot()
        {
            await Shell.Current.Navigation.PushAsync(
                new IeradumsForma(
                    Ieradums,
                    saglabatDatus
                )
            );
        }
        public void AtjaunotDatus()
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(nameof(Ieradums))
            );

            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(nameof(IzpilditsSodien))
            );
        }
    }
}
