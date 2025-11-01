using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ClientBlazorAPI.Services;
using ClientBlazorAPI.Models;

namespace ClientBlazorAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7271/api/")
            });
            builder.Services.AddScoped<WSServiceUtilisateur>();

            //builder.Services.AddBlazorBootstrap();

            // Enregistrement des services
            builder.Services.AddScoped<WSServiceUtilisateur>();
            builder.Services.AddScoped<WSServiceNotation>();

            await builder.Build().RunAsync();
        }
    }
}
