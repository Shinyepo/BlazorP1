using BlazorP1.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;

namespace BlazorP1.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddBlazoredToast();
            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<IBananaService, BananaService>();
            builder.Services.AddScoped<IUnitService, UnitService>();
            builder.Services.AddScoped<IBattleService, BattleService>();
            builder.Services.AddScoped<ILeaderboardService, LeaderboardService>();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthProvider>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IBuildingService, BuildingService>();
            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();

            await builder.Build().RunAsync();
        }
    }
}
