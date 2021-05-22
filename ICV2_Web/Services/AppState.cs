using ICV2_Web.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICV2_Web.Services
{
    public class AppState
    {
        public string selectedCompanyCode { get; set; } = "404";

        public void UpdateSelectedCompany(ComponentBase Source,string selectedComany)
        {
            this.selectedCompanyCode = selectedComany;
            NotifyStateChanged(Source, selectedComany);
        }

        public event Action<ComponentBase, string> StateChanged;

        public void NotifyStateChanged(ComponentBase Source, string Property) => StateChanged?.Invoke(Source, Property);

        public bool CompanyReady()
        {
            if (selectedCompanyCode != "404")
            {
                return true;
            }
            return false;
        }
        //public string WhosUserIsIn()
        //{
        //    if (authState.User.Identity.IsAuthenticated)
        //        return (authState).User.FindFirst(x => x.Type == System.Security.Claims.ClaimTypes.Name).Value;
        //    else string.Empty;


        //}

    }
}
