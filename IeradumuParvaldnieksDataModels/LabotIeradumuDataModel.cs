using System;
using System.Collections.Generic;
using System.Text;

namespace IeradumuParvaldnieksDataModels
{
    public class LabotIeradumuDataModel
    {
        public Guid IeradumsID { get; set; }
        public string Nosaukums { get; set; } = "";
        public string Apraksts { get; set; } = "";
        public string Atkartosana { get; set; } = "";
        public List<string> NedelasDienas { get; set; } = new();
    }
}
