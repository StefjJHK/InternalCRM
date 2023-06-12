namespace BIP.InternalCRM.Primitives.ErrorHandling;

public abstract record Error(string Code, string Message)
{
    public static implicit operator string(Error error) => error.Code;
};