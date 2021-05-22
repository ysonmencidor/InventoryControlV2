using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models
{
    public class Group
    {
        public int GroupId { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
