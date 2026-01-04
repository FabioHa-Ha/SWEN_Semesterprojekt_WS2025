using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semesterprojekt.DTOs
{
    public class ProfileDTO
    {
        public ProfileDTO(string email, string favoriteGenre)
        {
            this.email = email;
            this.favoriteGenre = favoriteGenre;
        }

        public string email {  get; set; }
        public string favoriteGenre { get; set; }
    }
}
