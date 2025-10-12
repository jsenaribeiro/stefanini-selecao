namespace Api.Domain.Pessoas;

public class Pessoa : Entity
{
   public Pessoa() // EF
   {
      this.Nome = string.Empty;
      this.Nascimento = DateOnly.MinValue;
   }  

   public Pessoa(string nome, DateOnly nascimento)
   {
      this.Nome = nome;
      this.Nascimento = nascimento;
   }

   public string Nome { get; set; }

   public Sexo? Sexo { get; set; } 

   public CPF? CPF { get; set; } 

   public Email? Email { get; set; }

   public DateOnly Nascimento { get; set; }

   public string? Nacionalidade { get; set; }
}