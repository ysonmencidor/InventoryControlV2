using DataAccessLibrary.ApiSettings;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary
{
    public static class QNEConnectionString
    {
        //private readonly IConfiguration configuration;

        //public static QNEConnectionString(IConfiguration configuration)
        //{
        //    this.configuration = configuration;
        //}

        public static QneConnectionSettings Settings { get; set; } = new QneConnectionSettings();

        public static string ChooseConnection(string company_code)
        {
            string IC_NUTRATECH_CODE = "01";
            string IC_APTHEALTH_CODE = "02";
            string IC_BIOCARE_CODE = "03";

            if (company_code == IC_NUTRATECH_CODE)
            {
                return Settings.NTConnection;
            }
            else if (company_code == IC_APTHEALTH_CODE)
            {
                return Settings.APTConnection;
            }
            else if (company_code == IC_BIOCARE_CODE)
            {
                return Settings.AVLConnection;
            }
            else
            {
                throw new System.InvalidOperationException("No qne connection string specified for company_code: " + company_code);
            }

        }
    }
}
