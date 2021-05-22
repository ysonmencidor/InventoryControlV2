using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary
{
    public class AppConnectionString
    {
        public AppConnectionString(string value) => Value = value;

        public string Value { get; }
    }
}
