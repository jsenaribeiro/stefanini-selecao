
using Api.Domain;

namespace Api.Service.Handlers;

public abstract class AbstractHandler
{
   protected readonly IServiceProvider provider;

   protected readonly IUnitOfWork unitOfWork;

   protected AbstractHandler(IServiceProvider provider)
   {
      this.provider = provider;

      unitOfWork = provider.GetService(typeof(IUnitOfWork)) as IUnitOfWork
         ?? throw new ArgumentNullException(nameof(IUnitOfWork));
   }
}