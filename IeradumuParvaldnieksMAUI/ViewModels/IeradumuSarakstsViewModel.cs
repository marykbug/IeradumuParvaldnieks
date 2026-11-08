using IeradumuParvaldnieksKlases;
using IeradumuParvaldnieksMAUI.Services;
using IeradumuParvaldnieksMAUI.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace IeradumuParvaldnieksMAUI.ViewModels
{
    public partial class IeradumuSarakstsViewModel : INotifyPropertyChanged
    {
        // Fiksēts testa lietotājs, kuram tiks pievienoti jaunizveidotie ieradumi.
        public string PasreizejaisLietotajsID { get; } = "demo-user-1";

        // Visi ieradumi, kas saņemti no API.
        public ObservableCollection<Ieradums> Ieradums { get; set; }

        // Galvenajā skatā tiek rādīti tikai aktīvie ieradumi.
        public ObservableCollection<Ieradums> AktivieIeradumi { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        // Serviss saziņai ar API.
        public APIIeradumuService ApiService { get; }

        public Command JaunsIeradumsCommand { get; }
        public Command ArhivetieIeradumiCommand { get; }

        public IeradumuSarakstsViewModel()
        {
            ApiService = new APIIeradumuService();

            Ieradums = new ObservableCollection<Ieradums>();
            AktivieIeradumi = new ObservableCollection<Ieradums>();

            JaunsIeradumsCommand = new Command(JaunsIeradums);
            ArhivetieIeradumiCommand =
                new Command(AtvertArhivetosIeradumus);
        }

        public void AtjaunotSarakstu()
        {
            AktivieIeradumi.Clear();

            foreach (Ieradums ieradums in Ieradums)
            {
                if (ieradums.Status == IeradumuStatus.Aktivs)
                {
                    AktivieIeradumi.Add(ieradums);
                }
            }

            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(nameof(AktivieIeradumi))
            );
        }

        private async void JaunsIeradums()
        {
            await Shell.Current.Navigation.PushAsync(
                new IeradumsForma(
                    ApiService,
                    PasreizejaisLietotajsID
                )
            );
        }

        private async void AtvertArhivetosIeradumus()
        {
            await Shell.Current.Navigation.PushAsync(
                new ArhivetieIeradumi(
                    ApiService
                )
            );
        }

        // Ielādē aktuālos ieradumus no API.
        public async Task IeladetDatusAsync()
        {
            try
            {
                List<Ieradums> apiIeradumi =
                    await ApiService.IegutVisusAsync();

                Ieradums =
                    new ObservableCollection<Ieradums>(
                        apiIeradumi
                    );

                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs(nameof(Ieradums))
                );

                AtjaunotSarakstu();
            }
            catch (HttpRequestException)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Kļūda",
                    "Neizdevās izveidot savienojumu ar API.",
                    "Labi"
                );
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync(
                    "Kļūda",
                    ex.Message,
                    "Labi"
                );
            }
        }
    }
}