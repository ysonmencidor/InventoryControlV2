using Blazored.LocalStorage;
using Blazored.Modal;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using DataAccessLibrary.Models.QneServices;
using ICV2_Web.Authentication;
using ICV2_Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ICV2_Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddBlazoredLocalStorage();
            //builder.Services.AddBlazoredModal();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
            builder.Services.AddScoped<AppState>();



            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:43119") });
            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddScoped<SpinnerService>();
            builder.Services.AddScoped<SpinnerAutomaticallyHttpMessageHandler>();
            builder.Services.AddScoped(sp => {

                var accessTokenHandler = sp.GetRequiredService<SpinnerAutomaticallyHttpMessageHandler>();
                accessTokenHandler.InnerHandler = new HttpClientHandler();
                var uriHelper = sp.GetRequiredService<NavigationManager>();
                return new HttpClient(accessTokenHandler)
                {
                    BaseAddress = new Uri("http://localhost:43119")
                };
            });

            builder.Services
              .AddBlazorise(options =>
              {
                  options.ChangeTextOnKeyPress = true;
              })
              .AddBootstrapProviders()
              .AddFontAwesomeIcons();

            await builder.Build().RunAsync();
        }
    }
}
