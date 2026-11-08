using IeradumuParvaldnieksKlases;
using IeradumuParvaldnieksMAUI.Services;
using IeradumuParvaldnieksMAUI.Views;
using IeradumuParvaldnieksTestData;
using System.Collections.ObjectModel;
using System.ComponentModel;
namespace IeradumuParvaldnieksMAUI.ViewModels
{
    public partial class IeradumuSarakstsViewModel : INotifyPropertyChanged
    {
        // Fiksēts testa lietotājs, kuram tiks pievienoti jaunizveidotie ieradumi.
        public string PasreizejaisLietotajsID { get; } = "demo-user-1";
        // Visi ieradumi, kas tiek saglabāti lokālajā failā.
        public ObservableCollection<Ieradums> Ieradums { get; set; }
        // Galvenajā skatā tiek rādīti tikai aktīvie ieradumi.
        public ObservableCollection<Ieradums> AktivieIeradumi { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        private readonly IIeradumuStorageService storageService;
        private bool datiIeladeti = false;
        public Command JaunsIeradumsCommand { get; }
        public Command ArhivetieIeradumiCommand { get; }
        public IeradumuSarakstsViewModel()
        {
            storageService = new JSONIeradumuStorageService();

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
                    Ieradums,
                    PasreizejaisLietotajsID,
                    SaglabatDatusAsync
                )
            );
        }
        private async void AtvertArhivetosIeradumus()
        {
            await Shell.Current.Navigation.PushAsync(
                new ArhivetieIeradumi(
                    Ieradums,
                    SaglabatDatusAsync
                )
            );
        }
        public async Task IeladetDatusAsync()
        {
            // Dati tiek ielādēti tikai vienu reizi.
            if (datiIeladeti)
            {
                return;
            }
            List<Ieradums> saglabatieIeradumi;
            try
            {
                saglabatieIeradumi =
                    await storageService.IeladetAsync();
            }
            catch (ArgumentException)
            {
                // Ja vecie saglabātie dati neatbilst jaunajai
                // ieraduma struktūrai, izmanto testa datus.
                saglabatieIeradumi =
                    IeradumsTestData.IzveidotIeradumus();

                // Vecais lokālais fails tiek aizstāts ar jauniem datiem.
                await storageService.SaglabatAsync(
                    saglabatieIeradumi
                );
            }
            // Pirmajā palaišanas reizē izmanto testa datus.
            if (saglabatieIeradumi.Count == 0)
            {
                saglabatieIeradumi =
                    IeradumsTestData.IzveidotIeradumus();

                await storageService.SaglabatAsync(
                    saglabatieIeradumi
                );
            }
            Ieradums =
                new ObservableCollection<Ieradums>(
                    saglabatieIeradumi
                );
            datiIeladeti = true;
        }
        public async Task SaglabatDatusAsync()
        {
            await storageService.SaglabatAsync(Ieradums);
        }
    }
}