using System;
using System.IO;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace API
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = new ConfigurationBuilder();
      BuildConfig(builder);

      Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Build())
        .CreateLogger();

      Serilog.Debugging.SelfLog.Enable(Console.Error);

      try
      {
        Log.Logger.Information("Application Starting");
        var host = CreateHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope())
        {
          var services = scope.ServiceProvider;
          try
          {
            var context = services.GetRequiredService<ApplicationDbContext>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            context.Database.Migrate();
            Seed.SeedData(context, userManager).Wait();
          }
          catch (Exception ex)
          {
            Log.Error(ex, "An error occured during migration");
          }
        }

        host.Run();
      }
      catch (Exception ex)
      {
        Log.Fatal(ex, "Application start-up failed");
      }
      finally
      {
        Log.CloseAndFlush();
      }
    }

    private static void BuildConfig(IConfigurationBuilder builder)
    {
      builder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
            });
  }
}
