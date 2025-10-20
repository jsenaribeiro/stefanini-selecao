namespace Api.Domain.Pessoas;

public class Pessoa : Entity<Guid>
{
   public Pessoa(string nome, DateOnly nascimento)
   {
      Nome = nome;
      Nascimento = nascimento;
   }

   public Pessoa(string nome, string nascimento)
   {
      var invalids = new List<Invalid>();

      Nome = nome;

      if (string.IsNullOrWhiteSpace(nome))
         invalids.Add(Invalid.RequiredOf(nameof(Nome), nome));

      if (string.IsNullOrWhiteSpace(nascimento))
         invalids.Add(Invalid.RequiredOf(nameof(Nascimento), nascimento));

      if (invalids.Count > 0) throw Errors.Invalid(invalids.ToArray());

      try { Nascimento = nascimento.ToDateOnly("dd/MM/yyyy"); }
      catch { throw Errors.Invalid("Nascimento", nascimento); }
   }

   [Required]
   public string Nome { get; set; }

   public Sexo? Sexo { get; set; }

   [Unique]
   [CpfValidator]
   public string? CPF { get; set; }

   [EmailValidator]
   public string? Email { get; set; }

   [Required]
   public DateOnly Nascimento { get; set; }

   public string? Nacionalidade { get; set; }

   public Endereco? Endereco { get; set; }
}