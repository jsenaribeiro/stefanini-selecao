using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.NetworkInformation;

namespace Api.Domain;

public static class Errors
{
   public static DomainException NotFound(string nome) => new(404, Messages.NAO_ENCONTRADO, nome);

   public static DomainException Invalid(params Invalid[] invalids) => new(invalids);

   public static DomainException Invalid(string field, string value) =>
      new(new[] { new Invalid(field, value, string.Format(Messages.INVALIDO, field)) });

   public static DomainException Unavailable(string service) => new(500, Messages.INACESSIVEL, service);

   public static DomainException Required(string field) => new(300, Messages.OBRIGATORIO, field);

   public static DomainException From(string text, int status, params string[] args) => new(status, text, args);
}