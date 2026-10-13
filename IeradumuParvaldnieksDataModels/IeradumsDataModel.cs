namespace IeradumuParvaldnieksDataModels
{
    public class IeradumsDataModel
    {
        public Guid Id { get; set; }
        public string LietotajsID { get; set; }
        public string Nosaukums { get; set; }
        public string Apraksts { get; set; }
        public string Status { get; set; }
        public string Atkartosana { get; set; }
        public DateTimeOffset Izveidots { get; set; }
        public DateTimeOffset Labots { get; set; }
        public bool IrIzpilditsSodien { get; set; }
    }
}
