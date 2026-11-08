using IeradumuParvaldnieksDataModels;
using IeradumuParvaldnieksKlases;
using IeradumuParvaldnieksMAUI.Services;
using System.ComponentModel;

namespace IeradumuParvaldnieksMAUI.ViewModels
{
    public partial class IeradumsFormaViewModel : INotifyPropertyChanged
    {
        private readonly APIIeradumuService apiService;
        private readonly Ieradums? ieradums;
        private readonly string lietotajsID;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string Nosaukums { get; set; }
        public string Apraksts { get; set; }

        public bool Pirmdiena { get; set; }
        public bool Otrdiena { get; set; }
        public bool Tresdiena { get; set; }
        public bool Ceturtdiena { get; set; }
        public bool Piektdiena { get; set; }
        public bool Sestdiena { get; set; }
        public bool Svetdiena { get; set; }

        private IeradumuAtkartosana atkartosana;

        public IeradumuAtkartosana Atkartosana
        {
            get
            {
                return atkartosana;
            }

            set
            {
                atkartosana = value;

                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs(nameof(Atkartosana))
                );

                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs(nameof(RaditNedelasDienas))
                );
            }
        }

        public bool RaditNedelasDienas
        {
            get
            {
                return Atkartosana == IeradumuAtkartosana.Nedela;
            }
        }

        public IeradumuAtkartosana[] AtkartosanasVeidi { get; } =
            Enum.GetValues<IeradumuAtkartosana>();

        public Command SaglabatCommand { get; }


        // Jauna ieraduma izveide.
        public IeradumsFormaViewModel(
            APIIeradumuService apiService,
            string lietotajsID)
        {
            this.apiService = apiService;
            this.lietotajsID = lietotajsID;

            Nosaukums = "";
            Apraksts = "";
            Atkartosana = IeradumuAtkartosana.Diena;

            SaglabatCommand = new Command(Saglabat);
        }


        // Esoša ieraduma labošana.
        public IeradumsFormaViewModel(
            Ieradums ieradums,
            APIIeradumuService apiService)
        {
            this.ieradums = ieradums;
            this.apiService = apiService;
            lietotajsID = ieradums.LietotajsID;

            Nosaukums = ieradums.Nosaukums;
            Apraksts = ieradums.Apraksts;
            Atkartosana = ieradums.Atkartosana;

            foreach (NedelasDienas diena in ieradums.NedelasDienas)
            {
                switch (diena)
                {
                    case NedelasDienas.Pirmdiena:
                        Pirmdiena = true;
                        break;

                    case NedelasDienas.Otrdiena:
                        Otrdiena = true;
                        break;

                    case NedelasDienas.Tresdiena:
                        Tresdiena = true;
                        break;

                    case NedelasDienas.Ceturtdiena:
                        Ceturtdiena = true;
                        break;

                    case NedelasDienas.Piektdiena:
                        Piektdiena = true;
                        break;

                    case NedelasDienas.Sestdiena:
                        Sestdiena = true;
                        break;

                    case NedelasDienas.Svetdiena:
                        Svetdiena = true;
                        break;
                }
            }

            SaglabatCommand = new Command(Saglabat);
        }


        private List<string> IegutIzveletasDienas()
        {
            List<string> dienas = new List<string>();

            if (Atkartosana != IeradumuAtkartosana.Nedela)
            {
                return dienas;
            }

            if (Pirmdiena)
                dienas.Add(NedelasDienas.Pirmdiena.ToString());

            if (Otrdiena)
                dienas.Add(NedelasDienas.Otrdiena.ToString());

            if (Tresdiena)
                dienas.Add(NedelasDienas.Tresdiena.ToString());

            if (Ceturtdiena)
                dienas.Add(NedelasDienas.Ceturtdiena.ToString());

            if (Piektdiena)
                dienas.Add(NedelasDienas.Piektdiena.ToString());

            if (Sestdiena)
                dienas.Add(NedelasDienas.Sestdiena.ToString());

            if (Svetdiena)
                dienas.Add(NedelasDienas.Svetdiena.ToString());

            return dienas;
        }


        private async void Saglabat()
        {
            try
            {
                List<string> izveletasDienas =
                    IegutIzveletasDienas();

                if (Atkartosana == IeradumuAtkartosana.Nedela &&
                    izveletasDienas.Count == 0)
                {
                    await Shell.Current.DisplayAlertAsync(
                        "Kļūda",
                        "Jāizvēlas vismaz viena nedēļas diena.",
                        "Labi"
                    );

                    return;
                }

                if (ieradums == null)
                {
                    IzveidotIeradumsDataModel modelis =
                        new IzveidotIeradumsDataModel
                        {
                            LietotajsID = lietotajsID,
                            Nosaukums = Nosaukums,
                            Apraksts = Apraksts,
                            Atkartosana = Atkartosana.ToString(),
                            NedelasDienas = izveletasDienas
                        };

                    await apiService.IzveidotAsync(modelis);
                }
                else
                {
                    LabotIeradumuDataModel modelis =
                        new LabotIeradumuDataModel
                        {
                            IeradumsID = ieradums.Id,
                            Nosaukums = Nosaukums,
                            Apraksts = Apraksts,
                            Atkartosana = Atkartosana.ToString(),
                            NedelasDienas = izveletasDienas
                        };

                    await apiService.LabotAsync(
                        ieradums.Id,
                        modelis
                    );
                }

                await Shell.Current.Navigation.PopAsync();
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