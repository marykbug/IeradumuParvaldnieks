using IeradumuParvaldnieksKlases;

namespace IeradumuParvaldnieksMAUI.Models
{
    public class IeradumsLocalModel
    {
        public Guid Id { get; set; }
        public string LietotajsID { get; set; } = "";
        public string Nosaukums { get; set; } = "";
        public string Apraksts { get; set; } = "";
        public IeradumuStatus Status { get; set; }
        public IeradumuAtkartosana Atkartosana { get; set; }
        public DateTimeOffset Izveidots { get; set; }
        public DateTimeOffset Labots { get; set; }
        public List<DateOnly> IzpildesDatumi { get; set; } = new();
    }
}
