namespace Api.Domain.Pessoas;

public class Pessoa : Entity
{
   public Pessoa() // EF
   {
      Nome = string.Empty;
      Nascimento = DateOnly.MinValue;
   }

   public Pessoa(string nome, DateOnly nascimento)
   {
      Nome = nome;
      Nascimento = nascimento;
   }
   
   public Pessoa(string nome, string nascimento)
   {
      Nome = nome;

      try { Nascimento = nascimento.ToDateOnly("dd/MM/yyyy"); }
      catch { throw Failure.Invalid("Nascimento", nascimento); }
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