using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class UniqueAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        return string.IsNullOrWhiteSpace(ErrorMessage);
    }
}
