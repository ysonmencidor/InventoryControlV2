using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models.QneModels
{
    public class StockGroups
    {
        public Guid Id { get; set; }
        public string GroupCode { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int OptimisticLockField { get; set; }
        public int RowIndex { get; set; }
    }
}
