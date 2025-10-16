using System.Globalization;

public static class UseExtensions
{
   public static void UseSwagger(this WebApplication app, bool isDevelopmentOnly)
   {
      if (!isDevelopmentOnly || app.Environment.IsDevelopment())
      {
         app.UseDeveloperExceptionPage();
         app.UseSwagger();
         app.UseSwaggerUI();
      }
      else
      {
         app.UseExceptionHandler("/Error");
         app.UseHttpsRedirection();
      }
   }

   public static void UseLocalization(this WebApplication app, string idioma)
   {
      var idiomas = new[] { "pt-BR", "en-US" };

      app.UseRequestLocalization(opcoes =>
      {
         opcoes.SetDefaultCulture(idioma)
               .AddSupportedCultures(idiomas)
               .AddSupportedUICultures(idiomas);
      });

      CultureInfo culturaBrasileira = new(idioma);
      CultureInfo.DefaultThreadCurrentCulture = culturaBrasileira;
      CultureInfo.DefaultThreadCurrentUICulture = culturaBrasileira;
   }


   public static void UseApiControllers(this WebApplication app)
   {
      app.UseRouting();
      app.UseCors(true);
      app.UseResponseCaching();
      app.UseAuthentication();
      app.UseAuthorization();
      app.MapControllers();
      app.MapHealthChecks("/health");
   }

   public static void UseCors(this WebApplication app, bool enable)
   {
      if (enable) app.UseCors(x =>
      {
         x.AllowAnyHeader();
         x.AllowAnyMethod();
         x.AllowAnyOrigin();
      });
   }
}