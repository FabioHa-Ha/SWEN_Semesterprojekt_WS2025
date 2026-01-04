using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt.Exceptions
{
    public class UnkownRatingException : Exception
    {
        public UnkownRatingException(string message) : base(message) { }
    }
}
