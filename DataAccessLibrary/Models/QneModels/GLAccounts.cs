using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models.QneModels
{
    public class GLAccounts
    {
        public string GLAccountCode { get; set; }
        public string Description { get; set; }
        public Guid AccountId { get; set; }
    }
}
