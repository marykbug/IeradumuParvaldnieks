using IeradumuParvaldnieksKlases;
using IeradumuParvaldnieksMAUI.Views;
using System.Collections.ObjectModel;

namespace IeradumuParvaldnieksMAUI.ViewModels
{
    public class IeradumsFormaViewModel
    {
        private Ieradums? ieradums;
        private ObservableCollection<Ieradums>? ieradumi;
        private string? lietotajsID;

        public string Nosaukums { get; set; }
        public string Apraksts { get; set; }
        public IeradumuAtkartosana Atkartosana { get; set; }

        private readonly Func<Task> saglabatDatus;

        public Command SaglabatCommand { get; }

        // Jauna ieraduma izveide
        public IeradumsFormaViewModel(
            ObservableCollection<Ieradums> ieradumi,
            string lietotajsID,
            Func<Task> saglabatDatus)
        {
            this.ieradumi = ieradumi;
            this.lietotajsID = lietotajsID;
            this.saglabatDatus = saglabatDatus;

            Nosaukums = "";
            Apraksts = "";
            Atkartosana = IeradumuAtkartosana.Diena;

            SaglabatCommand = new Command(Saglabat);
        }

        // Esoša ieraduma labošana
        public IeradumsFormaViewModel(
            Ieradums ieradums,
            Func<Task> saglabatDatus)
        {
            this.ieradums = ieradums;
            this.saglabatDatus = saglabatDatus;

            lietotajsID = ieradums.LietotajsID;

            Nosaukums = ieradums.Nosaukums;
            Apraksts = ieradums.Apraksts;
            Atkartosana = ieradums.Atkartosana;

            SaglabatCommand = new Command(Saglabat);
        }

        public IeradumuAtkartosana[] AtkartosanasVeidi { get; } =
            Enum.GetValues<IeradumuAtkartosana>();

        private async void Saglabat()
        {
            try
            {
                if (ieradums != null)
                {
                    ieradums.Labot(
                        Nosaukums,
                        Apraksts,
                        Atkartosana
                    );
                }
                else
                {
                    if (ieradumi == null ||
                        string.IsNullOrWhiteSpace(lietotajsID))
                    {
                        return;
                    }

                    Ieradums jaunsIeradums = new Ieradums(
                        Guid.NewGuid(),
                        Nosaukums,
                        lietotajsID,
                        Apraksts,
                        Atkartosana
                    );

                    ieradumi.Add(jaunsIeradums);
                }

                await saglabatDatus();

                await Shell.Current.Navigation.PopAsync();
            }
            catch (ArgumentException ex)
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