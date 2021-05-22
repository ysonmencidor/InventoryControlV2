using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary
{
    public class AuthenticatedUserModel
    {
        public int Id { get; set; }
        public string username { get; set; }
        public string RoleName { get; set; }
        public string AccessType { get; set; }
        public bool IsPasswordDefault { get; set; }
    }
}
