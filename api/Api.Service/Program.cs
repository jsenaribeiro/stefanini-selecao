using Api.Domain;
using Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.GetConfiguration();
var logger = builder.GetLogging();
var services = builder.Services;

logger.LogInformation("Iniciando serviço...");

try
{
    services.AddControllers();
    services.AddMediatorCQRS();
    services.AddHttpContextAccessor();
    services.AddEndpointsApiExplorer();
    services.AddScoped<IUnitOfWork, UnitOfWork>();
    // services.AddJwtBearer(configuration);
    services.AddSqlContext(configuration, true);
    services.AddHealthCheck(configuration);
    services.AddCors(configuration);
    services.AddSwagger("v1");

    var app = builder.Build();

    app.UseSwagger(true);
    app.UseRouting();
    app.UseCors();
    app.UseResponseCaching();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.MapHealthChecks("/health");

    app.RunMigrations();
    app.Run();
}
catch (Exception ex)
{
    logger.LogError(ex, "Erro na inicialização");
    throw;
}
finally
{
    logger.LogError("Serviço encerrado.");
    NLog.LogManager.Shutdown();
}