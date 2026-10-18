using System;
using System.Collections.Generic;
using System.Text;
using IeradumuParvaldnieksKlases;
using System.ComponentModel;
namespace IeradumuParvaldnieksMAUI.ViewModels
{
    public class IeradumsDetalasViewModel : INotifyPropertyChanged
    {
        public Ieradums Ieradums { get; set; }
        public Command AtzimetKaIzpildituCommand { get; }
        public IeradumsDetalasViewModel(Ieradums ieradums)
        {
            if (ieradums == null)
            {
                throw new ArgumentException("Ieradumam jābūt norādītam.");
            }

            Ieradums = ieradums;
            AtzimetKaIzpildituCommand = new Command(AtzimetKaIzpilditu);
            ArhivetCommand = new Command(Arhivet);
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

            Ieradums.AtzimetKaIzpilditu(
                DateOnly.FromDateTime(DateTime.Today)
            );
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
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(nameof(Ieradums))
            );
        }
    }
}
