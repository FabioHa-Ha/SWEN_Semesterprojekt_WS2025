using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt.DTOs
{
    public class TypeCountDTO
    {
        public TypeCountDTO(string type, int count)
        {
            this.type = type;
            this.count = count;
        }

        public string type {  get; set; }
        public int count { get; set; }
    }
}
