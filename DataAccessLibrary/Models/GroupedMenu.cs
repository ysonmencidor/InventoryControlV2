using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models
{
    public class GroupedMenu
    {
        public int GroupedMenuId { get; set; }
        public int GroupId { get; set; }
        public int MenuId { get; set; }
    }
}
