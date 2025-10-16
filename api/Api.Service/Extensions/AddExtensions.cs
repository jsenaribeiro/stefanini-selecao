using System.Text;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Api.Service.Handlers;
using System.Diagnostics;
using Api.Service.Middlers;
using Microsoft.AspNetCore.Mvc;

public static class AddExtensions
{
   public static IServiceCollection AddControllers(this IServiceCollection services)
   {
      void configureControllers(MvcOptions options) =>
         options.Filters.Add<JsonExceptionFilter>();

      services.AddControllers(configureControllers);

      return services;
   }

   public static IServiceCollection AddJwtBearer(this IServiceCollection services, IConfiguration configuration)
   {
      services.AddAuthentication(options =>
      {
         options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
         options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(options =>
      {
         var secretKey = configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("JWT SecretKey não está no appsettings.");

         var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

         options.TokenValidationParameters = new TokenValidationParameters
         {
            ValidateIssuer = false,
            ValidateLifetime = true,
            ValidateAudience = false,
            IssuerSigningKey = signingKey,
            ValidateIssuerSigningKey = true,
         };
      });

      return services;
   }

   public static IServiceCollection AddMediatorCQRS(this IServiceCollection services)
   {
      var assembly = Assembly.GetAssembly(typeof(PessoaHandler))!;

      services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

      return services;
   }

   public static IServiceCollection AddSqlContext(this IServiceCollection services, IConfiguration configuration, bool test = false)
   {
      bool isRunningMigration()
      {
         var processName = Process.GetCurrentProcess().ProcessName;
         return processName.Contains("ef", StringComparison.OrdinalIgnoreCase);
      }

      if (isRunningMigration() && test) test = false;

      var connection = configuration.GetConnectionString("DefaultConnection");

#if RELEASE
         services.AddDbContext<SqlContext>(opt => opt.UseSqlServer(connection));
#else
      if (test) services.AddDbContext<SqlContext>(opt => opt.UseInMemoryDatabase("db"));
      else services.AddDbContext<SqlContext>(opt => opt.UseSqlServer(connection));
#endif

      return services;
   }

   public static IServiceCollection AddSwagger(this IServiceCollection services, string version = "v1")
   {
      var scheme = new OpenApiSecurityScheme
      {
         Scheme = "bearer",
         BearerFormat = "JWT",
         Name = "Authorization",
         In = ParameterLocation.Header,
         Type = SecuritySchemeType.Http,
         Description = "Digite: Bearer {seu token}"
      };

      var requirement = new OpenApiSecurityRequirement
      {
         {
            new OpenApiSecurityScheme
            {
               Reference = new OpenApiReference
               {
                  Id = "Bearer",
                  Type = ReferenceType.SecurityScheme,
               }
            },
             Array.Empty<string>()
         }
      };

      var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
      var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
      var apiInfo = new OpenApiInfo { Title = "Lancamentos API", Version = version };

      services.AddSwaggerGen(config =>
      {
         config.SwaggerDoc(version, apiInfo);
         config.AddSecurityDefinition("Bearer", scheme);
         config.AddSecurityRequirement(requirement);
         config.IncludeXmlComments(xmlPath);
      });

      return services;
   }

   public static IServiceCollection AddHealthCheck(this IServiceCollection services, IConfiguration configuration)
   {
      var connection = configuration.GetConnectionString("DefaultConnection");

      services.AddHealthChecks().AddSqlServer(connection!);

      return services;
   }

   public static IServiceCollection AddCors(this IServiceCollection services, IConfiguration configuration)
   {
      var origins = configuration["allowedOrigins"] ?? "*";

      if (origins != "*")
         services.AddCors(options => options
            .AddDefaultPolicy(policy => policy
               .WithOrigins(origins)
               .AllowAnyHeader()
               .AllowAnyMethod()));

      return services;
   }
}