using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.NetworkInformation;

namespace Api.Domain;

public static class Failure
{
   public static DomainException NotFound(string nome) => new(404, Messages.NAO_ENCONTRADO, nome);

   public static DomainException Invalid(params Invalid[] invalids) => new(invalids);

   public static DomainException Invalid(string field, string value) =>
      new(new Invalid[] { new Invalid(field, value, string.Format(Messages.INVALIDO, field)) });

   public static DomainException Unavailable(string servico) => new(500, Messages.INACESSIVEL, servico);

   public static DomainException From(string text, int status, params string[] args) => new(status, text, args);
}