using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Api.Domain;
using Api.Domain.Pessoas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public class H2Context : DbContext
{
   public DbSet<Pessoa> Pessoas { get; set; }

   public H2Context(DbContextOptions<H2Context> dco, ILogger<H2Context> logger) : base(dco)
   {
      logger.LogInformation("H2Context");

      if (Database.IsInMemory()) Database.EnsureCreated();

#if RELEASE

      else
      {
         logger.LogInformation("Criando configuration condicional...");

         var config = new ConfigurationBuilder()
             .SetBasePath(AppContext.BaseDirectory)
             .AddJsonFile("appsettings.Development.json", optional: true)
             .AddJsonFile("appsettings.json", optional: false)
             .Build();

         var connectionString = config.GetConnectionString("DefaultConnection") ?? "";
         var connectionStringLog = connectionString
            .Split("Password")[0]
            .Split("password")[0];

         logger.LogInformation("ConnectionString: " + connectionStringLog);

         var optionsBuilder = new DbContextOptionsBuilder<H2Context>();

         optionsBuilder.UseMy(connectionString, options =>
         {
            options.EnableRetryOnFailure(
               maxRetryCount: 10,
               maxRetryDelay: TimeSpan.FromSeconds(30),
               errorNumbersToAdd: null
            );
         });
      }
   
#endif

   }

   protected override void OnModelCreating(ModelBuilder mb)
   {
      var currentAssembly = Assembly.GetExecutingAssembly();

      mb.ApplyConfigurationsFromAssembly(currentAssembly);

      if (Database.IsInMemory())
      {
         var newPessoa = (string nome, string data) => new Pessoa(nome, data)
         {
            Id = Guid.NewGuid(),
            Log = null
         };

         var fulano = newPessoa("Fulano", "2001-01-01");
         var beltrano = newPessoa("Beltrano", "2002-02-02");
         var sicrano = newPessoa("Sicrano", "2003-03-03");

         mb.Entity<Pessoa>().HasData(fulano, beltrano, sicrano);
      }

      base.OnModelCreating(mb);
   }

   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
   {
      if (optionsBuilder.IsConfigured) return;

      else optionsBuilder
         .EnableSensitiveDataLogging()
         .UseSnakeCaseNamingConvention()
         .LogTo(Console.WriteLine, LogLevel.Information);
   }
}