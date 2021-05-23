using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models.QneModels
{
    public class stockbatchnumbers
    {
        public Guid Id { get; set; }
        public Guid StockId { get; set; }
        public string BatchNo { get; set; }
    }
}
