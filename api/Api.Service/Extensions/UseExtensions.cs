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
}