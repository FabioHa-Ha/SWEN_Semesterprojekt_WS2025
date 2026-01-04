using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt.Exceptions
{
    public class UnkownMediaEntryException : Exception
    {
        public UnkownMediaEntryException(string message) : base(message) { }
    }
}
