using IeradumuParvaldnieksKlases;
using IeradumuParvaldnieksMAUI.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace IeradumuParvaldnieksMAUI.ViewModels
{
    public partial class IeradumsFormaViewModel : INotifyPropertyChanged
    {
        private Ieradums? ieradums;
        private ObservableCollection<Ieradums>? ieradumi;
        private string? lietotajsID;
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

        // Norāda, vai formā jārāda nedēļas dienu izvēle.
        public bool RaditNedelasDienas
        {
            get
            {
                return Atkartosana == IeradumuAtkartosana.Nedela;
            }
        }

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

            // Atzīmē iepriekš saglabātās nedēļas dienas.
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

        public IeradumuAtkartosana[] AtkartosanasVeidi { get; } =
            Enum.GetValues<IeradumuAtkartosana>();

        // Izveido sarakstu ar lietotāja izvēlētajām nedēļas dienām.
        private List<NedelasDienas> IegutIzveletasDienas()
        {
            List<NedelasDienas> dienas =
                new List<NedelasDienas>();
            // Nedēļas dienas nepieciešamas tikai nedēļas atkārtojumam.
            if (Atkartosana != IeradumuAtkartosana.Nedela)
            {
                return dienas;
            }

            if (Pirmdiena)
            {
                dienas.Add(NedelasDienas.Pirmdiena);
            }

            if (Otrdiena)
            {
                dienas.Add(NedelasDienas.Otrdiena);
            }

            if (Tresdiena)
            {
                dienas.Add(NedelasDienas.Tresdiena);
            }

            if (Ceturtdiena)
            {
                dienas.Add(NedelasDienas.Ceturtdiena);
            }

            if (Piektdiena)
            {
                dienas.Add(NedelasDienas.Piektdiena);
            }

            if (Sestdiena)
            {
                dienas.Add(NedelasDienas.Sestdiena);
            }

            if (Svetdiena)
            {
                dienas.Add(NedelasDienas.Svetdiena);
            }

            return dienas;
        }
        private async void Saglabat()
        {
            try
            {
                List<NedelasDienas> izveletasDienas =
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
                if (ieradums != null)
                {
                    ieradums.Labot(
                        Nosaukums,
                        Apraksts,
                        Atkartosana,
                        izveletasDienas
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
                        Atkartosana,
                        izveletasDienas
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