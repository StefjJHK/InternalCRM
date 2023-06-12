namespace BIP.InternalCRM.Application.Extensions;

public static class ObjectExtensions
{
    public static bool NotEquals(this object? objA, object? objB) => !Equals(objA, objB);

    public static bool NotEquals(this string? objA, string? objB) => !Equals(objA?.ToLower(), objB?.ToLower());

    public static bool NotEquals(this decimal valueA, decimal valueB) => !valueA.Equals(valueB);

    public static bool NotEquals(this DateTime valueA, DateTime valueB) => !valueA.Equals(valueB);
}