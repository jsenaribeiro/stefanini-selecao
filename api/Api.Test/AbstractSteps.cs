

using Api.Domain;
using Api.Infrastructure;
using Api.Service.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TechTalk.SpecFlow;

public abstract class AbstractSteps : IDisposable
{
   protected IServiceScope scope;

   protected IUnitOfWork unitOfWork;

   protected IServiceProvider provider;

   protected DefaultHttpContext? httpContext;

   protected AbstractSteps()
   {
      var settings = new Dictionary<string, string>
      {
         {"AllowedHosts", "*"},
         {"Logging:LogLevel:Default", "Information"},
         {"Logging:LogLevel:Microsoft.AspNetCore", "Warning"},
         {"ConnectionStrings:DefaultConnection", ""}
      };

      var configuration = new ConfigurationBuilder()
         .AddInMemoryCollection(settings!)
         .Build();

      this.provider = new ServiceCollection()
         .AddScoped<PessoaHandler>()
         .AddScoped<IUnitOfWork, UnitOfWork>()
         .AddSingleton(typeof(ILogger<>), typeof(LoggerInMemory<>))
         .AddSingleton(AddHttpContext)
         .AddSingleton(configuration)
         .AddSqlContext(configuration, true)
         .AddLogging()
         .AddMediatorCQRS()
         .BuildServiceProvider();

      this.scope = this.provider.CreateScope();

      this.unitOfWork = this.provider
         .GetRequiredService<IUnitOfWork>();
   }

   private Func<IServiceProvider, IHttpContextAccessor> AddHttpContext =>
      (IServiceProvider sp) => new HttpContextAccessor { HttpContext = httpContext };

   [AfterScenario]
   public void Dispose() => scope?.Dispose();

   [AfterScenario]
   [BeforeScenario]
   protected abstract Task ClearScenario();
}