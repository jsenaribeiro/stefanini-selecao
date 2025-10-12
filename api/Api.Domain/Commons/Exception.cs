using System.Net;

namespace Api.Domain;

public class DomainException : Exception
{
   public DomainException(HttpStatusCode status, string message) : base(message)
   {
      this.Status = (int)status;
   }

   public DomainException(int status, string message) : base(message)
   {
      this.Status = status;
   }

   public DomainException(int status, string message, params string[] args)
      : base(Format(message, args))
   {
      this.Status = status;
   }

   public int Status { get; set; }

   private static string Format(string text, params string[] args) =>
      string.Format(text, args);

   public static DomainException NotFound(string nome) =>
      new(404, Messages.NAO_ENCONTRADO, nome);

   public static DomainException Invalid(string campo) =>
      new(400, Messages.INVALIDO, campo);
}