using BIP.InternalCRM.Primitives.ErrorHandling;

namespace BIP.InternalCRM.Domain.DomainErrors;

public abstract record DomainError(
    string Code,
    string Message
) : Error(Code, Message);