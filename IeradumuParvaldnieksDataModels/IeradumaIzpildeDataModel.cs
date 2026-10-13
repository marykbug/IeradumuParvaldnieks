using System;
using System.Collections.Generic;
using System.Text;

namespace IeradumuParvaldnieksDataModels
{
    public class IeradumaIzpildeDataModel
    {
        public Guid Id { get; set; }
        public Guid IeradumsID { get; set; }
        public DateOnly Datums { get; set; }
    }
}
