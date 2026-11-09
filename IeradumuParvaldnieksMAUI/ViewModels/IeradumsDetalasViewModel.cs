using IeradumuParvaldnieksDataModels;
using IeradumuParvaldnieksKlases;
using IeradumuParvaldnieksMAUI.Services;
using IeradumuParvaldnieksMAUI.Views;
using System.ComponentModel;

namespace IeradumuParvaldnieksMAUI.ViewModels
{
    public partial class IeradumsDetalasViewModel : INotifyPropertyChanged
    {
        private readonly APIIeradumuService apiService;

        public Ieradums Ieradums { get; set; }

        public Command AtzimetKaIzpildituCommand { get; }
        public Command ArhivetCommand { get; }
        public Command LabotCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public IeradumsDetalasViewModel(
            Ieradums ieradums,
            APIIeradumuService apiService)
        {
            Ieradums = ieradums;
            this.apiService = apiService;

            AtzimetKaIzpildituCommand =
                new Command(AtzimetKaIzpilditu);

            ArhivetCommand =
                new Command(Arhivet);

            LabotCommand =
                new Command(Labot);
        }

        public string IzpilditsSodien
        {
            get { return Ieradums.IzpilditsSodien; }
        }

        private async void AtzimetKaIzpilditu()
        {
            try
            {
                AtzimetIzpildiDataModel modelis =
                    new AtzimetIzpildiDataModel
                    {
                        IeradumsID = Ieradums.Id,
                        Datums = DateOnly.FromDateTime(
                            DateTime.Today
                        )
                    };

                await apiService.AtzimetKaIzpildituAsync(
                    Ieradums.Id,
                    modelis
                );

                await AtjaunotDatusAsync();
            }
            catch (HttpRequestException)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Kļūda",
                    "Neizdevās izveidot savienojumu ar API.",
                    "Labi"
                );
            }
            catch (InvalidOperationException ex)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Kļūda",
                    ex.Message,
                    "Labi"
                );
            }
        }

        private async void Arhivet()
        {
            try
            {
                await apiService.ArhivetAsync(
                    Ieradums.Id
                );

                await AtjaunotDatusAsync();
            }
            catch (HttpRequestException)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Kļūda",
                    "Neizdevās izveidot savienojumu ar API.",
                    "Labi"
                );
            }
            catch (InvalidOperationException ex)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Kļūda",
                    ex.Message,
                    "Labi"
                );
            }
        }

        private async void Labot()
        {
            await Shell.Current.Navigation.PushAsync(
                new IeradumsForma(
                    Ieradums,
                    apiService
                )
            );
        }

        public async Task AtjaunotDatusAsync()
        {
            try
            {
                Ieradums? atjaunotaisIeradums =
                    await apiService.IegutPecIdAsync(
                        Ieradums.Id
                    );

                if (atjaunotaisIeradums == null)
                {
                    await Shell.Current.DisplayAlertAsync(
                        "Kļūda",
                        "Ieradums nav atrasts.",
                        "Labi"
                    );

                    return;
                }

                Ieradums = atjaunotaisIeradums;

                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs(nameof(Ieradums))
                );

                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs(nameof(IzpilditsSodien))
                );
            }
            catch (HttpRequestException)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Kļūda",
                    "Neizdevās izveidot savienojumu ar API.",
                    "Labi"
                );
            }
        }
    }
}