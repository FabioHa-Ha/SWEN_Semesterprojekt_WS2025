using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt.Exceptions
{
    internal class InvalidRequestBodyException : Exception
    {
        public InvalidRequestBodyException(string message) : base(message) { }
    }
}
