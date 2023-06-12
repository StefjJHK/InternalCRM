namespace BIP.InternalCRM.Application.Extensions;

public static class StringExtensions
{
    public static bool IsEmpty(this string? str) => string.IsNullOrWhiteSpace(str);

    public static bool IsNotEmpty(this string? str) => !string.IsNullOrWhiteSpace(str);
}