using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt.DTOs
{
    internal class ErrorDTO
    {
        public ErrorDTO(string error) 
        {
            this.error = error;
        }
        public string error { get; set; }
    }
}
