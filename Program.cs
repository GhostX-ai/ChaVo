using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChaVoV1.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ChaVoV1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            string path = Path.GetFullPath("wwwroot/log.txt");
            if (!File.Exists(path))
            {
                var file = File.Create(Path.GetFullPath("wwwroot/log.txt"));
                file.Close();
            }
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<DataContext>();
                    Seed.SeedData(context);
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    string text = DateTime.Now.ToShortDateString() + "-" + ex.Message.ToString() + "\n-----------------------------------------";
                    File.WriteAllText(path, text);
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
