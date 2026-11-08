using IeradumuParvaldnieksKlases;
using IeradumuParvaldnieksMAUI.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace IeradumuParvaldnieksMAUI.ViewModels
{
    public partial class ArhivetieIeradumiViewModel : INotifyPropertyChanged
    {
        private readonly APIIeradumuService apiService;

        public ObservableCollection<Ieradums> ArhivetieIeradumi { get; set; }

        public Command<Ieradums> AtjaunotCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ArhivetieIeradumiViewModel(
            APIIeradumuService apiService)
        {
            this.apiService = apiService;

            ArhivetieIeradumi =
                new ObservableCollection<Ieradums>();

            AtjaunotCommand =
                new Command<Ieradums>(Atjaunot);
        }

        // Ielādē arhivētos ieradumus no API.
        public async Task IeladetDatusAsync()
        {
            try
            {
                List<Ieradums> visiIeradumi =
                    await apiService.IegutVisusAsync();

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

        private async void Atjaunot(Ieradums ieradums)
        {
            try
            {
                await apiService.AtjaunotNoArhivaAsync(
                    ieradums.Id
                );

                await IeladetDatusAsync();
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
    }
}