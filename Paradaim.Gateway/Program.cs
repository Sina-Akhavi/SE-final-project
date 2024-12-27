using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Paradaim.Gateway.GYM.Interfaces;
using Paradaim.Gateway.GYM; 
using Paradaim.BaseGateway;
using Paradaim.Gateway.Models;

namespace Paradaim.Gateway
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Create a host builder
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    // Configure DbContext for Entity Framework
                    services.AddDbContext<ParadaimDbContext>(options =>
                        options.UseMySql(
                            "Server=116.203.210.161;Database=ParadaimDB;User=Paradaim;Password=I&Youmm68",
                            new MySqlServerVersion(new Version(8, 0, 31))
                        ));

                    // Add Identity services
                    services.AddIdentity<IdentityUser, IdentityRole>()
                        .AddEntityFrameworkStores<ParadaimDbContext>()
                        .AddDefaultTokenProviders();

                    // Register IFaqGateway and FaqGateway
                    services.AddTransient<IFaqGateway, FaqGateway>(); // You can also use AddScoped or AddSingleton based on your needs
                })
                .Build();

            // Use the service provider to access the DbContext
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            // Retrieve the DbContext from DI
            var context = services.GetRequiredService<ParadaimDbContext>();
            Console.WriteLine("Gateway Layer Initialized Successfully!");

            // Optionally, run database migrations if needed
            await context.Database.MigrateAsync();
        }
    }
}
