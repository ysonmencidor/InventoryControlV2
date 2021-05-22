using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models
{
    public class MenuAccess
    {
        public int MenuAccessId { get;set;}
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public String Name { get; set; }
    }
}
