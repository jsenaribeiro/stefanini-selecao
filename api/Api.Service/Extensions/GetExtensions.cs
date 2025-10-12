using NLog.Extensions.Logging;
using NLog.Web;

public static class GetExtensions
{
   public static ILogger GetLogging(this WebApplicationBuilder builder)
   {
      builder.Logging.SetMinimumLevel(LogLevel.Trace);
      builder.Logging.AddNLog();
      builder.Host.UseNLog();

      var loggerFactory = LoggerFactory.Create(b =>
      {
         b.ClearProviders();
         b.AddNLog();
      });

      return loggerFactory.CreateLogger<Program>();
   }

   public static IConfiguration GetConfiguration(this WebApplicationBuilder builder)
   {
      var env = builder.Environment;
      var configuration = builder.Configuration;
      var json = $"appsettings.{env.EnvironmentName}.json";

      return new ConfigurationBuilder()
         .AddConfiguration(configuration)
         .AddJsonFile($"appsettings.json", true)
         .AddJsonFile(json, optional: true)
         .AddEnvironmentVariables()
         .Build();
   }
}