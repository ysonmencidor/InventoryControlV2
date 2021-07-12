using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models.QneModels
{
    public class Department
    {
        public Guid Id { get; set; }
        public string AccountCode { get; set; }
        public string Description { get; set; }
    }
}
