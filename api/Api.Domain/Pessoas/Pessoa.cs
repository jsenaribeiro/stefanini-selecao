namespace Api.Domain.Pessoas;

public class Pessoa : Entity
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

   public string Nome { get; set; }

   public Sexo? Sexo { get; set; }

   [CpfValidation(false)]
   public string? CPF { get; set; }

   [EmailValidation(false)]
   public string? Email { get; set; }

   public DateOnly Nascimento { get; set; }

   public string? Nacionalidade { get; set; }
}