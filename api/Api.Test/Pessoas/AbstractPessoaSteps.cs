using Api.Domain.Pessoas;
using TechTalk.SpecFlow;

namespace Api.Test.Pessoas;

public abstract class AbstractPessoaSteps : AbstractSteps
{
   public object? GetValueOf<T>(string campo, object? valor)
   {
      if (campo == "Nascimento")
      {
         if (valor is null) valor = default(DateOnly);
         else if (valor is string s && s == "") valor = default(DateOnly);
         else if (valor?.ToString() == "00/00/0000") valor = default(DateOnly);
         else valor = valor!.ToString()!.ToDateOnly("dd/MM/yyyy");
      }

      if (campo == "Sexo" && valor is string sexo)
      {
         if (string.IsNullOrWhiteSpace(sexo)) valor = null;
         else if (Enum.TryParse<Sexo>(sexo, out var s)) valor = s;
         else valor = null;
      }

      return valor;
   }

   public Pessoa[] GetInstantiationOf(Table table)
   {
      var pessoas = new List<Pessoa>();

      foreach (var row in table.Rows)
      {
         var nome = row["nome"];
         var nascimento = row["nascimento"].ToDateOnly("dd/MM/yyyy");
         var sexo = row["sexo"].ToUpper() == "M" ? Sexo.M : Sexo.F;

         var pessoa = new Pessoa(nome, nascimento)
         {
            Sexo = sexo,
            CPF = row["cpf"],
            Email = row["email"],
            Nacionalidade = row["nacionalidade"]
         };

         pessoas.Add(pessoa);
      }

      return pessoas.ToArray();
   }
}