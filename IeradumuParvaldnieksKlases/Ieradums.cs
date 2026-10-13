using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
///Bazēts uz " UzdevumuParvaldnieksKlases/Uzdevums"
namespace IeradumuParvaldnieksKlases
{
    public class Ieradums
    {
        private Guid ID;
        private string nosaukums;
        public string Nosaukums { get { return nosaukums; } }
        public Guid Id { get { return ID; } }
        private string apraksts;
        public string Apraksts {
            get {  return apraksts; }
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
        private List<IeradumaIzpilde> izpildes = new List<IeradumaIzpilde>();
        private IeradumuAtkartosana atkartosana;
        public IeradumuAtkartosana Atkartosana
        {
            get { return atkartosana; }
        }
        public DateTimeOffset Izveidots { get; } ///dabūts no ChatGPT
        private DateTimeOffset labots; ///dabūts no ChatGPT
        public DateTimeOffset Labots
        {
            get { return labots; }
        }
        public Ieradums(Guid id, string nos, string lietid, string apr, IeradumuAtkartosana atkart)
        {
            if (lietid == null || lietid.Length == 0)
            {
                throw new ArgumentException("Lietotāja ID nedrīkst būt tukšs.");
            }

            if (nos == null || nos.Length == 0 || nos.Length > 100)
            {
                throw new ArgumentException("Ieraduma nosaukumam jābūt no 1 līdz 100 rakstzīmēm.");
            }

            if (id == Guid.Empty)
            {
                throw new ArgumentException("Ieraduma ID nedrīkst būt tukšs.");
            }

            ID = id;
            nosaukums = nos;
            apraksts = apr;
            lietotajsid = lietid;
            status = IeradumuStatus.Aktivs;
            atkartosana = atkart;

            Izveidots = DateTimeOffset.UtcNow;
            labots = Izveidots;
        }
        public void Arhivet()
        {
            status = IeradumuStatus.Arhivets;
            labots = DateTimeOffset.UtcNow;
        }
        public void Labot(string jaunNos, string jaunApr)
        {
            if (jaunNos == null || jaunNos.Length == 0 || jaunNos.Length > 100)
            {
                throw new ArgumentException("Ieraduma nosaukumam jābūt no 1 līdz 100 rakstzīmēm.");
            }
            if (jaunApr == null || jaunApr.Length == 0)
            {
                throw new ArgumentException("Ieraduma apraksts nedrīkst būt tukšs.");
            }
            nosaukums = jaunNos;
            apraksts = jaunApr;
            labots = DateTimeOffset.UtcNow; 
        }
        public void AtzimetKaIzpilditu(DateOnly datums)
        {
            if (status == IeradumuStatus.Arhivets)
            {
                throw new InvalidOperationException("Arhivētu ieradumu nevar atzīmēt kā izpildītu.");
            }

            foreach (IeradumaIzpilde izpilde in izpildes)
            {
                if (izpilde.Datums == datums)
                {
                    throw new InvalidOperationException("Ieradums šajā datumā jau ir izpildīts.");
                }
            }

            IeradumaIzpilde jaunaIzpilde =
                new IeradumaIzpilde(this, datums);

            izpildes.Add(jaunaIzpilde);
        }
        public bool IrIzpilditsSodien()
        {
            DateOnly sodien = DateOnly.FromDateTime(DateTime.Today);

            foreach (IeradumaIzpilde izpilde in izpildes)
            {
                if (izpilde.Datums == sodien)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
