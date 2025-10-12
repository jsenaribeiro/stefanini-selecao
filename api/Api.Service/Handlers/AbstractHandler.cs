
using Api.Domain;

namespace Api.Service.Handlers;

public abstract class AbstractHandler
{
   protected readonly IUnitOfWork unitOfWork;

   protected readonly IServiceProvider provider;

   protected AbstractHandler(IServiceProvider provider)
   {
      this.provider = provider;

      unitOfWork = provider.GetService(typeof(IUnitOfWork)) as IUnitOfWork
         ?? throw new ArgumentNullException(nameof(IUnitOfWork));
   }
}