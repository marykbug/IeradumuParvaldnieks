using System;
using System.Collections.Generic;

/// Bazēts uz "UzdevumuParvaldnieksKlases/Uzdevums"
namespace IeradumuParvaldnieksKlases
{
    public class Ieradums
    {
        private Guid ID;
        public Guid Id
        {
            get { return ID; }
        }
        private string nosaukums;
        public string Nosaukums
        {
            get { return nosaukums; }
        }
        private string apraksts;
        public string Apraksts
        {
            get { return apraksts; }
        }
        private string lietotajsid;
        public string LietotajsID
        {
            get { return lietotajsid; }
        }
        private IeradumuStatus status;
        public IeradumuStatus Status
        {
            get { return status; }
        }
        private List<IeradumaIzpilde> izpildes =
            new List<IeradumaIzpilde>();

        public IReadOnlyList<IeradumaIzpilde> Izpildes
        {
            get { return izpildes.AsReadOnly(); }
        }
        private IeradumuAtkartosana atkartosana;
        public IeradumuAtkartosana Atkartosana
        {
            get { return atkartosana; }
        }
        private List<NedelasDienas> nedelasDienas =
            new List<NedelasDienas>();

        public IReadOnlyList<NedelasDienas> NedelasDienas
        {
            get { return nedelasDienas.AsReadOnly(); }
        }
        public DateTimeOffset Izveidots { get; }

        private DateTimeOffset labots;

        public DateTimeOffset Labots
        {
            get { return labots; }
        }
        // Izmanto ieradumiem, kuriem nav jānorāda nedēļas dienas.
        public Ieradums(
            Guid id,
            string nos,
            string lietid,
            string apr,
            IeradumuAtkartosana atkart)
        {
            if (atkart == IeradumuAtkartosana.Nedela)
            {
                throw new ArgumentException(
                    "Nedēļas ieradumam jānorāda nedēļas dienas."
                );
            }

            ParbauditPamatdatus(id, nos, lietid);

            ID = id;
            nosaukums = nos;
            apraksts = apr;
            lietotajsid = lietid;
            status = IeradumuStatus.Aktivs;
            atkartosana = atkart;

            Izveidots = DateTimeOffset.UtcNow;
            labots = Izveidots;
        }


        // Izmanto nedēļas ieradumam ar izvēlētām nedēļas dienām.
        public Ieradums(
            Guid id,
            string nos,
            string lietid,
            string apr,
            IeradumuAtkartosana atkart,
            IEnumerable<NedelasDienas> dienas)
        {
            ParbauditPamatdatus(id, nos, lietid);

            List<NedelasDienas> izveletasDienas =
                new List<NedelasDienas>(dienas);

            if (atkart == IeradumuAtkartosana.Nedela &&
                izveletasDienas.Count == 0)
            {
                throw new ArgumentException(
                    "Nedēļas ieradumam jāizvēlas vismaz viena nedēļas diena."
                );
            }

            ID = id;
            nosaukums = nos;
            apraksts = apr;
            lietotajsid = lietid;
            status = IeradumuStatus.Aktivs;
            atkartosana = atkart;

            if (atkartosana == IeradumuAtkartosana.Nedela)
            {
                nedelasDienas = izveletasDienas;
            }

            Izveidots = DateTimeOffset.UtcNow;
            labots = Izveidots;
        }
        // Izmanto saglabāta ieraduma atjaunošanai no lokālajiem datiem.
        public Ieradums(
            Guid id,
            string nos,
            string lietid,
            string apr,
            IeradumuAtkartosana atkart,
            IEnumerable<NedelasDienas> dienas,
            IeradumuStatus saglabataisStatus,
            DateTimeOffset izveidots,
            DateTimeOffset labots,
            IEnumerable<DateOnly> izpildesDatumi)
            : this(
                id,
                nos,
                lietid,
                apr,
                atkart,
                dienas)
        {
            foreach (DateOnly datums in izpildesDatumi)
            {
                AtzimetKaIzpilditu(datums);
            }

            status = saglabataisStatus;
            Izveidots = izveidots;
            this.labots = labots;
        }
        private void ParbauditPamatdatus(
            Guid id,
            string nos,
            string lietid)
        {
            if (string.IsNullOrWhiteSpace(lietid))
            {
                throw new ArgumentException(
                    "Lietotāja ID nedrīkst būt tukšs."
                );
            }

            if (string.IsNullOrWhiteSpace(nos) ||
                nos.Length > 100)
            {
                throw new ArgumentException(
                    "Ieraduma nosaukumam jābūt no 1 līdz 100 rakstzīmēm."
                );
            }

            if (id == Guid.Empty)
            {
                throw new ArgumentException(
                    "Ieraduma ID nedrīkst būt tukšs."
                );
            }
        }
        public void Arhivet()
        {
            status = IeradumuStatus.Arhivets;
            labots = DateTimeOffset.UtcNow;
        }
        public void AtjaunotNoArhiva()
        {
            if (status == IeradumuStatus.Aktivs)
            {
                throw new InvalidOperationException(
                    "Ieradums nav arhivēts."
                );
            }

            status = IeradumuStatus.Aktivs;
            labots = DateTimeOffset.UtcNow;
        }
        public void Labot(
            string jaunNos,
            string jaunApr)
        {
            Labot(
                jaunNos,
                jaunApr,
                atkartosana,
                nedelasDienas
            );
        }
        public void Labot(
            string jaunNos,
            string jaunApr,
            IeradumuAtkartosana jaunAtkart)
        {
            List<NedelasDienas> dienas =
                new List<NedelasDienas>();

            if (jaunAtkart == IeradumuAtkartosana.Nedela)
            {
                dienas =
                    new List<NedelasDienas>(nedelasDienas);
            }

            Labot(
                jaunNos,
                jaunApr,
                jaunAtkart,
                dienas
            );
        }
        public void Labot(
            string jaunNos,
            string jaunApr,
            IeradumuAtkartosana jaunAtkart,
            IEnumerable<NedelasDienas> jaunasDienas)
        {
            if (string.IsNullOrWhiteSpace(jaunNos) ||
                jaunNos.Length > 100)
            {
                throw new ArgumentException(
                    "Ieraduma nosaukumam jābūt no 1 līdz 100 rakstzīmēm."
                );
            }

            if (string.IsNullOrWhiteSpace(jaunApr))
            {
                throw new ArgumentException(
                    "Ieraduma apraksts nedrīkst būt tukšs."
                );
            }

            List<NedelasDienas> izveletasDienas =
                new List<NedelasDienas>(jaunasDienas);

            if (jaunAtkart == IeradumuAtkartosana.Nedela &&
                izveletasDienas.Count == 0)
            {
                throw new ArgumentException(
                    "Nedēļas ieradumam jāizvēlas vismaz viena nedēļas diena."
                );
            }

            nosaukums = jaunNos;
            apraksts = jaunApr;
            atkartosana = jaunAtkart;

            if (atkartosana == IeradumuAtkartosana.Nedela)
            {
                nedelasDienas = izveletasDienas;
            }
            else
            {
                nedelasDienas =
                    new List<NedelasDienas>();
            }

            labots = DateTimeOffset.UtcNow;
        }
        public void AtzimetKaIzpilditu(DateOnly datums)
        {
            if (status == IeradumuStatus.Arhivets)
            {
                throw new InvalidOperationException(
                    "Arhivētu ieradumu nevar atzīmēt kā izpildītu."
                );
            }

            foreach (IeradumaIzpilde izpilde in izpildes)
            {
                if (izpilde.Datums == datums)
                {
                    throw new InvalidOperationException(
                        "Ieradums šajā datumā jau ir izpildīts."
                    );
                }
            }
            IeradumaIzpilde jaunaIzpilde =
                new IeradumaIzpilde(this, datums);

            izpildes.Add(jaunaIzpilde);
        }
        public bool IrIzpilditsSodien()
        {
            DateOnly sodien =
                DateOnly.FromDateTime(DateTime.Today);

            foreach (IeradumaIzpilde izpilde in izpildes)
            {
                if (izpilde.Datums == sodien)
                {
                    return true;
                }
            }

            return false;
        }
        public string IzpilditsSodien
        {
            get
            {
                if (IrIzpilditsSodien())
                {
                    return "Jā";
                }

                return "Nē";
            }
        }
    }
}