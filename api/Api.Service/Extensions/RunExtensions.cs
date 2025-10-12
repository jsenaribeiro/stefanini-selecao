using Microsoft.EntityFrameworkCore;

public static class RunExtensions
{
   public static void RunMigrations(this WebApplication app)
   {

#if RELEASE

      using (var scope = app.Services.CreateScope())
      {
         var db = scope.ServiceProvider.GetRequiredService<SqlContext>();

         var pendingMigrations = db.Database.GetPendingMigrations();
         if (pendingMigrations.Any())
         {
            Console.WriteLine("Aplicando migrations pendentes...");
            db.Database.Migrate();
            Console.WriteLine("Migrations aplicadas com sucesso!");
         }
         else
         {
            Console.WriteLine("Nenhuma migration pendente. Nada a aplicar.");
         }
      }

#endif

   }
}