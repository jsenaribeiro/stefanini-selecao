using Microsoft.AspNetCore.Mvc;

public static class ActionResultExtensions
{
    public static T? ValueOf<T>(this IActionResult ar)
    {
        if (ar is not ObjectResult or) return default;

        if (or.Value is not T value) return default;

        return value;
    }

    public static int? GetStatusCode(this IActionResult result)
    {
        return result switch
        {
            ObjectResult objectResult => objectResult.StatusCode,
            StatusCodeResult statusCodeResult => statusCodeResult.StatusCode,
            _ => null
        };
    }
}