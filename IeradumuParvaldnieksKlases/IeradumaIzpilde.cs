using System;
using System.Collections.Generic;
using System.Text;

namespace IeradumuParvaldnieksKlases
{
    public class IeradumaIzpilde
    {
        private Guid id;
        public Guid Id
        {
            get { return id; }
        }

        private Ieradums ieradums;
        public Ieradums Ieradums
        {
            get { return ieradums; }
        }

        private DateOnly datums;
        public DateOnly Datums
        {
            get { return datums; }
        }
        public IeradumaIzpilde(Ieradums ierad, DateOnly dat)
        {
            if (ierad == null)
            {
                throw new ArgumentException("Ieradumam jābūt norādītam.");
            }

            id = Guid.NewGuid();
            ieradums = ierad;
            datums = dat;
        }
    }
}

