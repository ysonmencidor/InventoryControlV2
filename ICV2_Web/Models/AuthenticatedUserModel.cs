using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICV2_Web.Models
{
    public class AuthenticatedUserModel
    {
        public string Access_Token { get; set; }
        public string Username { get; set; }
    }
}
