using System.Text.RegularExpressions;

namespace Api.Domain.Pessoas;

public record CPF(ulong Numero)
{
	public CPF(string cpf) : this(ToNumber(cpf)) { }

	public CPF(long numero) : this((ulong)numero) { }

	public bool IsValid
	{
		get
		{
			string cpf = this.Numero.ToString().PadLeft(11, '0');
			string d = Regex.Replace(cpf ?? string.Empty, @"[^\d]", "");

			if (d.Length != 11 || d.All(c => c == d.First()))
				return false;

			int[] digitos = d.Select(c => (int)char.GetNumericValue(c)).ToArray();

			int resto1 = digitos.Take(9)
				 .Select((digito, i) => digito * (10 - i))
				 .Sum() % 11;

			int dv1 = resto1 < 2 ? 0 : 11 - resto1;

			if (digitos[9] != dv1)
				return false;

			int resto2 = digitos.Take(10)
				 .Select((digito, i) => digito * (11 - i))
				 .Sum() % 11;

			int dv2 = resto2 < 2 ? 0 : 11 - resto2;

			return digitos[10] == dv2;
		}
	}

	public static CPF Empty => new CPF(0);

	private static ulong ToNumber(string cpf)
	{
		var valor = Regex.Replace(cpf, @"\D+", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
		return ulong.Parse(valor);
	}
	
	public override string ToString()
	{
		var cpfString = this.Numero.ToString().PadLeft(11, '0');
		
		var cpfArray = new[]{
			cpfString.Substring(0, 3),
			cpfString.Substring(3, 3),
			cpfString.Substring(6, 3)
		};

		return $"{string.Join('.', cpfArray)}-{cpfString.Substring(9, 2)}";
	}
}