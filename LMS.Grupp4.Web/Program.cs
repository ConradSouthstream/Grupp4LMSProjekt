using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Data;
using LMS.Grupp4.Data.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Grupp4.Web
{ 
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var WebHost = CreateHostBuilder(args).Build();
            using (var scope = WebHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();
                // TODO context.Database.Migrate();
                var config = services.GetRequiredService<IConfiguration>();
                var adminPW = config["AdminPw"];
                try
                {
                    Seed.InitAsync(services, adminPW).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex.Message, "Seed Fail");
                    throw;
                }

            }

            await WebHost.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
     


    }
}
