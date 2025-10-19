using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt.DTOs
{
    public class JwtDTO
    {
        public JwtDTO(string token)
        {
            this.token = token;
        }

        public string token {  get; set; }
    }
}
