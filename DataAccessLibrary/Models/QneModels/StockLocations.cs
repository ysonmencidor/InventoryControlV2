using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models.QneModels
{
    public class StockLocations
    {
        public Guid Id { get; set; }
        public string LocationCode { get; set; }
        public bool IsActive { get; set; }
    }
}
