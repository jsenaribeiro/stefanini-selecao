using System.Net;

namespace Api.Domain;

public class DomainException : Exception
{
   public int Status { get; set; }

   public Invalid[] Invalids { get; set; } = [];

   public DomainException(HttpStatusCode status, string message) : base(message) => Status = (int)status;

   public DomainException(int status, string message) : base(message) => Status = status;

   public DomainException(int status, string message, params string[] args)
      : base(Format(message, args)) { this.Status = status; }

   public DomainException(params Invalid[] invalids)
   {
      Status = 400;
      Invalids = invalids;
   }

   private static string Format(string text, params string[] args) => string.Format(text, args);
}

public class InvalidException(string field, string error, object? value = null) 
   : DomainException(new [] { new Invalid(field, value, error) }) { }